using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn;
using Yarn.Unity;
public class DialogueUIManager : MonoBehaviour
{
    public Text characterName;

    public void Awake()
    {
        FindObjectOfType<DialogueRunner>().AddCommandHandler("name", SetCharacterName);
    }

    public void SetCharacterName(string[] parameters)
    {
        characterName.text = parameters[0];
    }
}
