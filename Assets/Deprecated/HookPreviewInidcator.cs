using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookPreviewInidcator : MonoBehaviour
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
                currentHookPreview.transform.position =new Vector3(raycastHit.point.x, raycastHit.point.y, -10);
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
        } else
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
