using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[ExecuteAlways]
public class EditorInfo : MonoBehaviour
{
    public float posGridSnapping = 2;
    public float sizeGridSnapping = 2 ;
    public float colliderSizeOffset = 0;
    public GameObject partnerBlock; //used for the blocks other form in the other dimension
    private Vector2 lastSize;
    private Vector3 lastPos;
    DimensionFilter.Dimension lastDimension;

    private void Update()
    {
        if (Application.IsPlaying(gameObject))
        {
            Destroy(this);
        }
        if (partnerBlock)
        {
            GameObject partnerPartner = partnerBlock.GetComponent<EditorInfo>().partnerBlock;
            if (!partnerPartner) //assign partners.
            {
                partnerPartner = gameObject;
            }
            DimensionFilter dimensionFilter = GetComponent<DimensionFilter>();
            if (dimensionFilter)
            {
                if (dimensionFilter.dimension != lastDimension) //keep the partners in opposite dimensions
                {
                    partnerBlock.GetComponent<DimensionFilter>().dimension = lastDimension;
                }
                lastDimension = dimensionFilter.dimension;
            }
            
            
            Vector2 pSize = partnerBlock.GetComponent<BoxCollider2D>().size;
            Vector2 size = GetComponent<BoxCollider2D>().size;
            Debug.Log($"Size: {size}\n" + $"pSize: {pSize}\n" +
                $"LastSize: {lastSize}\n");
            //Debug.Log($"pPos: {partnerBlock.transform.position}, " + $"Pos: {transform.position}\n"+
            //    $"LastPos: {lastPos}");
            if (size != lastSize) // if my size changes, adjust the size of my partner
            {
                Debug.Log("size changed");
                pSize = size;
            }
            if (transform.position != lastPos) // same for pos
            {
                Debug.Log("pos changed");
                partnerBlock.transform.position = transform.position;
            }
            
            lastPos = transform.position;
            lastSize = size;
        }
    }
}
