using Demonics.Manager;
using UnityEngine;

public class GrabbedState : State
{
	[SerializeField] private GameObject _techThrowPrefab = default;
	private DeathState _deathState;
	private KnockdownState _knockdownState;
	private AirborneHurtState _airborneHurtState;
	private KnockbackState _knockbackState;

	private void Awake()
	{
		_deathState = GetComponent<DeathState>();
		_knockdownState = GetComponent<KnockdownState>();
		_airborneHurtState = GetComponent<AirborneHurtState>();
		_knockbackState = GetComponent<KnockbackState>();
	}

	public override void Enter()
	{
		base.Enter();
		_playerAnimator.Hurt();
		_playerAnimator.SetSpriteOrder(-1);
		_player.SetPushboxTrigger(true);
		_playerMovement.SetRigidbodyKinematic(true);
		_player.OtherPlayer.SetToGrabPoint(_player);
		_player.OtherPlayerStateManager.TryToThrowState();
	}

	public override bool ToKnockdownState()
	{
		_player.OtherPlayer.SetResultAttack(_player.playerStats.mThrow.damage);
		_player.Health -= _player.playerStats.mThrow.damage;
		_playerUI.SetHealth(_player.Health);
		_player.OtherPlayerUI.IncreaseCombo();
		_player.OtherPlayer.StopComboTimer();
		GameManager.Instance.HitStop(_player.playerStats.mThrow.hitstop);
		if (_player.Health <= 0)
		{
			ToDeathState();
		}
		_stateMachine.ChangeState(_knockdownState);
		return true;
	}

	public override bool ToGrabState()
	{
		ObjectPoolingManager.Instance.Spawn(_techThrowPrefab,new Vector2(transform.position.x, transform.position.y + 1.0f));
		_stateMachine.ChangeState(_knockbackState);
		_player.OtherPlayerStateManager.TryToKnockbackState();
		return true;
	}

	public override bool ToAirborneHurtState(AttackSO attack)
	{
		_airborneHurtState.Initialize(attack);
		_stateMachine.ChangeState(_airborneHurtState);
		return true;
	}

	private void ToDeathState()
	{
		_stateMachine.ChangeState(_deathState);
	}

	public override void Exit()
	{
		base.Exit();
		_player.transform.rotation = Quaternion.identity;
		_playerMovement.SetRigidbodyKinematic(false);
		_player.SetPushboxTrigger(false);
		_playerAnimator.SetSpriteOrder(0);
		_playerUI.UpdateHealthDamaged();
		_player.transform.SetParent(null);
	}
}