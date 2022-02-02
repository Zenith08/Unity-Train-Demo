using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayLoco : RayTrain
{
    private float speed = 0;

    public float maxSpeedF = 5;
    public float maxSpeedR = 3;

    public string goFaster = "w";
    public string goSlower = "s";

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        //Update speed
        if (Input.GetKeyDown(goFaster))
        {
            speed++;
            speed = speed > maxSpeedF ? maxSpeedF : speed;
            Debug.Log("Speed now " + speed);
        }
        else if (Input.GetKeyDown(goSlower))
        {
            speed--;
            Debug.Log("Speed now " + speed);
            speed = speed < -maxSpeedR ? -maxSpeedR : speed;
        }
    }

    private void FixedUpdate()
    {
        if(trainAhead == null)
        {
            FixedRotationMovement(speed);
            if(trainBehind != null)
            {
                trainBehind.PullingFixedUpdate(speed);
            }
        }
    }
}
