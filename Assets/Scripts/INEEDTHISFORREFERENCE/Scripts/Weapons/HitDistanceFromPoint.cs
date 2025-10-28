using System;

public struct HitDistanceFromPoint : IComparable<HitDistanceFromPoint>
{
    public int index;
    public float distance;

    public int CompareTo(HitDistanceFromPoint other)
    {
        return distance.CompareTo(other.distance);
    }
}