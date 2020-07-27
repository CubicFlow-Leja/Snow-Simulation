using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class PlayerActionHolder 
{
    public virtual void RClick(PlayerClass p)
    {
        p._Parent.StopPlayerInputs();

        p.StaminaDrained = true;
        p._Parent._TargetSpeed = Mathf.Lerp(2.0f, 5.0f, p._Stamina / p._MaxStamina);
       // p._Parent._TargetSpeed = 5.0f;
        _WaitForKeyUp(p, KeyCode.Mouse1, RClickPressed, RClickReleased);
    }
    public virtual void RClickPressed(PlayerClass p)
    {
        p._Parent._TargetSpeed = Mathf.Lerp(2.0f, 5.0f, p._Stamina / p._MaxStamina);
    }

    public virtual void RClickReleased(PlayerClass p)
    {
        p.StaminaDrained = false;
        p._Parent.ResetPlayerInputs();
        p._Parent._TargetSpeed = 1.0f; 

    }
    
    public virtual void _WaitForKeyUp(PlayerClass p,KeyCode _Code, Action<PlayerClass> FunctionToCall, Action<PlayerClass> FunctionToCall2)
    {
        p._Parent.StartCor(HoldingKey(p, FunctionToCall, FunctionToCall2, _Code));
    }


    public virtual IEnumerator HoldingKey(PlayerClass p,Action<PlayerClass> _FunctionToCall, Action<PlayerClass> _FunctionToCall2, KeyCode _Code)
    {
      
        while (Input.GetKey(_Code))
        {
            _FunctionToCall(p);
            yield return new WaitForSeconds(Time.deltaTime);
        }
      
        KeyUp(p, _FunctionToCall2);
        yield return null;
    }



    public virtual void KeyUp(PlayerClass p,Action<PlayerClass> Fun)
    {
        Fun(p);
    }
    
}

