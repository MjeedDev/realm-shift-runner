using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private static readonly int IsRunningHash = Animator.StringToHash("IsRunning");
    private static readonly int IsJumpingHash = Animator.StringToHash("IsJumping");
    private static readonly int IsSlidingHash = Animator.StringToHash("IsSliding");
    private static readonly int IsDeadHash = Animator.StringToHash("IsDead");

    public void PlayRun()
    {
        SetMovementBools(true, false, false);
    }

    public void PlayJump()
    {
        SetMovementBools(false, true, false);
    }

    public void PlaySlide()
    {
        SetMovementBools(false, false, true);
    }

    public void PlayDeath()
    {
        SetMovementBools(false, false, false);
        animator.SetBool(IsDeadHash, true);
    }

    private void SetMovementBools(bool isRunning, bool isJumping, bool isSliding)
    {
        animator.SetBool(IsRunningHash, isRunning);
        animator.SetBool(IsJumpingHash, isJumping);
        animator.SetBool(IsSlidingHash, isSliding);
    }
}