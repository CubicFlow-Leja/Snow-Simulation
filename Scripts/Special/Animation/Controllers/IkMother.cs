using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IkMother : IKClass
{
    public GameObject HandDefaultPosition;
    public GameObject HandRaisedPosition;
    public float IKGravityLeftHand = 1.5f;
    private bool Raised = true;

    public bool _Raised
    {
        set { Raised = value; }
        get { return Raised; }
    }

    override public bool ReturnGrabType()
    {
        return false;
    }


    override public void TriggerIKLeft()
    {
       
        if (Raised)
            IKTargetLeftHandTemp.transform.position += (HandRaisedPosition.transform.position - IKTargetLeftHandTemp.transform.position) * IKGravityLeftHand * Time.deltaTime;
        else      
            IKTargetLeftHandTemp.transform.position += (HandDefaultPosition.transform.position - IKTargetLeftHandTemp.transform.position) * IKGravityLeftHand * Time.deltaTime;


        Ani.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        Ani.SetIKPosition(AvatarIKGoal.LeftHand, IKTargetLeftHandTemp.transform.position);

    }


}

