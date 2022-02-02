using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothSwitchTrack : TrackPiece
{
    [Header("Switch Track Functionality")]
    public TrackPiece primaryDirection;
    public TrackPiece secondaryDirection;
    // TRUE is primary, false is Secondary
    public bool direction = false;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            direction = !direction;
            Debug.Log("Switch is now " + direction);
        }
    }

    public override bool UpdateActiveTrack(float currentDistance, bool reversedOnTrack, out TrackPiece onTrack, out float newDistance)
    {
        if(direction)
        {
            return primaryDirection.UpdateActiveTrack(currentDistance, reversedOnTrack, out onTrack, out newDistance);
        }
        else
        {
            return secondaryDirection.UpdateActiveTrack(currentDistance, reversedOnTrack, out onTrack, out newDistance);
        }
    }

    public override Vector3 GetPositionOnTrack(float distanceOnTrack, bool reversedOnTrack)
    {
        if(direction)
        {
            return primaryDirection.GetPositionOnTrack(distanceOnTrack, reversedOnTrack);
        }
        else
        {
            return secondaryDirection.GetPositionOnTrack(distanceOnTrack, reversedOnTrack);
        }
    }

    public override bool IsReversedOnTrack(TrackPiece oldTrack)
    {
        // Never called
        Debug.LogError("This really should never have been called.");
        return false;
    }

    public virtual TrackPiece GetEffectiveTrack()
    {
        if(direction)
        {
            return primaryDirection;
        }
        else
        {
            return secondaryDirection;
        }
    }

    public virtual TrackPiece GetOffsideTrack()
    {
        if(!direction)
        {
            return primaryDirection;
        }
        else
        {
            return secondaryDirection;
        }
    }

    public override Vector3 CalculatePosition(float distance)
    {
        // Can be called in unusual situaitons.
        Debug.LogError("Calculate Postion should not be called for switch tracks");
        if(direction)
        {
            return primaryDirection.CalculatePosition(distance);
        }
        else
        {
            return secondaryDirection.CalculatePosition(distance);
        }
    }

    public override float GetTrackLength()
    {
        if(direction)
        {
            return primaryDirection.GetTrackLength();
        }
        else
        {
            return secondaryDirection.GetTrackLength();
        }
    }
}
