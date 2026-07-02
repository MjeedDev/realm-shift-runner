using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private PlayerSettingsSO settings;
    [SerializeField] private DifficultyManager difficultyManager;
    [SerializeField] private LaneSystem laneSystem;

    private int currentLaneIndex;
    private float jumpTimer;
    private float slideTimer;

    private bool isStopped;
    private bool isDead;

    public bool IsSliding => slideTimer > 0f;
    public bool IsJumping => jumpTimer > 0f;
    public bool IsFalling => IsJumping && jumpTimer > settings.jumpDuration * 0.5f;

    private void Awake()
    {
        currentLaneIndex = laneSystem.CenterLaneIndex;
    }

    private void Update()
    {
        if (isStopped) return;

        if (isDead)
        {
            UpdateDeathFall();
            return;
        }

        MoveForward();
        MoveTowardsLane();
        UpdateJump();
        UpdateSlide();
    }

    public bool MoveLeft()
    {
        int targetLaneIndex = laneSystem.ClampLaneIndex(currentLaneIndex - 1);

        if (targetLaneIndex == currentLaneIndex) return false;

        currentLaneIndex = targetLaneIndex;
        return true;
    }

    public bool MoveRight()
    {
        int targetLaneIndex = laneSystem.ClampLaneIndex(currentLaneIndex + 1);

        if (targetLaneIndex == currentLaneIndex) return false;

        currentLaneIndex = targetLaneIndex;
        return true;
    }

    public bool Jump()
    {
        if (IsJumping || IsSliding) return false;

        jumpTimer = settings.jumpDuration;
        return true;
    }

    public bool Slide()
    {
        if (IsSliding || IsJumping) return false;

        slideTimer = settings.slideDuration;
        return true;
    }

    public bool CancelJumpAndSlide()
    {
        jumpTimer = 0f;
        slideTimer = settings.slideDuration;
        return true;
    }

    private void MoveForward()
    {
        transform.position += Vector3.forward * difficultyManager.CurrentForwardSpeed * Time.deltaTime;
    }

    private void MoveTowardsLane()
    {
        Vector3 targetPosition = laneSystem.GetLanePosition(currentLaneIndex);
        Vector3 currentPosition = transform.position;

        Vector3 desiredPosition = new Vector3(targetPosition.x, currentPosition.y, currentPosition.z);
        transform.position = Vector3.MoveTowards(currentPosition, desiredPosition, settings.laneChangeSpeed * Time.deltaTime);
    }

    private void UpdateJump()
    {
        if (!IsJumping) return;

        jumpTimer -= Time.deltaTime;

        float normalizedTime = 1f - (jumpTimer / settings.jumpDuration);
        float heightOffset = Mathf.Sin(normalizedTime * Mathf.PI) * settings.jumpHeight;

        Vector3 position = transform.position;
        position.y = heightOffset;
        transform.position = position;

        if (jumpTimer <= 0f)
        {
            position.y = 0f;
            transform.position = position;
        }
    }

    private void UpdateSlide()
    {
        if (!IsSliding) return;

        slideTimer -= Time.deltaTime;

        Vector3 position = transform.position;

        if (position.y > 0f)
        {
            position.y = Mathf.MoveTowards(position.y, 0f, settings.airSlideFallSpeed * Time.deltaTime);
            transform.position = position;
        }
    }

    private void UpdateDeathFall()
    {
        Vector3 position = transform.position;

        if (position.y > 0f)
        {
            position.y = Mathf.MoveTowards(position.y, 0f, settings.deathFallSpeed * Time.deltaTime);
            transform.position = position;
            return;
        }

        position.y = 0f;
        transform.position = position;

        isStopped = true;
    }

    public void Stop()
    {
        isDead = true;
        jumpTimer = 0f;
        slideTimer = 0f;
    }
}