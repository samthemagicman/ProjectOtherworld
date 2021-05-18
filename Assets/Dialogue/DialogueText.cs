using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DialogueText : MonoBehaviour
{
    TMP_Text tmpText;

    string currentText = "";

    List<string> textTagStack = new List<string>();
    
    void Awake()
    {
        DOTween.SetTweensCapacity(1000, 100);
        tmpText = GetComponent<TMP_Text>();
    }

    public void SetText(string text, bool typewrite=true)
    {
        // tmpText.SetText(text);
        tmpText.text = text;
        tmpText.ForceMeshUpdate();
        currentText = text;
        if (typewrite)
        {
            StartCoroutine("Typewrite");
        }
    }

    private void SetTextToInvisible()
    {
        TMP_TextInfo textInfo = tmpText.textInfo;
        for (int i = 0; i < textInfo.characterCount; ++i)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible)
            {
                continue;
            }

            TMP_MeshInfo meshInfo = textInfo.meshInfo[charInfo.materialReferenceIndex];

            int vertexIndex = textInfo.characterInfo[i].vertexIndex;
            var oldVertexColor = meshInfo.colors32;
            oldVertexColor[charInfo.vertexIndex + 0].a = 0;
            oldVertexColor[charInfo.vertexIndex + 1].a = 0;
            oldVertexColor[charInfo.vertexIndex + 2].a = 0;
            oldVertexColor[charInfo.vertexIndex + 3].a = 0;
        }

        tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    IEnumerator Typewrite()
    {
        //DOTween.To(() => transform.position, x => transform.position = x, new Vector3(2, 2, 2), 1);

        SetTextToInvisible();

        TMP_TextInfo textInfo = tmpText.textInfo;
        for (int i = 0; i < textInfo.characterCount; ++i)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible)
            {
                continue;
            }
            
            TMP_MeshInfo meshInfo = textInfo.meshInfo[charInfo.materialReferenceIndex];
            int vertexIndex = charInfo.vertexIndex;

            var oldVertexColor = meshInfo.colors32;

            var verts = meshInfo.vertices;

            float val = 0;

            for (int j = 0; j < 4; j++)
            {
                int vInd = vertexIndex + j;
                var originalVerts = verts[vInd];
                var offset = originalVerts + new Vector3(0, -10, 0);
                float trans = 0;


                DOTween.To(() => originalVerts,
                    x =>
                    {
                        verts[vInd] = x;

                        //var meshInfo = textInfo.meshInfo[0];
                        //meshInfo.colors32[curIndex] = Color.red;
                        textInfo.meshInfo[0].mesh.vertices = meshInfo.vertices;
                        tmpText.UpdateGeometry(textInfo.meshInfo[0].mesh, 0);

                        trans = x.y / originalVerts.y;
                        oldVertexColor[vInd].a = (byte)(trans * 255);
                        tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                    },
                    offset,
                    0.2f).From(false);


                /*DOTween.Shake(() => originalVerts,
                    x =>
                    {
                        verts[vInd] = x;

                        //var meshInfo = textInfo.meshInfo[0];
                        //meshInfo.colors32[curIndex] = Color.red;
                        textInfo.meshInfo[0].mesh.vertices = meshInfo.vertices;
                        tmpText.UpdateGeometry(textInfo.meshInfo[0].mesh, 0);
                    },
                    10f,
                    1f, 5, 30f, true, false).From(false);*/

                //verts[vertexIndex + j] =
                meshInfo.mesh.vertices = meshInfo.vertices;
            }
            tmpText.UpdateGeometry(meshInfo.mesh, 0);
            
            /*
            DOTween.To(() => val,
                    x => {
                        val = x;

                        float val2 = val / 255f;

                        oldVertexColor[charInfo.vertexIndex + 0].a = (byte) val;
                        oldVertexColor[charInfo.vertexIndex + 1].a = (byte) val;
                        oldVertexColor[charInfo.vertexIndex + 2].a = (byte) val;
                        oldVertexColor[charInfo.vertexIndex + 3].a = (byte) val;
                        tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

                        //meshInfo.vertices[vertexIndex];

                    },
                    255,
                    0.2f);*/

            yield return new WaitForSeconds(0.02f);
        }

        yield return null;
    }
}
