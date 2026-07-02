using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerMotor playerMotor;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Obstacle obstacle)) return;
        if (!other.TryGetComponent(out ObstacleRealmResolver resolver)) return;
        if (!resolver.IsActiveInCurrentRealm()) return;
        if (DidPlayerAvoidObstacle(obstacle)) return;

        playerController.Die();
    }

    private bool DidPlayerAvoidObstacle(Obstacle obstacle)
    {
        switch (obstacle.RequiredAction)
        {
            case ObstacleRequiredAction.Jump:
                return playerMotor.IsJumping;

            case ObstacleRequiredAction.Slide:
                return playerMotor.IsSliding;

            case ObstacleRequiredAction.None:
                return false;

            default:
                return false;
        }
    }
}