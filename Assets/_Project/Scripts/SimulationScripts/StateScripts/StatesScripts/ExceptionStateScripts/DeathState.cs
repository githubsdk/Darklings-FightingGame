using UnityEngine;

public class DeathState : State
{
    public override void UpdateLogic(PlayerNetwork player)
    {
        if (!player.enter)
        {
            player.enter = true;
            player.animationFrames = 0;
            player.player.StopShakeCoroutine();
            GameplayManager.Instance.RoundOver(false);
            GameSimulation.IntroFrame = 360;
            player.health = 1;
            if (!SceneSettings.IsTrainingMode)
            {
                GameSimulation.Run = false;
            }
        }
        player.velocity = DemonicsVector2.Zero;
        player.animation = "Knockdown";
        player.animationFrames++;
        if (player.animationFrames >= 250)
        {
            if (player.otherPlayer.state != "Taunt")
            {
                player.otherPlayer.enter = false;
                player.otherPlayer.state = "Taunt";
            }
        }
        if (SceneSettings.IsTrainingMode)
        {
            if (player.animationFrames >= 105)
            {
                player.enter = false;
                player.state = "Idle";
            }
        }
        else
        {
            if (player.animationFrames >= 370)
            {
                player.otherPlayer.enter = false;
                player.otherPlayer.state = "Taunt";
                player.enter = false;
                player.state = "Taunt";
            }
        }
    }
}