using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TrackPiece : MonoBehaviour
{
    [Header("Base Track Continuity")]
    public TrackPiece nextForewards;
    public TrackPiece nextBackwards;

    // This is here for when tracks are part of a switch track, it lets us check for reversal correctly.
    public SmoothSwitchTrack parentPiece;

    protected virtual void Start()
    {
        if(parentPiece == null)
        {
            if(transform.parent != null && transform.parent.TryGetComponent<SmoothSwitchTrack>(out SmoothSwitchTrack newParent))
            {
                parentPiece = newParent;
            }
            else
            {
                parentPiece = null;
            }
        }
    }

    public virtual bool UpdateActiveTrack(float currentDistance, bool reversedOnTrack, out TrackPiece onTrack, out float newDistance)
    {
        if(currentDistance < 0)
        {
            if(nextBackwards == null)
            {
                Debug.LogError("End of track back, derail");
                onTrack = null;
                newDistance = 0f;
                return true;
            }
            if(reversedOnTrack)
            {
                nextForewards.UpdateActiveTrack(nextForewards.GetTrackLength()-Mathf.Abs(currentDistance), !IsReversedOnTrack(nextForewards), out onTrack, out newDistance);
            }
            else
            {
                // The xor is not required but included for consistency
                nextBackwards.UpdateActiveTrack(nextBackwards.GetTrackLength()-Mathf.Abs(currentDistance), IsReversedOnTrack(nextBackwards), out onTrack, out newDistance);
            }
            return true;
        }
        else if(currentDistance >= GetTrackLength())
        {
            if(nextForewards == null)
            {
                Debug.LogError("End of track, derail");
                onTrack = null;
                newDistance = 0f;
                return true;
            }
            Debug.Log("Getting foreward track not null");
            if(reversedOnTrack)
            {
                nextBackwards.UpdateActiveTrack(currentDistance-GetTrackLength(), !IsReversedOnTrack(nextBackwards), out onTrack, out newDistance);
            }
            else
            {
                nextForewards.UpdateActiveTrack(currentDistance-GetTrackLength(), IsReversedOnTrack(nextForewards), out onTrack, out newDistance);
            }
            return true;
        }
        else
        {
            onTrack = this;
            newDistance = currentDistance;
            return false;
        }
    }

    public virtual Vector3 GetPositionOnTrack(float distanceOnTrack, bool reversedOnTrack)
    {
        //Debug.Log("Getting position on track " + name + " length is " + GetTrackLength() + " and position is " + distanceOnTrack);
        if(distanceOnTrack < 0)
        {
            if(reversedOnTrack)
            {
                if(nextForewards == null)
                {
                    Debug.LogError("End of track but reversed, derail");
                    return Vector3.zero;
                }
                return nextForewards.GetPositionOnTrack(nextForewards.GetTrackLength() - distanceOnTrack, !IsReversedOnTrack(nextForewards));
            }
            else
            {
                if(nextBackwards == null)
                {
                    Debug.LogError("End of track back, derail");
                    return Vector3.zero;
                }
                return nextBackwards.GetPositionOnTrack(nextBackwards.GetTrackLength() - distanceOnTrack, IsReversedOnTrack(nextBackwards));
            }
        }
        else if(distanceOnTrack >= GetTrackLength())
        {
            if(reversedOnTrack)
            {
                if(nextBackwards == null)
                {
                    Debug.LogError("End of track but reversed, derail");
                    return Vector3.zero;
                }
                return nextBackwards.GetPositionOnTrack(distanceOnTrack-GetTrackLength(), !IsReversedOnTrack(nextBackwards));
            }
            else
            {
                if(nextForewards == null)
                {
                    Debug.LogError("End of track, derail");
                    return Vector3.zero;
                }
                return nextForewards.GetPositionOnTrack(distanceOnTrack-GetTrackLength(), IsReversedOnTrack(nextForewards));
            }
        }
        else
        {
            float effectivePostion = distanceOnTrack;
            if(reversedOnTrack)
            {
                effectivePostion = GetTrackLength() - distanceOnTrack;
            }
            return CalculatePosition(effectivePostion);
        }
    }

    public virtual bool IsReversedOnTrack(TrackPiece oldTrack)
    {
        if(oldTrack == this)
        {
            Debug.LogError("Is Reversed On Track was called on itself. This is a break.");
            return false;
        }
        Debug.Log("Reverse check within " + gameObject.name + " old track is " + oldTrack.gameObject.name);
        TrackPiece forwardCompare = nextForewards;
        if(nextForewards is SmoothSwitchTrack sstf)
        {
            Debug.Log("Next forward is switch, changing comparison from " + forwardCompare.gameObject.name);
            forwardCompare = sstf.GetEffectiveTrack();
            Debug.Log("Forward compare now " + forwardCompare.gameObject.name);
            if(oldTrack == this || oldTrack == forwardCompare || oldTrack == nextForewards) // we are dealing with the track in front
            {
                if(parentPiece != null)
                {
                    return forwardCompare.nextForewards == parentPiece || forwardCompare.nextForewards == this;
                }
                else
                {
                    return forwardCompare.nextForewards == this;
                }
            }
        }
        else if(forwardCompare == oldTrack)
        {
            // Narrow down that we are working forwards
            //Debug.Log("Comparison value is " + forwardCompare?.nextForewards.gameObject.name);
            if(parentPiece != null)
            {
                return forwardCompare.nextForewards == parentPiece || forwardCompare.nextForewards == this;
            }
            else
            {
                return forwardCompare.nextForewards == this;
            }
        }

        // Else go backwards
        TrackPiece backwardCompare = nextBackwards;
        if(nextBackwards is SmoothSwitchTrack sstb && nextBackwards == oldTrack)
        {
            backwardCompare = sstb.GetEffectiveTrack();
            if(oldTrack == this || oldTrack == backwardCompare || oldTrack == nextBackwards) // We are looking backwards
            {
                if(parentPiece != null)
                {
                    return backwardCompare.nextBackwards == parentPiece || backwardCompare.nextBackwards == this;
                }
                else
                {
                    return backwardCompare.nextBackwards == this;
                }
            }
        }
        else if(backwardCompare == oldTrack)
        {
            //Debug.Log("BackwardCompare is " + backwardCompare.gameObject.name);
            //Debug.Log("Comparison value is " + backwardCompare?.nextBackwards.gameObject.name);
            if(parentPiece != null)
            {
                return backwardCompare.nextBackwards == parentPiece || backwardCompare.nextBackwards == this;
            }
            else
            {
                return backwardCompare.nextBackwards == this;
            }
        }

        Debug.LogError("Something is wrong with the logic.");
        return false;

        //bool selfHasParent = parentPiece != null;
        //bool oldTrackParent = oldTrack.parentPiece != null;

        // if(!selfHasParent && !oldTrackParent)
        // {
        //     if((oldTrack == nextBackwards && oldTrack.nextBackwards == this) ||
        //        (oldTrack == nextForewards && oldTrack.nextForewards == this))
        //     {
        //         return true;
        //     }
        //     else
        //     {
        //         return false;
        //     }
        // }
    }

    public abstract Vector3 CalculatePosition(float distance);

    public abstract float GetTrackLength();
}
