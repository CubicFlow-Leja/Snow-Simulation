using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IkHeadTrigger : MonoBehaviour
{
    public GameObject PointToLookAt;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            if (other.GetComponent<AbstractPawn>() != null)
                SendIkDataToPawn(true, other.GetComponent<AbstractPawn>());

    }
    
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            if (other.GetComponent<AbstractPawn>() != null)
                SendIkDataToPawn(false, other.GetComponent<AbstractPawn>());
    }

    public void SendIkDataToPawn(bool Add,AbstractPawn Pawn)
    {
        Pawn.AffectHeadLook(PointToLookAt, Add);
    }
}
