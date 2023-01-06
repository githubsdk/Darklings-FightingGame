using UnityEngine;


public class AttackState : State
{
    private static bool knock;
    private static bool b;
    private static bool opponentInCorner;
    public override void UpdateLogic(PlayerNetwork player)
    {
        player.dashDirection = 0;
        if (!player.enter)
        {
            player.animationFrames = 0;
            b = false;
            SetTopPriority(player);
            player.canChainAttack = false;
            player.enter = true;
            player.sound = player.attackNetwork.attackSound;
            player.animation = player.attackNetwork.name;
            player.attackFrames = DemonicsAnimator.GetMaxAnimationFrames(player.playerStats._animation, player.animation);
            opponentInCorner = false;
            if (DemonicsPhysics.IsInCorner(player.otherPlayer))
            {
                opponentInCorner = true;
            }
        }
        if (!player.isAir)
        {
            player.velocity = new DemonicsVector2(player.attackNetwork.travelDistance * (DemonicsFloat)player.flip, (DemonicsFloat)0);
        }
        else
        {
            player.velocity = new DemonicsVector2(player.velocity.x, player.velocity.y - (float)DemonicsPhysics.GRAVITY);
        }
        if (GameSimulation.Hitstop <= 0)
        {
            player.animationFrames++;
            player.attackFrames--;
            if (player.canChainAttack && player.start)
            {
                // if (!b)
                // {
                //     b = true;
                //     knockbackFrame = 0;
                //     start = player.position;
                //     end = new DemonicsVector2(player.position.x + (player.attack.knockbackForce.x * -player.flip), DemonicsPhysics.GROUND_POINT);
                // }
                // knock = true;
                if ((!(player.attackInput == InputEnum.Medium && player.isCrouch)))
                {
                    if (player.inputBuffer.inputItems[0].frame + 20 >= DemonicsWorld.Frame)
                    {
                        player.attackInput = player.inputBuffer.inputItems[0].inputEnum;
                        player.start = false;
                        player.isAir = false;
                        player.isCrouch = false;
                        if (player.direction.y < 0)
                        {
                            player.isCrouch = true;
                        }
                        AttackSO atk = PlayerComboSystem.GetComboAttack(player.playerStats, player.attackInput, player.isCrouch, false);
                        player.attackNetwork = new AttackNetwork()
                        {
                            damage = atk.damage,
                            travelDistance = (DemonicsFloat)atk.travelDistance.x,
                            name = atk.name,
                            attackSound = atk.attackSound,
                            hurtEffect = atk.hurtEffect,
                            knockbackForce = (DemonicsFloat)atk.knockbackForce.x,
                            knockbackDuration = atk.knockbackDuration,
                            hitstop = atk.hitstop,
                            impactSound = atk.impactSound,
                            hitStun = atk.hitStun,
                            comboTimerStarter = player.attackInput == InputEnum.Heavy ? ComboTimerStarterEnum.Red : ComboTimerStarterEnum.Yellow
                        };
                        player.enter = false;
                        player.state = "Attack";
                    }
                }
            }
            // if (knock)
            // {
            //     if (opponentInCorner && !player.isAir)
            //     {
            //         if (player.attack.knockbackDuration > 0)
            //         {
            //             if (knockbackFrame <= player.attack.knockbackDuration)
            //             {
            //                 DemonicsFloat ratio = (DemonicsFloat)knockbackFrame / (DemonicsFloat)player.attack.knockbackDuration;
            //                 DemonicsFloat distance = end.x - start.x;
            //                 DemonicsFloat nextX = DemonicsFloat.Lerp(start.x, end.x, ratio);
            //                 DemonicsFloat baseY = DemonicsFloat.Lerp(start.y, end.y, (nextX - start.x) / distance);
            //                 DemonicsFloat arc = player.attack.knockbackArc * (nextX - start.x) * (nextX - end.x) / ((-0.25f) * distance * distance);
            //                 DemonicsVector2 nextPosition = new DemonicsVector2((DemonicsFloat)nextX, (DemonicsFloat)baseY + arc);
            //                 nextPosition = new DemonicsVector2((DemonicsFloat)nextX, (DemonicsFloat)player.position.y);
            //                 player.position = nextPosition;
            //                 knockbackFrame++;
            //             }
            //         }
            //     }
            // }
        }
        //ToJumpState(player);
        // ToJumpForwardState(player);
        ToIdleState(player);
        // ToIdleFallState(player);
    }
    private void ToJumpState(PlayerNetwork player)
    {
        if (player.attack.jumpCancelable)
        {
            if (player.direction.y > 0)
            {
                player.enter = false;
                player.isCrouch = false;
                player.isAir = false;
                GameSimulation.Hitstop = 0;
                player.state = "Jump";
            }
        }
    }
    private void ToJumpForwardState(PlayerNetwork player)
    {
        if (player.attack.jumpCancelable)
        {
            if (player.direction.y > 0 && player.direction.x != 0)
            {
                player.jumpDirection = (int)player.direction.x;
                player.enter = false;
                player.isCrouch = false;
                player.isAir = false;
                GameSimulation.Hitstop = 0;
                player.state = "JumpForward";
            }
        }
    }
    private void ToIdleFallState(PlayerNetwork player)
    {
        if (player.isAir && (DemonicsFloat)player.position.y <= DemonicsPhysics.GROUND_POINT && (DemonicsFloat)player.velocity.y <= (DemonicsFloat)0)
        {
            knock = false;
            player.isCrouch = false;
            player.isAir = false;
            player.enter = false;
            player.state = "Idle";
        }
    }
    private void ToIdleState(PlayerNetwork player)
    {
        if (player.attackFrames <= 0)
        {
            player.start = false;
            // knock = false;
            player.enter = false;
            player.isCrouch = false;
            player.isAir = false;
            player.state = "Idle";
            // if (player.isAir)
            // {
            //     player.attackInput = InputEnum.Direction;
            //     player.isCrouch = false;
            //     player.isAir = false;
            //     player.state = "Fall";
            // }
            // else
            // {
            //     if (player.direction.y < 0)
            //     {
            //         player.attackInput = InputEnum.Direction;
            //         player.isCrouch = false;
            //         player.isAir = false;
            //         player.state = "Crouch";
            //     }
            //     else
            //     {
            //         player.attackInput = InputEnum.Direction;
            //         player.isCrouch = false;
            //         player.isAir = false;
            //         player.state = "Idle";
            //     }
            // }
        }
    }
    public override bool ToHurtState(PlayerNetwork player, AttackSO attack)
    {
        player.enter = false;
        if (player.attack.hasSuperArmor && !player.player.PlayerAnimator.InRecovery())
        {
            GameSimulation.Hitstop = attack.hitstop;
            player.player.PlayerAnimator.SpriteSuperArmorEffect();
            player.player.SetHealth(player.player.CalculateDamage(attack));
            player.player.StartShakeContact();
            player.player.PlayerUI.Damaged();
            return false;
        }
        if (attack.causesKnockdown)
        {
            player.state = "Airborne";
        }
        else
        {
            if (attack.knockbackArc == 0 || attack.causesSoftKnockdown)
            {
                player.state = "Hurt";
            }
            else
            {
                player.state = "HurtAir";
            }
        }
        return true;
    }
}