using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] private PlayerMotor motor;
    [SerializeField] private PlayerAnimator playerAnimator;

    private PlayerState currentState;

    public PlayerRunningState RunningState { get; private set; }
    public PlayerJumpingState JumpingState { get; private set; }
    public PlayerSlidingState SlidingState { get; private set; }
    public PlayerDeadState DeadState { get; private set; }

    private void Awake()
    {
        RunningState = new PlayerRunningState(this, motor, playerAnimator);
        JumpingState = new PlayerJumpingState(this, motor, playerAnimator);
        SlidingState = new PlayerSlidingState(this, motor, playerAnimator);
        DeadState = new PlayerDeadState(this, motor, playerAnimator);
    }

    private void Start()
    {
        ChangeState(RunningState);
    }

    private void Update()
    {
        currentState?.Tick();
    }

    public void ChangeState(PlayerState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public bool MoveLeft()
    {
        return currentState != null && currentState.MoveLeft();
    }

    public bool MoveRight()
    {
        return currentState != null && currentState.MoveRight();
    }

    public bool Jump()
    {
        return currentState != null && currentState.Jump();
    }

    public bool Slide()
    {
        return currentState != null && currentState.Slide();
    }

    public void Die()
    {
        ChangeState(DeadState);
    }
}