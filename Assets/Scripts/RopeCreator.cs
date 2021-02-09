using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeCreator : MonoBehaviour
{
    public Rigidbody2D endJoint;
    public RopeJoint ropeJoint;
    public GameObject[] joints;
    int ropeCount = 15;
    float ropeLength = 0.5f;

    bool roped = false;
    GameObject startObject;
    GameObject endObject;

    DistanceJoint2D distanceJoint;

    RopeBridge ropeBridge;

    void Start()
    {
        ropeBridge = GetComponent<RopeBridge>();
        distanceJoint = GetComponents<DistanceJoint2D>()[0];
        startObject = CreateEmpty(new Vector3(0,0,0));
    }

    void StopRope()
    {
        roped = false;
      // distanceJoint.enabled = false;
       //Destroy(startObject);
       //Destroy(endObject);
    }

    void StartRope()
    {
        roped = true;
    }
    GameObject CreateEmpty(Vector3 position)
    {
        GameObject empty = new GameObject();
        Rigidbody2D body = empty.AddComponent<Rigidbody2D>();
        body.bodyType = RigidbodyType2D.Static;
        empty.transform.position = position;
        return empty;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (roped)
        {
            ropeBridge.DrawRope();
        }


        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D ray = Physics2D.Raycast(transform.position, mousePos - transform.position);

            if (ray && ray.transform.tag != "Unropable")
            {
                //endObject = CreateEmpty(transform.position);
                startObject.transform.position = ray.point;
                StartRope();
                ropeBridge.StartPoint = startObject.transform;
                ropeBridge.EndPoint = transform;
                ropeBridge.StartRope(Vector2.Distance(transform.position, startObject.transform.position));
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopRope();
        }

    }

    private void FixedUpdate()
    {
        if (roped)
        {
            ropeBridge.Simulate();
        }
    }
}

public class HookPreview : MonoBehaviour
{
    public GameObject hookPreviewPrefab;
    GameObject currentHookPreview;

    bool pausePreview = false;

    private void Start()
    {
        CreatePreview();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //CreatePreview();
            pausePreview = true;
        }
        else if (Input.GetMouseButtonUp(0) || (!Input.GetMouseButton(0) && currentHookPreview != null))
        {
            //DeletePreview();
            pausePreview = false;
        }

        if (currentHookPreview != null && !pausePreview)
        {
            RaycastHit2D raycastHit = GetRay();

            if (raycastHit)
            {
                currentHookPreview.transform.position = new Vector3(raycastHit.point.x, raycastHit.point.y, -10);
                currentHookPreview.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(raycastHit.normal.y, raycastHit.normal.x) * Mathf.Rad2Deg - 90);
            }
        }
    }

    RaycastHit2D GetRay()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D ray = Physics2D.Raycast(transform.position, mousePos - transform.position);

        if (ray && ray.transform.tag != "Unropable")
        {
            return ray;
        }
        else
        {
            return new RaycastHit2D();
        }
    }

    void CreatePreview()
    {
        currentHookPreview = Instantiate(hookPreviewPrefab);
    }

    void DeletePreview()
    {
        Destroy(currentHookPreview);
    }
}


/*
 * using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeCreator : MonoBehaviour
{
    public Rigidbody2D endJoint;
    public RopeJoint ropeJoint;
    LineRenderer line;
    public GameObject[] joints;
    int ropeCount = 15;
    float ropeLength = 0.5f;

    bool roped = false;
    GameObject startObject;
    GameObject endObject;

    DistanceJoint2D distanceJoint;

    void Start()
    {
        distanceJoint = GetComponents<DistanceJoint2D>()[0];
    }

    void StopRope()
    {
        roped = false;
        foreach (GameObject rope in joints)
        {
            Destroy(rope);
        }
      // distanceJoint.enabled = false;
       Destroy(startObject);
       //Destroy(endObject);
    }

    void StartRope()
    {
        //endJoint.gameObject.transform.position = startObject.transform.position;
        float distance = Vector2.Distance(transform.position, startObject.transform.position);
        int ropeCountDynamic = (int)Mathf.Ceil(distance / ropeLength);
        line = GetComponent<LineRenderer>();
        line.positionCount = ropeCountDynamic;
        GameObject lastRope = startObject.gameObject;
        joints = new GameObject[ropeCountDynamic];

        Vector3 direction = (startObject.gameObject.transform.position - transform.position).normalized;

        for (int i = 0; i < ropeCountDynamic; i++)
        {
            GameObject newRopeJoint = Instantiate(ropeJoint.gameObject, lastRope.transform.position - (direction * ropeLength), new Quaternion());
            newRopeJoint.GetComponent<RopeJoint>().SetConnectedRope(lastRope, ropeLength);

            joints[i] = newRopeJoint;
            lastRope = newRopeJoint;
        }
        endObject = lastRope;

       //distanceJoint.connectedBody = lastRope.GetComponent<Rigidbody2D>();
       //distanceJoint.enabled = true;
        roped = true;
    }
    GameObject CreateEmpty(Vector3 position)
    {
        GameObject empty = new GameObject();
        Rigidbody2D body = empty.AddComponent<Rigidbody2D>();
        body.bodyType = RigidbodyType2D.Static;
        empty.transform.position = position;
        return empty;
    }

    // Update is called once per frame
    void Update()
    {
        if (roped)
        {
            endObject.transform.position = transform.position;
            Vector2 lastPos = startObject.transform.position;
            for (var i = 0; i < joints.Length; i++)
            {
                line.SetPosition(i, lastPos);
                lastPos = joints[i].transform.position;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D ray = Physics2D.Raycast(transform.position, mousePos - transform.position);

            if (ray)
            {
                startObject = CreateEmpty(ray.point);
                //endObject = CreateEmpty(transform.position);
                StartRope();
            }
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            StopRope();
        }

    }
}
 */