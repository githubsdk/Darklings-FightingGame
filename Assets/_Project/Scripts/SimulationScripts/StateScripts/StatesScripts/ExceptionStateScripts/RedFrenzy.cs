public class RedFrenzyState : State
{
    public override void UpdateLogic(PlayerNetwork player)
    {
        if (!player.enter)
        {
            player.enter = true;
            player.sound = "Vanish";
            player.animationFrames = 0;
            player.animation = "RedFrenzy";
            player.attackFrames = DemonicsAnimator.GetMaxAnimationFrames(player.playerStats._animation, player.animation);
        }
        player.animationFrames++;
        player.attackFrames--;
        ToIdleState(player);
    }
    private void ToIdleState(PlayerNetwork player)
    {
        if (player.attackFrames <= 0)
        {
            player.enter = false;
            player.state = "Idle";
        }
    }
}