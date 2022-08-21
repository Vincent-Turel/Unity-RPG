using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public TMP_Text text;
    public Slider slider;

    void Update()
    {
        Material material = text.fontSharedMaterial;
        float value = slider.value;
        float minValue = 0.42f;
        if (value < 0.6 && value > minValue)
        {
            material.SetVector("_FaceTex_ST", new Vector4(4.5f,1f,-3.95f + (value-minValue)/0.0000018f*0.000046f,0f));
        } else if (value > 0.6)
        {
            material.SetVector("_FaceTex_ST", new Vector4(4.5f,1f,0.7f,0f));
        }
        else
        {
            material.SetVector("_FaceTex_ST", new Vector4(4.5f,1f,-4f,0f));
        }
    }
}
