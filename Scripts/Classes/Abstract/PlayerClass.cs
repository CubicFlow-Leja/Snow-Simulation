using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class PlayerClass 
{
   
    protected CharacterController PlayerController;
    protected AbstractController ParentScript = null;

    //ovo je za wallcollision seme
    private protected Vector3 WallVector;
    private protected float DotProd=0.0f;
    
    //ovo je bas movevec
    private protected Vector3 MoveVec = Vector3.zero;

    private protected float Speed = 1.875f;
    private protected float RotaSpeed =12.0f;


    private protected float TargetSpeed = 1.0f;
    private protected float CurrentSpeed = 0.0f;
    private protected float SpeedGravity =5.0f;

   
   

    public float _Speed
    {
        set { Speed = value; }
        get { return Speed; }
    }
 
    public Vector3 _MoveVec
    {
        set { MoveVec = value; }
        get { return MoveVec; }
    }

    
    public CharacterController _playercontroller
    {
        set
        {
            if (value != null)
                PlayerController = value;

        }
        get { return PlayerController; }
    }
    public AbstractController _Parent
    {
        set
        {
            if (value != null)
                ParentScript = value;

        }
        get { return ParentScript; }
    }
    
    public abstract void Movement(Vector3 MoveDir,float _TargetSpeed);

    
}
