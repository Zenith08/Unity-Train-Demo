using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightTrack : TrackPiece
{
    public Transform start;
    public Transform end;

    public override Vector3 CalculatePosition(float distanceOnTrack)
    {
        return Vector3.Lerp(start.position, end.position, distanceOnTrack / GetTrackLength());
    }

    public override float GetTrackLength()
    {
        return Vector3.Distance(start.position, end.position);
    }
}
