using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
public class DimensionFilter : MonoBehaviour
{
    public enum Dimension { One, Two }

    public Dimension dimension = Dimension.One;
    Collider2D collider;

    int startLayer;
    int currentDimensionLayer;
    int otherDimensionLayer;

    public Material startingMaterial;
    Renderer renderer;
    float lerpTime = 1f;
    float currentLerpTime;

    float lerpSpeed = 1.3f;

    bool wasEnabledLastFrame = false;

    public void Enable()
    {
        lerpSpeed = -lerpSpeed;
    }
    public void Disable()
    {
        lerpSpeed = -lerpSpeed;
    }
    void Start()
    {
        if (DimensionSwapping.currentDimension == dimension)
        {
            wasEnabledLastFrame = true;
        }
        renderer = GetComponent<Renderer>();
        startingMaterial = renderer.material;
        currentDimensionLayer = LayerMask.NameToLayer("CurrentDimension");
        otherDimensionLayer = LayerMask.NameToLayer("OtherDimension");
        collider = GetComponent<Collider2D>();
        RenderPipelineManager.beginFrameRendering += OnBeginFrameRendering;
        RenderPipelineManager.endFrameRendering += OnEndFrameRendering;
    }


    private void Update()
    {
        currentLerpTime += lerpSpeed * Time.deltaTime;
        currentLerpTime = Mathf.Clamp(currentLerpTime, 0, lerpTime);
        float perc = currentLerpTime / lerpTime;
        //renderer.material.SetFloat("_Fade", Mathf.Lerp(0f, 1f, perc));
    }

    void OnBeginFrameRendering(ScriptableRenderContext context, Camera[] cameras)
    {
        startLayer = gameObject.layer;
        if (DimensionSwapping.currentDimension == dimension)
        {
            collider.enabled = true;
            wasEnabledLastFrame = true;
            gameObject.layer = currentDimensionLayer;
        } else
        {
            collider.enabled = false;
            gameObject.layer = otherDimensionLayer;
        }
    }

    void OnEndFrameRendering(ScriptableRenderContext context, Camera[] cameras)
    {
        //gameObject.layer = startLayer;
    }
    void OnDestroy()
    {
        RenderPipelineManager.beginFrameRendering -= OnBeginFrameRendering;
        RenderPipelineManager.endFrameRendering -= OnEndFrameRendering;
    }
}
