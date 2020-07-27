using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherActionholder : PlayerActionHolder
{
    //private float LanternRadiusScale = 0.2f;
    private float LanternRadiusScaleRun = 0.5f;

    //override public void LClick(PlayerClass p)
    //{
    //    p._Parent.StopPlayerInputs();

    //    p._playercontroller.GetComponent<AbstractPawn>().ReturnIkClass().gameObject.GetComponent<IkMother>()._Raised = false;

    //    LanternScript.Lantern.SetTargetRadiusScale(LanternRadiusScale);

    //    _WaitForKeyUp(p, KeyCode.Mouse0, LClickPressed, LClickReleased);
    //}

    //public void LClickPressed(PlayerClass p)
    //{
    //    nista zasad
    //}

    //public void LClickReleased(PlayerClass p)
    //{
    //    p._Parent.ResetPlayerInputs();

    //    p._playercontroller.GetComponent<AbstractPawn>().ReturnIkClass().gameObject.GetComponent<IkMother>()._Raised = true;

    //    LanternScript.Lantern.ResetTargetRadiusScale();

    //}



    override public void RClickPressed(PlayerClass p)
    {
        base.RClickPressed(p);

        //if (p._playercontroller.GetComponent<AbstractPawn>().ReturnIkClass().gameObject.GetComponent<IkMother>() != null)
            p._playercontroller.GetComponent<AbstractPawn>().ReturnIkClass().gameObject.GetComponent<IkMother>()._Raised = false;
        //else
        //    p._Parent.LinkedController.Player._playercontroller.GetComponent<AbstractPawn>().ReturnIkClass().gameObject.GetComponent<IkMother>()._Raised = false;
        LanternScript.Lantern.SetTargetRadiusScale(LanternRadiusScaleRun);
        
    }

    public override void RClickReleased(PlayerClass p)
    {
        base.RClickReleased(p);


        //if(p._playercontroller.GetComponent<AbstractPawn>().ReturnIkClass().gameObject.GetComponent<IkMother>()!=null)
            p._playercontroller.GetComponent<AbstractPawn>().ReturnIkClass().gameObject.GetComponent<IkMother>()._Raised = true;
        //else
        //    p._Parent.LinkedController.Player._playercontroller.GetComponent<AbstractPawn>().ReturnIkClass().gameObject.GetComponent<IkMother>()._Raised = true;
        LanternScript.Lantern.ResetTargetRadiusScale();

    }




}
