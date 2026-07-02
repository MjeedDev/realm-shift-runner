using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerStateMachine stateMachine;
    [SerializeField] private PlayerVFXController vfxController;

    public bool MoveLeft()
    {
        return stateMachine.MoveLeft();
    }

    public bool MoveRight()
    {
        return stateMachine.MoveRight();
    }

    public bool Jump()
    {
        return stateMachine.Jump();
    }

    public bool Slide()
    {
        return stateMachine.Slide();
    }

    public void Die()
    {
        stateMachine.Die();
        vfxController.PlayDeathVFX();
        gameManager.GameOver();
    }
}