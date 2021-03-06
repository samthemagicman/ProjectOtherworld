﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DimensionSwapping : MonoBehaviour
{
    public static DimensionFilter.Dimension currentDimension;

    public GameObject player;
    public Camera dimension2Camera;

    public Material secondDimensionPreviewMaterial;
    public Material firstDimensionPreviewMaterial;
    public Material screenDimensionPreviewMaterial;


    float lerpTime = 0.2f;
    float currentLerpTime;

    float lerpSpeed = 1f;

    public Volume dimensionPreviewVolume;
    Vignette vignette;
    ChromaticAberration chromaticAbberation;
    ColorAdjustments colorAdjustment;
    LensDistortion lensDistortion;

    bool previewingDimension = false;

    void Start()
    {
        DimensionFilter[] objects = Resources.FindObjectsOfTypeAll(typeof(DimensionFilter)) as DimensionFilter[];

        dimensionPreviewVolume.profile.TryGet(out vignette);
        dimensionPreviewVolume.profile.TryGet(out chromaticAbberation);
        dimensionPreviewVolume.profile.TryGet(out colorAdjustment);
        dimensionPreviewVolume.profile.TryGet(out lensDistortion);

        screenDimensionPreviewMaterial.SetFloat("InnerColorCircleSize", 0);
        screenDimensionPreviewMaterial.SetFloat("SaturatedCircleSize", 0);
    }



    // Update is called once per frame. Just in case you forgot
    void Update()
    {
        previewingDimension = Input.GetButton("PreviewDimension");

        currentLerpTime += lerpSpeed * Time.deltaTime / Time.timeScale;
        currentLerpTime = Mathf.Clamp(currentLerpTime, 0, lerpTime);
        float perc = currentLerpTime / lerpTime;

        vignette.intensity.value = Mathf.Lerp(0f, 0.4f, perc);
        chromaticAbberation.intensity.value = Mathf.Lerp(0f, 0.5f, perc);
        colorAdjustment.saturation.value = Mathf.Lerp(0f, -30f, perc);
        lensDistortion.intensity.value = Mathf.Lerp(0f, -0.3f, perc);

        secondDimensionPreviewMaterial.SetFloat("WorldCircleSize", Mathf.Lerp(-0.1f, 0.6f, perc));
        firstDimensionPreviewMaterial.SetFloat("WorldCircleSize", Mathf.Lerp(-0.1f, 0.6f, perc));


        secondDimensionPreviewMaterial.SetFloat("Transparency", Mathf.Lerp(0f, 0.2f, perc));
        firstDimensionPreviewMaterial.SetFloat("Transparency", Mathf.Lerp(0f, 0.2f, perc));

        bool changed = UpdateSaturationCircles();

        if (previewingDimension)
        {
            changed = true;
            lerpSpeed = Mathf.Abs(lerpSpeed);
        }
        else
        {
            lerpSpeed = -Mathf.Abs(lerpSpeed);
        }

        if (Input.GetButtonUp("PreviewDimension"))
        {
            changed = true;
            SwapDimension();
        }
        if (changed)
        {
            UpdateAllMaterials();
        }

    }

    void SwapDimension()
    {
        if (currentDimension == DimensionFilter.Dimension.One)
        {
            currentDimension = DimensionFilter.Dimension.Two;
        }
        else
        {
            currentDimension = DimensionFilter.Dimension.One;
        }
    }

    void UpdateAllMaterials()
    {

        DimensionFilter[] dimensions = Resources.FindObjectsOfTypeAll(typeof(DimensionFilter)) as DimensionFilter[];
        foreach (DimensionFilter dim in dimensions)
        {
            if (previewingDimension)
            {

                if (dim.dimension != currentDimension)
                {
                    dim.GetComponent<Renderer>().material = secondDimensionPreviewMaterial;
                }
                else
                {
                    dim.GetComponent<Renderer>().material = firstDimensionPreviewMaterial;
                }
            } else
            {
                if (dim.dimension == currentDimension)
                {
                    dim.GetComponent<Renderer>().material = dim.startingMaterial;
                } else
                {
                    if (dim.dimension != currentDimension)
                    {
                        dim.GetComponent<Renderer>().material = secondDimensionPreviewMaterial;
                    }
                    else
                    {
                        dim.GetComponent<Renderer>().material = firstDimensionPreviewMaterial;
                    }
                }
            }
        }
    }

    //listen, these variables are all the way down here because they're useless and no body loves them.
    float v = 0.0f;
    float v2 = 0.0f;

    bool UpdateSaturationCircles()
    {
        float currentSaturatedCircleSize = screenDimensionPreviewMaterial.GetFloat("SaturatedCircleSize");
        float saturatedCircleTargetSize = -0.63f;

        float currentColorCircleSize = screenDimensionPreviewMaterial.GetFloat("InnerColorCircleSize");
        float colorCircleTargetSize = -0.3f;

        //screenDimensionPreviewMaterial.SetFloat("SaturatedCircleSize", Mathf.SmoothStep(-0.63f, 0.6f, perc));

        if (previewingDimension)
        {
            screenDimensionPreviewMaterial.SetFloat("DistortionCircle", 0.6f);
            saturatedCircleTargetSize = 0.6f;
            if (currentDimension == DimensionFilter.Dimension.Two)
            {
                colorCircleTargetSize = 0.6f;
            }
        } else
        {
            screenDimensionPreviewMaterial.SetFloat("DistortionCircle", -0.2f);
        }

        if (currentDimension == DimensionFilter.Dimension.Two)
        {
            saturatedCircleTargetSize = 2f;

        }
        float saturatedCircleSize = Mathf.SmoothDamp(currentSaturatedCircleSize, saturatedCircleTargetSize, ref v, 0.05f, 10f, Time.deltaTime / Time.timeScale);
        float colorCircleSize = Mathf.SmoothDamp(currentColorCircleSize, colorCircleTargetSize, ref v2, 0.03f, 10f, Time.deltaTime / Time.timeScale);
        screenDimensionPreviewMaterial.SetFloat("SaturatedCircleSize", saturatedCircleSize);
        screenDimensionPreviewMaterial.SetFloat("InnerColorCircleSize", colorCircleSize);

        if (saturatedCircleSize == currentSaturatedCircleSize && colorCircleSize == currentColorCircleSize)
        {
            return false;
        } else
        {
            return true;
        }
    }
}
