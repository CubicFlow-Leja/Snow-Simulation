using UnityEngine;
using System;

public class PawnMother : AbstractPawn
{
    override public void SetupPawn(AbstractController _Controller)
    {
        TargetL = new GameObject();
        TargetL.name = "LeftTarget";

        TargetR = new GameObject();
        TargetR.name = "RightTarget";
        
        Controller = _Controller;
    }



    override protected private void ClearAll()
    {
        Hitpoints.Clear();
        HitpointsVertical.Clear();
        HitPointsRadial.Clear();
        RayHitsToUseR.Clear();
    }

    override public void Update()
    {
        if (Controller == null)
            return;

        ClearAll();

        origin = RayRoot.position + Vector3.up * RayOriginYOffset;

        CalculateHand(TargetR, RayHitsToUseR, 1.0f);
        CalculateWallInfluence();
        CalculatedHeadLookAt();
    }



    override public void SendAnimationInfo(float param)
    {
        this._AnimatorMain.SetFloat("Speed", param);
    }




    ///// <summary>
    ///// //GIZMO
    ///// </summary>
    //public void OnDrawGizmos()
    //{
    //    if (Controller == null)
    //        return;

    //    foreach (Vector3 vec in Hitpoints)
    //    {
    //        Gizmos.color = Color.blue;

    //        Vector3 origin = RayRoot.position + Vector3.up * RayOriginYOffset;
    //        Gizmos.DrawLine(origin, vec);

    //        Gizmos.color = Color.red;
    //        Gizmos.DrawSphere(vec, 0.05f);
    //    }

    //    foreach (Vector3 vec in HitpointsVertical)
    //    {
    //        Gizmos.color = Color.green;



    //        Vector3 startpos = RayRoot.position + Vector3.up * RayOriginYOffset;
    //        Vector3 dir = vec - startpos;
    //        dir.y = 0.0f;

    //        Vector3 origin = startpos + dir.normalized * Reach;


    //        Gizmos.DrawLine(origin, vec);

    //        Gizmos.color = Color.yellow;
    //        Gizmos.DrawSphere(vec, 0.05f);
    //    }

    //    foreach (Vector3 vec in HitPointsRadial)
    //    {
    //        Gizmos.color = Color.cyan;

    //        Vector3 origin = RayRoot.position + Vector3.up * RayOriginYOffset;


    //        Gizmos.DrawLine(origin, vec);

    //        Gizmos.color = Color.magenta;
    //        Gizmos.DrawSphere(vec, 0.05f);
    //    }

    //    Gizmos.color = Color.white;
    //    Gizmos.DrawSphere(TargetL.transform.position, 0.1f);
    //    Gizmos.DrawSphere(TargetR.transform.position, 0.1f);

    //    //Gizmos.color = Color.red;
    //    //Gizmos.DrawWireSphere(WallCalculusOrigin + CapsuleHeight * Vector3.up, CapsuleRadius);
    //    //Gizmos.DrawWireSphere(WallCalculusOrigin + CapsuleHeight * Vector3.down, CapsuleRadius);

    //    //Gizmos.color = Color.green;
    //    //Gizmos.DrawLine(WallCalculusOrigin, WallCalculusOrigin + WallVector.normalized);
    //}
}
