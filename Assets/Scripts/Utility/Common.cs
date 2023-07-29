using System;

public enum FactionType
{
    Red = 0,
    Green = 1,
    Blue = 2
}

public enum MovementLane
{
    Left,
    Mid,
    Right
}

[Serializable]
public class LaneTransversePositionBinding
{
    public MovementLane lane;
    public float xPosition;
}