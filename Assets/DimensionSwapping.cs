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
        currentLerpTime += lerpSpeed * Time.deltaTime;
        currentLerpTime = Mathf.Clamp(currentLerpTime, 0, lerpTime);
        float perc = currentLerpTime / lerpTime;

        dimensionPreviewMaterial.SetFloat("_Fade", Mathf.Lerp(0f, 0.8f, perc));
        vignette.intensity.value  = Mathf.Lerp(0f, 0.4f, perc);
        chromaticAbberation.intensity.value = Mathf.Lerp(0f, 0.5f, perc);
        colorAdjustment.saturation.value = Mathf.Lerp(0f, -30f, perc);
        lensDistortion.intensity.value = Mathf.Lerp(0f, -0.3f, perc);

        bool changed = false;
        if (Input.GetKey(KeyCode.E))
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
