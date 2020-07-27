using System.Collections;
using UnityEngine;
using System;

public class PlayerController : AbstractController
{
    public GameObject _Camera;
    
    private Action Inputs = delegate { };
    
    public Vector3 DefaultStartPosition;
    
    
    void Awake()
    {

    }
    private void Start()
    {
        Init();
    }

  
    override public void Init()
    {


      
        Player = new PlayerMain();
        Holder = new MotherActionholder();
        SetupControlls();

      
  
        this.transform.position = DefaultStartPosition;
     
        GameObject _Char = Instantiate(Char, this.transform.position, Quaternion.identity, null);
        _Char.GetComponent<AbstractPawn>().SetupPawn(this);

        Player._playercontroller = _Char.GetComponent<CharacterController>();
        Player._playercontroller.gameObject.tag = "Player";
        Player._Parent = this;

       
        Inputs = HandleInputs;
        
       
         _Camera.GetComponent<CameraController>().SetTarget(Player._playercontroller.gameObject, 2.0f);
        
    }




    void Update()
    {
        Inputs();
    }

    void HandleInputs()
    {

        if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKey(KeyCode.Mouse1))
            RightClickPress(Player);

      

        Player.Movement(new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")), TargetSpeed);


    }
    
    void NoInputsPlayer()
    {
        Player.Movement(new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")), TargetSpeed);
    }

    
   
    override public void StopPlayerInputs()
    {
        Inputs = NoInputsPlayer;
    }
    override public void ResetPlayerInputs()
    {
        Inputs = HandleInputs;
    }
    
}
