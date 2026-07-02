public abstract class PlayerState
{
    protected readonly PlayerStateMachine stateMachine;
    protected readonly PlayerMotor motor;
    protected readonly PlayerAnimator playerAnimator;

    protected PlayerState(PlayerStateMachine stateMachine, PlayerMotor motor, PlayerAnimator playerAnimator)
    {
        this.stateMachine = stateMachine;
        this.motor = motor;
        this.playerAnimator = playerAnimator;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Tick() { }

    public virtual bool MoveLeft() => false;
    public virtual bool MoveRight() => false;
    public virtual bool Jump() => false;
    public virtual bool Slide() => false;
}