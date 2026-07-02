public class PlayerJumpingState : PlayerState
{
    public PlayerJumpingState(PlayerStateMachine stateMachine, PlayerMotor motor, PlayerAnimator playerAnimator) : base(stateMachine, motor, playerAnimator)
    {
    }

    public override void Enter()
    {
        playerAnimator.PlayJump();
    }

    public override void Tick()
    {
        if (!motor.IsJumping)
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

    public override bool Slide()
    {
        if (!motor.CancelJumpAndSlide())
        {
            return false;
        }

        stateMachine.ChangeState(stateMachine.SlidingState);
        return true;
    }
}