public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(PlayerStateMachine stateMachine, PlayerMotor motor, PlayerAnimator playerAnimator) : base(stateMachine, motor, playerAnimator)
    {
    }

    public override void Enter()
    {
        motor.Stop();
        playerAnimator.PlayDeath();
    }
}