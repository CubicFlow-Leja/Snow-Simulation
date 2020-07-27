using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternScript : MonoBehaviour
{
    public static LanternScript Lantern = null;
    public float DefaultLanternRadius = 20.0f;
    public float RadiusScale = 1.0f;
    protected private float Scale = 1.0f;
    public float RadiusGravity = 1.5f;

    private void Awake()
    {
        if (Lantern == null)
            Lantern = this;
        else
            Destroy(this.gameObject);
    }

    void Update()
    {
        Scale += (RadiusScale - Scale) * Time.deltaTime * RadiusGravity;
       // Debug.Log(Scale);
        Shader.SetGlobalVector("_Pos", this.transform.position);
        Shader.SetGlobalFloat("_Radius", DefaultLanternRadius* Scale);

        //triba editat boju lanterne ovisno o radiusu
    }

    public void SetTargetRadiusScale(float _Scale)
    {
        RadiusScale = _Scale;
    }

    public void ResetTargetRadiusScale()
    {
        RadiusScale = 1.0f;
    }

    public float GetRadiusScale()
    {
        return Scale;
    }
}
