using UnityEngine;

public class PlayerSimulation : MonoBehaviour
{
    [SerializeField] private Player _player = default;
    [SerializeField] private PlayerAnimator _playerAnimator = default;
    [SerializeField] private Audio _audio = default;
    [SerializeField] private CollisionVisualizer _hurtBoxVisualizer = default;
    [SerializeField] private CollisionVisualizer _hitBoxVisualizer = default;
    [SerializeField] private CollisionVisualizer _pushBoxVisualizer = default;
    [SerializeField] private DisconnectMenu _disconnectMenu = default;


    public void Simulate(PlayerNetwork playerGs, PlayerConnectionInfo info)
    {
        if (!string.IsNullOrEmpty(playerGs.sound))
        {
            _audio.Sound(playerGs.sound).Play();
            playerGs.sound = "";
        }
        if (!string.IsNullOrEmpty(playerGs.soundStop))
        {
            _audio.Sound(playerGs.soundStop).Stop();
            playerGs.soundStop = "";
        }
        _player.Simulate(playerGs, info);
        if (info.state == PlayerConnectState.Disconnected)
        {
            _player.PlayerUI.Disconnected();
        }
        _player.PlayerUI.SetArcana(playerGs.arcana);
        _playerAnimator.SetAnimation(playerGs.animation, playerGs.animationFrames);
        _playerAnimator.SetSpriteOrder(playerGs.spriteOrder);
        _hitBoxVisualizer.ShowBox(playerGs.hitbox);
        _hurtBoxVisualizer.ShowBox(playerGs.hurtbox);
        _pushBoxVisualizer.ShowBox(playerGs.pushbox);
    }
}