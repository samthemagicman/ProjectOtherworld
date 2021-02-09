using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeJoint : MonoBehaviour
{
    HingeJoint2D joint = null;

    private void Update()
    {
        if (joint != null)
        {
            //line.SetPosition(0, transform.position);// transform.TransformPoint(joint.anchor.x, joint.anchor.y, 0));
            //line.SetPosition(1, joint.connectedBody.transform.position);//joint.connectedBody.transform.TransformPoint(joint.connectedAnchor.x, joint.connectedAnchor.y, 0));
        }
    }

    public void SetConnectedRope(GameObject connectedRope, float distance)
    {
        joint = GetComponent<HingeJoint2D>();
        joint.connectedBody = connectedRope.GetComponent<Rigidbody2D>();
        joint.anchor = new Vector2(distance, 0);
        //joint.connectedAnchor = new Vector2(2.5f, 0);
        //joint.anchor = new Vector2(-2.5f, 0);
    }
}
