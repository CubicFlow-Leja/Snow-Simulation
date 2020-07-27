using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderVariableController : MonoBehaviour
{
    private float _Time=0.0f;
    public float Intesity=5.0f;
    public Color EffectColor;
    public Color EffectColorSpecial;
    void Start()
    {
        Shader.SetGlobalColor("_SpecialCol", EffectColor);
        Shader.SetGlobalColor("_SpecialCol2", EffectColorSpecial);
        Shader.SetGlobalFloat("_Intensity", Intesity);
        Shader.SetGlobalFloat("_TimeSinceStartUp", _Time);

    }

    void Update()
    {
        Shader.SetGlobalColor("_SpecialCol", EffectColor);
        Shader.SetGlobalColor("_SpecialCol2", EffectColorSpecial);
        Shader.SetGlobalFloat("_Intensity", Intesity);
        //Shader.SetGlobalFloat("_TimeSinceStartUp", _Time);

        //_Time = Time.realtimeSinceStartup;
        //Shader.SetGlobalFloat("_TimeSinceStartUp", _Time);
    }

}
