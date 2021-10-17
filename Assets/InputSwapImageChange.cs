using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputSwapImageChange : MonoBehaviour
{
    public Sprite controllerImage;
    public Sprite defaultImage;
    Image imageComponent;

    void Start()
    {
        imageComponent = GetComponent<Image>();
        InputHandler.singleton.inputChanged.AddListener(updateImage);
    }

    private void Awake()
    {
        imageComponent = GetComponent<Image>();
        updateImage();
    }

    void updateImage()
    {
        if (InputHandler.singleton.isController)
        {
            imageComponent.sprite = controllerImage;
        }
        else imageComponent.sprite = defaultImage;
    }
}
