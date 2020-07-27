using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject _Target;
    private Vector3 MoveVector;
    private float Speed;
    public float ZFix;
   // public GameObject Marker;
    public float Height;

    //seta kojeg chara prati i speed
    public void SetTarget(GameObject obj,float speed)
    {
        Speed = speed;
        _Target = obj;
    }
    void Start()
    {
        
        MoveVector = Vector3.zero;
    }

   
    void Update()
    {
        if (_Target != null)
        {
           // Marker.transform.position = _Target.transform.position + Vector3.up *2.5f;//pozicija markera
            MoveVector.y = Height + _Target.transform.position.y - this.transform.position.y;//y pos
            MoveVector.x = _Target.transform.position.x - this.transform.position.x;//x pos
            MoveVector.z= _Target.transform.position.z - this.transform.position.z+ ZFix;//z pos kamere + Zfix jer kamera gleda pod kuten pa da char bude centriran
            //moga bi dotat da kontrolira y os cisto eto da imas zoom 
            this.transform.Translate(MoveVector * Time.deltaTime*Speed);
        }
    }
}
