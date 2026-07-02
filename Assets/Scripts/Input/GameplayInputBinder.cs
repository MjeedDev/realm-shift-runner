using UnityEngine;

public class GameplayInputBinder : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private RealmManager realmManager;

    [Header("Realm Shift")]
    [SerializeField] private float realmShiftDebounce = 0.2f;

    private float nextAllowedRealmShiftTime;

    private void OnEnable()
    {
        inputReader.OnMoveLeft += HandleMoveLeft;
        inputReader.OnMoveRight += HandleMoveRight;
        inputReader.OnJump += HandleJump;
        inputReader.OnSlide += HandleSlide;
        inputReader.OnRealmShift += HandleRealmShift;
    }

    private void OnDisable()
    {
        inputReader.OnMoveLeft -= HandleMoveLeft;
        inputReader.OnMoveRight -= HandleMoveRight;
        inputReader.OnJump -= HandleJump;
        inputReader.OnSlide -= HandleSlide;
        inputReader.OnRealmShift -= HandleRealmShift;
    }

    private void HandleMoveLeft()
    {
        if (gameManager.CurrentState != GameState.Playing) return;

        if (playerController.MoveLeft())
        {
            AudioManager.Play(AudioEventType.LaneChange);
        }
    }

    private void HandleMoveRight()
    {
        if (gameManager.CurrentState != GameState.Playing) return;

        if (playerController.MoveRight())
        {
            AudioManager.Play(AudioEventType.LaneChange);
        }
    }

    private void HandleJump()
    {
        if (gameManager.CurrentState != GameState.Playing) return;

        if (playerController.Jump())
        {
            AudioManager.Play(AudioEventType.Jump);
        }
    }

    private void HandleSlide()
    {
        if (gameManager.CurrentState != GameState.Playing) return;

        if (playerController.Slide())
        {
            AudioManager.Play(AudioEventType.Slide);
        }
    }

    private void HandleRealmShift()
    {
        if (gameManager.CurrentState != GameState.Playing) return;
        if (Time.time < nextAllowedRealmShiftTime) return;

        nextAllowedRealmShiftTime = Time.time + realmShiftDebounce;

        realmManager.ToggleRealm();
        AudioManager.Play(AudioEventType.RealmShift);
    }
}