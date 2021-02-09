using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingArrow : MonoBehaviour
{
    public GameObject hookPreviewPrefab;


    DistanceJoint2D joint;
    RopeBridge ropeBridge;
    LineRenderer lines;

    GameObject ropeVisualStartObject;
    RopePreview ropePreview;

    bool ropeButtonDown = false;
    bool ropeCreated = false;

    void Start()
    {
        ropeVisualStartObject = CreateEmpty(new Vector3(0, 0, 0));
        ropeBridge = GetComponent<RopeBridge>();
        joint = GetComponents<DistanceJoint2D>()[0];
        lines = GetComponent<LineRenderer>();

        ropePreview = new RopePreview(hookPreviewPrefab);
        ropePreview.CreatePreview();
    }

    private void LateUpdate()
    {

        if (ropeCreated)
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
                ropeVisualStartObject.transform.position = ray.point;
                ropeBridge.StartPoint = ropeVisualStartObject.transform;
                ropeBridge.EndPoint = transform;
                ropeBridge.StartRope(Vector2.Distance(transform.position, ropeVisualStartObject.transform.position));
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
        }
    }

    private void FixedUpdate()
    {
        if (ropeCreated)
        {
            ropeBridge.Simulate();
        }
    }

    private void Update()
    {
        bool ropeButtonDownThisFrame = Input.GetMouseButtonDown(0);
        if (ropeButtonDownThisFrame)
        {
            //CreatePreview();
            ropeButtonDown = true;
        }
        else if (Input.GetMouseButtonUp(0) || (!Input.GetMouseButton(0))) // && currentHookPreview != null))
        {
            //DeletePreview();
            ropeButtonDown = false;
        }

        RaycastHit2D ray = GetRay();

        if (ray && !ropeButtonDown)
        {
            ropePreview.UpdatePreview(ray.point, ray.normal);
        }

        if (ropeButtonDownThisFrame)
        {
            ShootArrow(ropePreview.previewPosition);
            ropeCreated = true;
        } else if (!ropeButtonDown)
        {
            RetractArrow();
            ropeCreated = false;
        }

        if (joint.enabled)
        {
            lines.enabled = true;
        } else
        {
            lines.enabled = false;
        }
    }

    GameObject CreateEmpty(Vector3 position)
    {
        GameObject empty = new GameObject();
        Rigidbody2D body = empty.AddComponent<Rigidbody2D>();
        body.bodyType = RigidbodyType2D.Static;
        empty.transform.position = position;
        return empty;
    }

    void UpdateRopeLength()
    {
        if (joint.enabled)
        {
            if (Input.GetKey(KeyCode.W))
            {
                joint.distance -= 10f * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                joint.distance += 10f * Time.deltaTime;
            }
        }
    }

    void ShootArrow(Vector2 position)
     {
        joint.autoConfigureDistance = true;
        joint.connectedAnchor = position;
        joint.enabled = true;
        joint.autoConfigureDistance = false;
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

    void RetractArrow()
    {
        joint.enabled = false;
    }


    public class RopePreview
    {
        public GameObject hookPreviewPrefab;
        public Vector2 previewPosition;
        GameObject currentHookPreview;

        public RopePreview(GameObject hookPreviewPrefab)
        {
            this.hookPreviewPrefab = hookPreviewPrefab;
        }

        public void UpdatePreview(Vector2 position, Vector2 normal)
        {
            if (currentHookPreview == null) CreatePreview();
            previewPosition = position;
            currentHookPreview.transform.position = new Vector3(position.x, position.y, -10);
            currentHookPreview.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg - 90);
        }

        public void CreatePreview()
        {
            currentHookPreview = Instantiate(hookPreviewPrefab);
        }

        public void DeletePreview()
        {
            Destroy(currentHookPreview);
        }
    }
}


