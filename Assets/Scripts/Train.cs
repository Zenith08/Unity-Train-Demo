using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    public TrackPiece currentTrack;

    public float speed;
    public float positionOnTrack = 0.0f;
    public bool reversedOnTrack = false;

    private readonly float MODIFIER = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = currentTrack.GetPositionOnTrack(positionOnTrack, reversedOnTrack);
    }

    // Update is called once per frame
    void Update()
    {
        //Update speed
        if (Input.GetKeyDown("w"))
        {
            speed++;
            Debug.Log("Speed now " + speed);
        }
        else if (Input.GetKeyDown("s"))
        {
            speed--;
            Debug.Log("Speed now " + speed);
        }

        positionOnTrack += speed * MODIFIER;

        transform.position = currentTrack.GetPositionOnTrack(positionOnTrack, reversedOnTrack);
        if(currentTrack.UpdateActiveTrack(positionOnTrack, reversedOnTrack, out TrackPiece newTrack, out float newPosition))
        {
            // Detects when we are on a new piece of track so we can process it.
            //reversedOnTrack = newTrack.IsReversedOnTrack(currentTrack);
            if(newTrack.IsReversedOnTrack(currentTrack))
            {
                reversedOnTrack = !reversedOnTrack;
            }
            currentTrack = newTrack;
            positionOnTrack = newPosition;
        }
    }
}
