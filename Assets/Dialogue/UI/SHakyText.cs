using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SHakyText : MonoBehaviour
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

        for (int i = 0; i < textInfo.characterCount; ++i)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible)
            {
                continue;
            }

            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            Vector3 offset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            for (int j = 0; j < 4; ++j)
            {
                var orig = verts[charInfo.vertexIndex + j];
                verts[charInfo.vertexIndex + j] = orig + offset;

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
