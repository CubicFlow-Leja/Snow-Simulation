using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class AbstractController : MonoBehaviour
{
   
    public PlayerClass Player;
    public GameObject Char;
    public PlayerActionHolder Holder;

    public abstract void Init();

    
    public virtual void StartCor(IEnumerator Cor)
    {
        StartCoroutine(Cor);
    }
   
    
   
    private protected Action<PlayerClass> RclickPress = delegate { };
   
    public Action<PlayerClass> _RclickPress { set { RclickPress = value; } }
   
    public virtual void RightClickPress(PlayerClass i) { RclickPress(i); }
  


    public virtual void SetupControlls()
    {
        this.RclickPress = this.Holder.RClick;
      
    }
    
    public abstract void StopPlayerInputs();
    public abstract void ResetPlayerInputs();


    protected private float TargetSpeed = 1.0f;

    public float _TargetSpeed
    {
        set { this.TargetSpeed = value; }
        get { return this.TargetSpeed; }
    }

    
}
