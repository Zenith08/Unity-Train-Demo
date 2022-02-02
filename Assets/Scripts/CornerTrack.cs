using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerTrack : TrackPiece
{
    public float radius = 4.0f;

    public override float GetTrackLength()
    {
        return (Mathf.PI * 2 * radius) / 4.0f; //90 degree turn
    }

    public override Vector3 CalculatePosition(float distance)
    {
        float angle = distance / radius;
        float offset = transform.rotation.eulerAngles.y;
        float theta = angle + Mathf.Deg2Rad * offset - (Mathf.PI/2);
        return new Vector3(Mathf.Cos(theta)*radius, transform.position.y, Mathf.Sin(theta)*radius) + transform.position;
    }
}
