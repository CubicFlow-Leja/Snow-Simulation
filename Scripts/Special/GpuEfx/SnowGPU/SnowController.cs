using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowController : MonoBehaviour
{

  
   // public float TerrainSize;

    
    public ComputeShader Compute;
    private int PhysicsSimId;

    public bool reinitialize;

    [Range(0, 0.5f)]
    public float Radius;


    [Range(0, 0.2f)]
    public float SnowDeform;
    [Range(0, 0.2f)]
    public float SnowFix;

    
    public Material TerrainMat;

    //Texture
    private RenderTexture RenderTex;
    public RenderTexture RenderTexTemp;
    private RenderTexture DeformationTex;

    public Texture2D InitialInput;


    //private Vector4[] Positions;
    //public List<GameObject> Objects = new List<GameObject>();
    //public LayerMask CanDeform;


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
        if (reinitialize)//reinit za profiling
        {
            reinitialize = false;
            Init();
        }

       
        //Positions = CreatePosList();
        //Compute.SetVectorArray("Positions", Positions);
        //Compute.SetInt("PositionsCount", Positions.Length);
        Compute.Dispatch(PhysicsSimId, RenderTex.width / 8, RenderTex.height / 8, 1);

    }

    //private Vector4[] CreatePosList()
    //{
    //    // Count =(int) Mathf.Clamp(Objects.Count,0,50);
    //    A = 0;
    //    List<Vector4> Temp = new List<Vector4>();

    //    for (int i = 0; i < Objects.Count; i++)
    //    {
    //        if (Objects[i].activeSelf)
    //        {
    //            Vector4 TempVec = new Vector4(Objects[i].transform.position.x - this.transform.position.x, 0.0f, Objects[i].transform.position.z - this.transform.position.z, Objects[i].transform.localScale.x);//.transform.position-this.transform.position
    //            //Temp[A] = TempVec;
    //            Temp.Add(TempVec);
    //            A++;
    //        }
    //        if (A == 50)
    //            break;
    //    }

    //    return Temp.ToArray();
    //}


    public void Init()
    {


        DepthTexCamera.depthTextureMode = DepthTextureMode.Depth;


        //init compute
        PhysicsSimId = Compute.FindKernel("SnowCalculus");
        int overwrite = Compute.FindKernel("FlashInput");

        //init RNDTX
        RenderTex = new RenderTexture(InitialInput.width, InitialInput.height, 24);
        RenderTex.format = RenderTextureFormat.RFloat;
        RenderTex.enableRandomWrite = true;
        RenderTex.Create();


        //RenderTexTemp = new RenderTexture(InitialInput.width, InitialInput.height, 24);
        //RenderTexTemp.format = RenderTextureFormat.RFloat;
        RenderTexTemp.enableRandomWrite = true;
        //RenderTexTemp.Create();


        DeformationTex = new RenderTexture(InitialInput.width, InitialInput.height, 24);
        DeformationTex.format = RenderTextureFormat.RFloat;
        DeformationTex.enableRandomWrite = true;
        DeformationTex.Create();

        
        //setanje u compute shaderu
       // Compute.SetFloat("TerrainSize", TerrainSize);
        Compute.SetFloat("TexWidth", RenderTex.width);
        Compute.SetFloat("TexHeight", RenderTex.height);



        Compute.SetTexture(overwrite, "Input", InitialInput);
        Compute.SetTexture(overwrite, "Result", RenderTex);
        Compute.SetTexture(overwrite, "DeformationTemp", DeformationTex);
        Compute.SetTexture(overwrite, "ResultTemp", RenderTexTemp);


        //proccan compute shader sa 8*8*1 thread 
        Compute.Dispatch(overwrite, RenderTex.width / 8, RenderTex.height / 8, 1);


        //result
        Compute.SetTexture(PhysicsSimId, "Result", RenderTex);
        Compute.SetTexture(PhysicsSimId, "DeformationTemp", DeformationTex);
        Compute.SetTexture(PhysicsSimId, "ResultTemp", RenderTexTemp);
        Compute.SetFloat("SnowFix", SnowFix);
        Compute.SetFloat("SnowDeform", SnowDeform);
        Compute.SetFloat("Radius", Radius);
        

        TerrainMat.SetTexture("_Displacement", RenderTex);
    }

    //public void OnTriggerEnter(Collider other)
    //{
    //    if (CanDeform == (CanDeform | (1 << other.gameObject.layer)))
    //    {
    //        if (!Objects.Contains(other.gameObject))
    //        {
    //            Objects.Add(other.gameObject);
    //        }
    //    }
    //}

    //public void OnTriggerExit(Collider other)
    //{
    //    if (CanDeform == (CanDeform | (1 << other.gameObject.layer)))
    //    {
    //        if (Objects.Contains(other.gameObject))
    //        {
    //            Objects.Remove(other.gameObject);
    //        }
    //    }
    //}
}

