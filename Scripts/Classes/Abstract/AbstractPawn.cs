using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class AbstractPawn : MonoBehaviour
{
    protected private AbstractController Controller;
 

    public Animator _AnimatorMain;
    public IKClass IKController;

    //IKRAY
    public Transform RayRoot;

    public float NormalOffset = 0.2f;
    public float RayOriginYOffset = 0.2f;
    public float Reach = 1.35f;
    public float MinReach = 0.75f;
    public float HandBehindCharFactor = 0f;
    public float HandBehindCharFactor2 = -0.75f;

    protected private RaycastHit hit;
    protected private Vector3 origin;
    protected private Vector3 dir;
    protected private Vector3 NewOrigin;
    protected private Vector3 TempTarget;
    protected private Vector3 TempDir;
    protected private Vector3 Target;
    protected private Vector3 NewDir;
    protected private float NewReach;


    //gizmo only
    protected private List<Vector3> Hitpoints = new List<Vector3>();
    protected private List<Vector3> HitpointsVertical = new List<Vector3>();
    protected private List<Vector3> HitPointsRadial = new List<Vector3>();

    protected private List<RayHitStruct> RayHitsToUseL = new List<RayHitStruct>();
    protected private List<RayHitStruct> RayHitsToUseR = new List<RayHitStruct>();
   

    public LayerMask mask;

    protected private RayHitStruct TargetStruct;

    protected private GameObject TargetL = null;
    protected private GameObject TargetR = null;
    public GameObject HeadTarget = null;


    public abstract void SetupPawn(AbstractController _Controller);

   


    public abstract void SendAnimationInfo(float param);

    public abstract void Update();

    
    protected private bool CheckPosViability(Vector3 OldPos, float SideToObey)
    {
        if ((OldPos - origin).magnitude > Reach)
            return false;

        if ((OldPos - origin).magnitude < MinReach)
            return false;

        if (Vector3.Dot((OldPos - origin).normalized, this.transform.forward) < HandBehindCharFactor2)
            return false;

        if (Vector3.Dot((OldPos - origin).normalized, SideToObey * this.transform.right) < HandBehindCharFactor)
            return false;

        return true;
    }

    protected private bool CheckPosViabilitySimple(Vector3 pos)
    {
        if ((pos - origin).magnitude > Reach)
            return false;
        if ((pos - origin).magnitude < MinReach)
            return false;
        return true;
    }

    protected private virtual void ClearAll()
    {
        Hitpoints.Clear();
        HitpointsVertical.Clear();
        HitPointsRadial.Clear();
        RayHitsToUseL.Clear();
        RayHitsToUseR.Clear();
    }

    protected private void CalculateHand(GameObject Target,List<RayHitStruct> HitsToUse,float SideToObey)
    {
        if (CheckPosViability(Target.transform.position, SideToObey))
            return;

        ShootHandRays(SideToObey, HitsToUse);
        CalculateClosest(Target, HitsToUse);
        if (SideToObey < 0)
            SendLeftHandData(Target);
        else
            SendRightHandData(Target);
    }
    
    protected private void ShootHandRays(float a, List<RayHitStruct> RayHitsToUse)
    {
        for (int i = 2; i < 7; i++)
            ShootPlanarRadial(i, a, RayHitsToUse);

    }

    protected private void ShootPlanarRadial(int i, float a, List<RayHitStruct> RayHitsToUse)
    {
        dir = a * this.transform.right * (0.1f * i) + this.transform.forward * (1.0f - 0.1f * i);
        dir = dir.normalized;


        if (Physics.Raycast(origin, dir, out hit, Reach, mask))
        {
            if (CheckPosViabilitySimple(hit.point))
            {
                RayHitStruct Hit;
                Hit.HitPoint = hit.point;
                Hit.HitNormal = hit.normal;
                Hit.HitObject = hit.collider.gameObject;

                //HitPointsToUse.Add(hit.point + hit.normal * NormalOffset);
                RayHitsToUse.Add(Hit);
            }
            else
            {
                //down
                ShootTangentRays(-1.0f, RayHitsToUse);
                //up
                ShootTangentRays(1.0f, RayHitsToUse);
            }

             Hitpoints.Add(hit.point);
        }
        else
        {
            Hitpoints.Add(origin + dir * Reach);

            //down
            ShootTangentRays(-1.0f, RayHitsToUse);
            //up
            ShootTangentRays(1.0f, RayHitsToUse);

        }
    }

    protected private void ShootTangentRays(float a, List<RayHitStruct> RayHitsToUse)
    {
        NewOrigin = origin + dir * Reach;
        TempTarget = NewOrigin + a * Vector3.up;
        TempDir = (TempTarget - origin).normalized;
        Target = origin + TempDir * Reach;

        NewDir = (Target - NewOrigin).normalized;
        NewReach = (Target - NewOrigin).magnitude;



        if (Physics.Raycast(NewOrigin, NewDir, out hit, NewReach, mask))
        {
            HitpointsVertical.Add(hit.point);

            RayHitStruct Hit;
            Hit.HitPoint = hit.point;
            Hit.HitNormal = hit.normal;
            Hit.HitObject = hit.collider.gameObject;

            RayHitsToUse.Add(Hit);
        }
        else
        {
            HitpointsVertical.Add(Target);
            ShootRadialRays(Target, RayHitsToUse);
        }


    }
    protected private void ShootRadialRays(Vector3 point, List<RayHitStruct> RayHitsToUse)
    {

        if (Physics.Raycast(origin, point - origin, out hit, Reach, mask))
        {
            HitPointsRadial.Add(hit.point);

            if (CheckPosViabilitySimple(hit.point))
            {
                RayHitStruct Hit;
                Hit.HitPoint = hit.point;
                Hit.HitNormal = hit.normal;
                Hit.HitObject = hit.collider.gameObject;
                RayHitsToUse.Add(Hit);
            }
       
        }
        else
            HitPointsRadial.Add(point);
    }

    
    protected private void CalculateClosest(GameObject Target, List<RayHitStruct> RayHitsToUse)
    {
        Target.transform.SetParent(null);
        TargetStruct.HitPoint = Vector3.zero;
        TargetStruct.HitNormal = Vector3.zero;
        TargetStruct.HitObject = null;

        float Distance = Reach * 2.0f;
        foreach (RayHitStruct HitStruct in RayHitsToUse)
        {
            float temp = (HitStruct.HitPoint - RayRoot.transform.position).magnitude;
            if (temp < Distance)
            {
                TargetStruct.HitPoint = HitStruct.HitPoint;
                TargetStruct.HitNormal = HitStruct.HitNormal;
                TargetStruct.HitObject = HitStruct.HitObject;
                Distance = temp;
            }
        }
        
        Target.transform.position = TargetStruct.HitPoint + TargetStruct.HitNormal * NormalOffset;

        if (TargetStruct.HitObject != null)
            Target.transform.SetParent(TargetStruct.HitObject.transform);
        else
            Target.transform.SetParent(null);
    }


    protected private void SendLeftHandData(GameObject Target)
    {
        if (Target.transform.position.magnitude != 0.0f)
            IKController.LeftTarget = Target;
        else
            IKController.LeftTarget = null;
    }

    protected private void SendRightHandData(GameObject Target)
    {
        if (Target.transform.position.magnitude != 0.0f)
            IKController.RightTarget = Target;
        else
            IKController.RightTarget = null;
    }


    public AbstractController ReturnParent() { return this.Controller; }
    public IKClass ReturnIkClass() { return this.IKController; }



    protected private Collider[] WallsNearby;
    protected private Vector3 WallCalculusOrigin = Vector3.zero;
    protected private Vector3 WallVector = Vector3.zero;
    protected private  Vector3 TempVectorWall = Vector3.zero;

    public float CapsuleRadius = 0.6f;
    public float CapsuleHeight = 1.6f;


    public virtual void CalculateWallInfluence()
    {
        WallVector = Vector3.zero;
        WallCalculusOrigin = this.transform.position;
        WallsNearby = Physics.OverlapCapsule(WallCalculusOrigin + CapsuleHeight * Vector3.down, WallCalculusOrigin + CapsuleHeight * Vector3.up, CapsuleRadius, mask);

        foreach (Collider col in WallsNearby)
        {
            TempVectorWall = col.ClosestPoint(WallCalculusOrigin);
            TempVectorWall.y = WallCalculusOrigin.y;

            if (Vector3.Dot((TempVectorWall - WallCalculusOrigin).normalized, this.transform.forward) > 0.0f)
                WallVector += (TempVectorWall - WallCalculusOrigin).normalized;
        }

    }

    public Vector3 ReturnWallInfluenceVector()
    {
        if (WallVector.magnitude == 0.0f)
            return Vector3.zero;
        return WallVector.normalized;
    }


    protected private List<GameObject> LookAtObjects = new List<GameObject>();
    protected private Vector3 TempLookatVec = Vector3.zero;
    public float MinLookAtDotProduct = 0.01f;
    protected private float TempDot = 0.0f;
    protected private float TempDot2 = 0.0f;
    public void AffectHeadLook(GameObject LookAt, bool Add)
    {
        if (Add)
        {
            if (!LookAtObjects.Contains(LookAt))
                LookAtObjects.Add(LookAt);
        }
        else
        {
            if (LookAtObjects.Contains(LookAt))
                LookAtObjects.Remove(LookAt);
        }
    }

    protected private void CalculatedHeadLookAt()
    {
        TempLookatVec = Vector3.zero;
        TempDot = -1.0f;
        foreach (GameObject LookAtObj in LookAtObjects)
        {
            TempLookatVec = LookAtObj.transform.position - RayRoot.transform.position;
            TempLookatVec.y = 0.0f;
            TempDot2 = Vector3.Dot(TempLookatVec.normalized, this.transform.forward);
            if (TempDot2 >= MinLookAtDotProduct)
                if (TempDot2 > TempDot)
                {
                    TempDot = TempDot2;
                    HeadTarget.transform.forward = TempLookatVec.normalized;
                }
        }


        if (TempDot >= MinLookAtDotProduct)
            IKController.HeadTarget = HeadTarget;
        else
            IKController.HeadTarget = null;
    }


}


public struct RayHitStruct
{
    public Vector3 HitPoint;
    public Vector3 HitNormal;
    public GameObject HitObject;
}



