using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class TrackEditorHelper : MonoBehaviour
{
    public BoxCollider frontCollider;
    public BoxCollider backCollider;

    public TrackPiece helpedObject;

    private Vector3 lastPosition;
    private Quaternion lastRotation;

    private void Awake()
    {
        helpedObject = transform.parent.GetComponent<TrackPiece>();
    }

    // Update is called only when something moves
    private void Update()
    {
        if(!transform.position.Equals(lastPosition) || !lastRotation.Equals(transform.rotation))
        {
            // Front Collider
            Collider[] frontColliders = Physics.OverlapSphere(frontCollider.transform.position, 0.5f, Physics.AllLayers, QueryTriggerInteraction.Collide);
            //Debug.DrawLine(frontCollider.transform.position, frontCollider.transform.position + new Vector3(0.5f, 0.5f, 0.5f), Color.red, 5f);
            Debug.Log("Colliders are " + frontColliders.Length);
            if(frontColliders.Length != 0)
            {
                Debug.Log("Colliders found");
                foreach(Collider c in frontColliders)
                {
                    if(c == frontCollider)
                    {
                        // next
                        Debug.Log("Collider is self");
                    }
                    else if(c.gameObject.tag == "TrackHelper")
                    {
                        Debug.Log("Overlapping another track at the front");
                        Vector3 offset = frontCollider.transform.position - helpedObject.transform.position;
                        Debug.Log("Target position is " + c.transform.position + " offset is " + offset);
                        helpedObject.transform.SetPositionAndRotation(c.transform.position - offset, helpedObject.transform.rotation);

                        if(c.transform.parent.TryGetComponent<TrackEditorHelper>(out TrackEditorHelper teh))
                        {
                            helpedObject.nextForewards = teh.helpedObject;
                            teh.TrackConnectFrom(c, helpedObject);
                        }
                    }
                    else
                    {
                        Debug.Log("Collider is " + c.gameObject.name);
                    }
                }
            }

            // Duplicate but with back collider
            Collider[] backColliders = Physics.OverlapSphere(backCollider.transform.position, 0.5f, Physics.AllLayers, QueryTriggerInteraction.Collide);
            //Debug.DrawLine(backCollider.transform.position, backCollider.transform.position + new Vector3(0.5f, 0.5f, 0.5f), Color.red, 5f);
            Debug.Log("Colliders are " + backColliders.Length);
            if(backColliders.Length != 0)
            {
                Debug.Log("Colliders found");
                foreach(Collider c in backColliders)
                {
                    if(c == backCollider)
                    {
                        // next
                        Debug.Log("Collider is self");
                    }
                    else if(c.gameObject.tag == "TrackHelper")
                    {
                        Debug.Log("Overlapping another track at the front");
                        Vector3 offset = backCollider.transform.position - helpedObject.transform.position;
                        Debug.Log("Target position is " + c.transform.position + " offset is " + offset);
                        helpedObject.transform.SetPositionAndRotation(c.transform.position - offset, helpedObject.transform.rotation);

                        if(c.transform.parent.TryGetComponent<TrackEditorHelper>(out TrackEditorHelper teh))
                        {
                            helpedObject.nextBackwards = teh.helpedObject;
                            teh.TrackConnectFrom(c, helpedObject);
                        }
                    }
                    else
                    {
                        Debug.Log("Collider is " + c.gameObject.name);
                    }
                }
            }
        }
        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }

    public void TrackConnectFrom(Collider c, TrackPiece obj)
    {
        if(c == frontCollider)
        {
            helpedObject.nextForewards = obj;
        }
        else
        {
            helpedObject.nextBackwards = obj;
        }
    }
}
