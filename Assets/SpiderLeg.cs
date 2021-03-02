using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderLeg : MonoBehaviour
{
    const float MIN_DIST = 100;

    HingeJoint2D joint1;
    HingeJoint2D joint2;
    CapsuleCollider2D hand;

    float len1 = 0f;
    float len2 = 0f;
    float len3 = 0f;
    public bool flipped;
    // Start is called before the first frame update
    void Start()
    {
        joint1 = GetComponentInChildren<HingeJoint2D>();
        joint2 = joint1.GetComponentInChildren<HingeJoint2D>();
        hand = joint2.GetComponentInChildren<CapsuleCollider2D>();

        len1 = joint1.transform.position.x;
        len2 = joint2.transform.position.x;
        len3 = hand.transform.position.x;

        if (flipped)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            joint1.GetComponent<SpriteRenderer>().flipX = true;
            joint2.GetComponent<SpriteRenderer>().flipX = true;

        }

    }

    // Update is called once per frame
    void Update()
    {
        updateIK(Input.mousePosition);
    }
    void updateIK(Vector2 targetPos)
    {
        Vector2 offset = targetPos - (Vector2)transform.position;
        float distToTarget = offset.magnitude;
        if(distToTarget < MIN_DIST)
        {
            offset = offset.normalized * MIN_DIST;
            distToTarget = MIN_DIST;
        }
        float baseRot = Vector2.Angle((Vector2)transform.position,targetPos);//need to make this account for NAN when less than min lenght
        float lenTotal = len1 + len2 + len3;
        float lenDummySide = (len1 + len2) * Mathf.Clamp01(distToTarget / lenTotal);

        Vector3 baseAngles = SSSCalc(lenDummySide, len3, distToTarget);
        Vector3 nextAngles = SSSCalc(len1, len2, lenDummySide);
        transform.Rotate(Vector3.forward * (baseAngles.y + nextAngles.y));
        //transform.RotateAroundLocal(Vector3.forward, baseAngles.y + nextAngles.y);
        //joint1.gameObject.transform.RotateAroundLocal(Vector3.forward, nextAngles.z);
        //joint2.gameObject.transform.RotateAroundLocal(Vector3.forward, baseAngles.z + nextAngles.x);
        //transform.rotation.Set(transform.rotation.x, transform.rotation.y,( baseAngles.y + nextAngles.y + baseRot), transform.rotation.w);
        //joint1.gameObject.transform.rotation.Set(transform.rotation.x, transform.rotation.y, (nextAngles.z), transform.rotation.w);
        //joint2.gameObject.transform.rotation.Set(transform.rotation.x, transform.rotation.y, (baseAngles.z + nextAngles.x + baseRot), transform.rotation.w);
    }

    float LawOfCos(float a, float b, float c)
    {
        if(2 * a * b == 0) // prevent div by zero
        {
            return 0;
        }
        return Mathf.Acos((a * a + b * b + c * c) / (2 * a * b));
    }
    Vector3 SSSCalc(float sideA,float sideB, float sideC)
    {
        if(sideC >= sideA + sideB) //if target is too far away, return zero
        {
            return Vector3.zero; //vector used to keep track of angles
        }
        float angleA = LawOfCos(sideB, sideC, sideA);
        float angleB = LawOfCos(sideC, sideA, sideB);
        float angleC = Mathf.PI - angleA - angleB;
        if (flipped)
        {
            angleA = -angleA;
            angleB = -angleB;
            angleC = -angleC;
        }
        return new Vector3(angleA, angleB, angleC);
    }
}
