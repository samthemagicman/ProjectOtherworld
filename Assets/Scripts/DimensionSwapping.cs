using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DimensionSwapping : MonoBehaviour
{
    public static DimensionFilter.Dimension currentDimension;

    public GameObject player;
    public Camera dimension2Camera;

    public Material dimensionPreviewMaterial;
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

    void UpdateAllMaterials()
    {

        DimensionFilter[] dimensions = Resources.FindObjectsOfTypeAll(typeof(DimensionFilter)) as DimensionFilter[];
        foreach (DimensionFilter dim in dimensions)
        {
            if (previewingDimension)
            {

                if (dim.dimension != currentDimension)
                {
                    dim.GetComponent<Renderer>().material = dimensionPreviewMaterial;
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
                        dim.GetComponent<Renderer>().material = dimensionPreviewMaterial;
                    }
                    else
                    {
                        dim.GetComponent<Renderer>().material = firstDimensionPreviewMaterial;
                    }
                }
            }
        }
    }

    float v = 0.0f;
    float v2 = 0.0f;

    bool UpdateSaturationCircles()
    {
        float currentSaturatedCircleSize = screenDimensionPreviewMaterial.GetFloat("SaturatedCircleSize");
        float saturatedCircleTargetSize = -0.63f;

        float currentColorCircleSize = screenDimensionPreviewMaterial.GetFloat("InnerColorCircleSize");
        float colorCircleTargetSize = 0f;

        //screenDimensionPreviewMaterial.SetFloat("SaturatedCircleSize", Mathf.SmoothStep(-0.63f, 0.6f, perc));

        if (Input.GetButton("PreviewDimension"))
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

    // Update is called once per frame
    void Update()
    {
        previewingDimension = Input.GetButton("PreviewDimension");

        currentLerpTime += lerpSpeed * Time.deltaTime / Time.timeScale;
        currentLerpTime = Mathf.Clamp(currentLerpTime, 0, lerpTime);
        float perc = currentLerpTime / lerpTime;

        dimensionPreviewMaterial.SetFloat("WorldCircleSize", Mathf.Lerp(-0.07f, 0.57f, perc));
        firstDimensionPreviewMaterial.SetFloat("WorldCircleSize", Mathf.Lerp(-0.07f, 0.57f, perc));


        vignette.intensity.value  = Mathf.Lerp(0f, 0.4f, perc);
        chromaticAbberation.intensity.value = Mathf.Lerp(0f, 0.5f, perc);
        colorAdjustment.saturation.value = Mathf.Lerp(0f, -30f, perc);
        lensDistortion.intensity.value = Mathf.Lerp(0f, -0.3f, perc);

        bool changed = UpdateSaturationCircles();

        if (Input.GetButton("PreviewDimension"))
        {
            changed = true;
            lerpSpeed = Mathf.Abs(lerpSpeed);
        } else
        {
            lerpSpeed = -Mathf.Abs(lerpSpeed);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            changed = true;
            if (currentDimension == DimensionFilter.Dimension.One)
            {
                currentDimension = DimensionFilter.Dimension.Two;
            } else
            {
                currentDimension = DimensionFilter.Dimension.One;
            }
        }
        if (changed)
        {
            UpdateAllMaterials();
        }


        #region Slomo
        if (Input.GetButton("Slomo"))
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 0.03f, 0.1f);
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
        }
        else
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, 0.12f);
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
        }
        #endregion

    }
}
