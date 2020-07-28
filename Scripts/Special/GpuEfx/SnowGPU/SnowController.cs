using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowController : MonoBehaviour
{

  
    
    public ComputeShader Compute;
    private int PhysicsSimId;

    public bool reinitialize;



    [Range(0, 0.2f)]
    public float SnowDeform;
    [Range(0, 0.2f)]
    public float SnowFix;

    
    public Material TerrainMat;

   
    private RenderTexture RenderTex;
    public RenderTexture RenderTexTemp;
    private RenderTexture DeformationTex;
    
    public Camera DepthTexCamera;
    private int A;

    public Texture GetSimulatedTex
    {
        get { return RenderTex; }
    }

    void Start()
    {
        Init();
    }


    void Update()
    {
        if (reinitialize)
        {
            reinitialize = false;
            Init();
        }

       
      
        Compute.Dispatch(PhysicsSimId, RenderTex.width / 8, RenderTex.height / 8, 1);

    }

    

    public void Init()
    {

        //init compute
        PhysicsSimId = Compute.FindKernel("SnowCalculus");
        int overwrite = Compute.FindKernel("FlashInput");

        //init RNDTX
        RenderTex = new RenderTexture(RenderTexTemp.width, RenderTexTemp.height, 24);
        RenderTex.format = RenderTextureFormat.RFloat;
        RenderTex.enableRandomWrite = true;
        RenderTex.Create();



        DeformationTex = new RenderTexture(RenderTexTemp.width, RenderTexTemp.height, 24);
        DeformationTex.format = RenderTextureFormat.RFloat;
        DeformationTex.enableRandomWrite = true;
        DeformationTex.Create(); 

        
        
        Compute.SetTexture(overwrite, "Result", RenderTex);
        Compute.SetTexture(overwrite, "DeformationTemp", DeformationTex);
      


        //proccan compute shader sa 8*8*1 thread 
        Compute.Dispatch(overwrite, RenderTex.width / 8, RenderTex.height / 8, 1);


        //result
        Compute.SetTexture(PhysicsSimId, "Result", RenderTex);
        Compute.SetTexture(PhysicsSimId, "DeformationTemp", DeformationTex);
        Compute.SetTexture(PhysicsSimId, "ResultTemp", RenderTexTemp);
        Compute.SetFloat("SnowFix", SnowFix);
        Compute.SetFloat("SnowDeform", SnowDeform);

        

        TerrainMat.SetTexture("_Displacement", RenderTex);
        
        DepthTexCamera.depthTextureMode = DepthTextureMode.Depth;
 
     
    }

    
}

