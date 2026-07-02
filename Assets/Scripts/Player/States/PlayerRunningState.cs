public class PlayerRunningState : PlayerState
{
    public PlayerRunningState(PlayerStateMachine stateMachine, PlayerMotor motor, PlayerAnimator playerAnimator) : base(stateMachine, motor, playerAnimator)
    {
    }

    public override void Enter()
    {
        playerAnimator.PlayRun();
    }

    public override bool MoveLeft()
    {
        return motor.MoveLeft();
    }

    public override bool MoveRight()
    {
        return motor.MoveRight();
    }

    public override bool Jump()
    {
        if (!motor.Jump())
        {
            return false;
        }

        stateMachine.ChangeState(stateMachine.JumpingState);
        return true;
    }

    public override bool Slide()
    {
        if (!motor.Slide())
        {
            return false;
        }

        stateMachine.ChangeState(stateMachine.SlidingState);
        return true;
    }
}