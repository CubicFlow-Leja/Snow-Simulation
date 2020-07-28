using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain  : PlayerClass
{
    override public void Movement(Vector3 movedir,float _TargetSpeed)
    {
        if (movedir.magnitude != 0.0f)
        {
            WallVector = PlayerController.GetComponent<AbstractPawn>().ReturnWallInfluenceVector();
            DotProd = Vector3.Dot(movedir.normalized, WallVector);
            if (DotProd < 0.0f)
                DotProd = 0.0f;


            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1.0f || Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1.0f)
            {
                MoveVec = movedir.normalized;
                PlayerController.gameObject.transform.forward = Vector3.RotateTowards(PlayerController.gameObject.transform.forward, MoveVec, RotaSpeed * Time.deltaTime, 1.0f);
            }
        }



        TargetSpeed += (_TargetSpeed - TargetSpeed) * Time.deltaTime * SpeedGravity;
        CurrentSpeed += (Mathf.Clamp01(movedir.magnitude*(1.0f-DotProd)) * TargetSpeed - CurrentSpeed) * Time.deltaTime * SpeedGravity;



        PlayerController.GetComponent<AbstractPawn>().SendAnimationInfo( CurrentSpeed);
        PlayerController.GetComponent<AbstractPawn>().ReturnIkClass().SetMoveDir(MoveVec* CurrentSpeed / 5.0f);
        
    
        PlayerController.Move((_MoveVec * Speed * CurrentSpeed - Vector3.up * 55.0f) * Time.deltaTime);
        
           
    }


}
