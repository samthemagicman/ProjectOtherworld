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
    public Material screenDimensionPreviewMaterial;


    float lerpTime = 0.2f;
    float currentLerpTime;

    float lerpSpeed = 1f;

    public Volume dimensionPreviewVolume;
    Vignette vignette;
    ChromaticAberration chromaticAbberation;
    ColorAdjustments colorAdjustment;
    LensDistortion lensDistortion;

    void Start()
    {
        DimensionFilter[] objects = Resources.FindObjectsOfTypeAll(typeof(DimensionFilter)) as DimensionFilter[];

        dimensionPreviewVolume.profile.TryGet(out vignette);
        dimensionPreviewVolume.profile.TryGet(out chromaticAbberation);
        dimensionPreviewVolume.profile.TryGet(out colorAdjustment);
        dimensionPreviewVolume.profile.TryGet(out lensDistortion);
    }

    void UpdateAllMaterials()
    {

        DimensionFilter[] dimensions = Resources.FindObjectsOfTypeAll(typeof(DimensionFilter)) as DimensionFilter[];
        foreach (DimensionFilter dim in dimensions)
        {
            if (dim.dimension != currentDimension)
            {
                dim.GetComponent<Renderer>().material = dimensionPreviewMaterial;
            }
            else
            {
                dim.GetComponent<Renderer>().material = dim.startingMaterial;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentLerpTime += lerpSpeed * Time.deltaTime / Time.timeScale;
        currentLerpTime = Mathf.Clamp(currentLerpTime, 0, lerpTime);
        float perc = currentLerpTime / lerpTime;

        //dimensionPreviewMaterial.SetFloat("_Fade", Mathf.Lerp(0f, 0.8f, perc));
        dimensionPreviewMaterial.SetFloat("WorldCircleSize", Mathf.Lerp(-0.07f, 0.57f, perc));
        screenDimensionPreviewMaterial.SetFloat("CircleSize", Mathf.Lerp(-0.07f, 0.6f, perc));
        vignette.intensity.value  = Mathf.Lerp(0f, 0.4f, perc);
        chromaticAbberation.intensity.value = Mathf.Lerp(0f, 0.5f, perc);
        colorAdjustment.saturation.value = Mathf.Lerp(0f, -30f, perc);
        lensDistortion.intensity.value = Mathf.Lerp(0f, -0.3f, perc);

        bool changed = false;



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
    }
}
