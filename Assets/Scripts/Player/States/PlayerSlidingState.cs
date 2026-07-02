public class PlayerSlidingState : PlayerState
{
    public PlayerSlidingState(PlayerStateMachine stateMachine, PlayerMotor motor, PlayerAnimator playerAnimator) : base(stateMachine, motor, playerAnimator)
    {
    }

    public override void Enter()
    {
        playerAnimator.PlaySlide();
    }

    public override void Tick()
    {
        if (!motor.IsSliding)
        {
            stateMachine.ChangeState(stateMachine.RunningState);
        }
    }

    public override bool MoveLeft()
    {
        return motor.MoveLeft();
    }

    public override bool MoveRight()
    {
        return motor.MoveRight();
    }
}