using UnityEngine;

public class LaneSystem : MonoBehaviour
{
    [SerializeField] private Transform leftLane;
    [SerializeField] private Transform centerLane;
    [SerializeField] private Transform rightLane;

    public int CenterLaneIndex => 1;
    public int MinLaneIndex => 0;
    public int MaxLaneIndex => 2;

    public Vector3 GetLanePosition(int laneIndex)
    {
        laneIndex = Mathf.Clamp(laneIndex, MinLaneIndex, MaxLaneIndex);

        return laneIndex switch
        {
            0 => leftLane.position,
            1 => centerLane.position,
            2 => rightLane.position,
            _ => centerLane.position
        };
    }

    public int ClampLaneIndex(int laneIndex)
    {
        return Mathf.Clamp(laneIndex, MinLaneIndex, MaxLaneIndex);
    }
}