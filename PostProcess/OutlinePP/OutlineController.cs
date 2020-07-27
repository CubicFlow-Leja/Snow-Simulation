using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class OutlineController : MonoBehaviour
{
    
    public Shader _outlineShader;
    public Shader _PixelShader;
    private Material OutlineMat;
    private Material PixelMat;
    public Color OutlineCol;
    public float _Scale;
    public float _Dist;

    public bool UsePixel;
    public int Rows;
    public int Columns;

    public Material _OutlineMat
    {
        get
        {
            if (!OutlineMat && _outlineShader)
            {
                OutlineMat = new Material(_outlineShader);
                OutlineMat.hideFlags = HideFlags.HideAndDontSave;
            }

            return OutlineMat;
        }
    }

    public Material _PixelMat
    {
        get
        {
            if (!PixelMat && _PixelShader)
            {
                PixelMat = new Material(_PixelShader);
                PixelMat.hideFlags = HideFlags.HideAndDontSave;
            }

            return PixelMat;
        }
    }


    public Camera _camera
    {
        get
        {
            if (!cam)
            {
                cam = GetComponent<Camera>();
                cam.depthTextureMode = DepthTextureMode.Depth;


            }
            return cam;
        }
    }
    private Camera cam;


    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!_OutlineMat)
        {
            Graphics.Blit(source, destination);
            return;
        }

      
        _OutlineMat.SetColor("_OutlineColor", OutlineCol);
        _OutlineMat.SetFloat("_Scale", _Scale);
        _OutlineMat.SetFloat("_Dist", _Dist);
        

        
       
        if (UsePixel)
        {
            RenderTexture TempTex = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
            _PixelMat.SetInt("_Columns", Columns);
            _PixelMat.SetInt("_Rows", Rows);
            Graphics.Blit(source, TempTex, _OutlineMat);//outline

            _PixelMat.SetTexture("_TempTex", TempTex);

            Graphics.Blit(source, destination, _PixelMat);//pixel
            TempTex.Release();
        }
        else
        {
            Graphics.Blit(source, destination, _OutlineMat);
        }
        



    }
}


// viewToWorld = _camera.cameraToWorldMatrix;
//_OutlineMat.SetMatrix("_viewToWorld", viewToWorld);
// _OutlineMat.SetFloat("_Dist2", _Dist2);
//  _OutlineMat.SetFloat("_AngleFactor", _AngleFactor);  
//  public float _AngleFactor;
// public float _Dist2;
// private Matrix4x4 viewToWorld;
