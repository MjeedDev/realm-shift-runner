using UnityEngine;

public class ObstaclePatternDefinition : MonoBehaviour
{
    [SerializeField] private ObstaclePatternType patternType;

    public ObstaclePatternType PatternType => patternType;
}