using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WobblyText : MonoBehaviour
{
    //https://www.youtube.com/watch?v=U85gbZY6oo8
    TMP_Text text;

    void Start()
    {
        text = GetComponent<TMP_Text>();
        //text.textInfo.characterCount;
        //text.maxVisibleCharacters;
    }

    void Update()
    {
        text.ForceMeshUpdate();
        TMP_TextInfo textInfo = text.textInfo;
        for (int i = 4; i < textInfo.characterCount - 7; ++i)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible)
            {
                continue;
            }

            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            for (int j = 0; j < 4; ++j)
            {
                var orig = verts[charInfo.vertexIndex + j];
                verts[charInfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time * 5f + orig.x * 0.08f) * 6f, 0);
                 
            }
        }

        for (int i = 0; i < textInfo.meshInfo.Length; ++i)
        {
            var meshInfo = textInfo.meshInfo[i];

            meshInfo.mesh.vertices = meshInfo.vertices;
            text.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}
