using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class IKClass : MonoBehaviour
{
    public float IkGravityTorso = 50.0f;
    public float IkGravityHead = 200.0f;
    public float IkGravityHands = 5.0f;


    public float WeightGravity = 3.0f;

    public GameObject HeadTargetDefault;

    protected private float WeightLeft = 0.0f;
    protected private float WeightRight = 0.0f;

    protected private Animator Ani;
    
    //target
    protected private GameObject TargetLeftHand = null;
    protected private GameObject TargetRightHand = null;
    protected private GameObject TargetHead = null;

    public GameObject ImportantTargetLeftHand;
    public GameObject ImportantTargetRightHand;

    public GameObject LeftTarget
    {
        set { TargetLeftHand = value; }
        get { return TargetLeftHand; }
    }
    public GameObject RightTarget
    {
        set { TargetRightHand = value; }
        get { return TargetRightHand; }
    }
    public Vector3 ImportantLeftTarget
    {
        set { ImportantTargetLeftHand.transform.position = value; }
        get { return ImportantTargetLeftHand.transform.position; }
    }
    public Vector3 ImportantRightTarget
    {
        set { ImportantTargetRightHand.transform.position = value; }
        get { return ImportantTargetRightHand.transform.position; }
    }

    public GameObject HeadTarget
    {
        set { TargetHead = value; }
        get { return TargetHead; }
    }

    public abstract bool ReturnGrabType();


    //temp targeti radi smoothinga
    public GameObject IKTargetLeftHandTemp = null;
    public GameObject IKTargetRightHandTemp = null;
    public GameObject IKTargetHeadTemp = null;
    

    //za naginjanje roota
    public Transform Root;
    public Vector3 RootDefaultLoc;

    //za crouch
    public Transform HipTest;
    public Transform HipTestTarget;
    public Transform ILeft;
    public Transform IRight;

    private void Awake()
    {
        Ani = GetComponent<Animator>();
        RootDefaultLoc = Root.transform.localPosition;
        ImportantTargetLeftHand.transform.SetParent(null);
        ImportantTargetRightHand.transform.SetParent(null);
        ImportantTargetLeftHand.transform.position = Vector3.zero;
        ImportantTargetRightHand.transform.position = Vector3.zero;
    }

  
    private void OnAnimatorIK(int layerIndex)
    {
        if (Ani)
        {
            ILeft.localPosition = Ani.GetIKPosition(AvatarIKGoal.LeftFoot);
            IRight.localPosition = Ani.GetIKPosition(AvatarIKGoal.RightFoot);
            Ani.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
            Ani.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
         
           
            ILeft.localPosition += (HipTestTarget.position - HipTest.position);
            IRight.localPosition += (HipTestTarget.position - HipTest.position);
        
            Ani.SetIKPosition(AvatarIKGoal.LeftFoot, ILeft.localPosition);
            Ani.SetIKPosition(AvatarIKGoal.RightFoot, IRight.localPosition);
        
            
            TriggerIKHead();
            TriggerIKLeft();
            TriggerIKRight();
        }
    }


    public void SetMoveDir(Vector3 BendVector)
    {
        Root.localPosition +=( RootDefaultLoc + Root.InverseTransformVector(BendVector*0.5f)+Vector3.down*BendVector.magnitude*0.1f- Root.localPosition ) *Time.deltaTime* IkGravityTorso;
    }

    public void TriggerIKHead()
    {
        if (TargetHead == null)
            IKTargetHeadTemp.transform.rotation = Quaternion.RotateTowards(IKTargetHeadTemp.transform.rotation, HeadTargetDefault.transform.rotation, IkGravityHead * Time.deltaTime);
        else
            IKTargetHeadTemp.transform.rotation = Quaternion.RotateTowards(IKTargetHeadTemp.transform.rotation, TargetHead.transform.rotation, IkGravityHead * Time.deltaTime);
        
        Ani.SetBoneLocalRotation(HumanBodyBones.Head, IKTargetHeadTemp.transform.localRotation);
    }




    public virtual void TriggerIKLeft()
    {
        if (ImportantLeftTarget.magnitude != 0.0f)
            LeftTarget = ImportantTargetLeftHand;

        if (LeftTarget == null)
            WeightLeft += (0.0f - WeightLeft) * Time.deltaTime * WeightGravity;
        else
        {
            WeightLeft += (1.0f - WeightLeft) * Time.deltaTime * WeightGravity;
            IKTargetLeftHandTemp.transform.position += (LeftTarget.transform.position - IKTargetLeftHandTemp.transform.position) * IkGravityHands * Time.deltaTime;
        }


        Ani.SetIKPositionWeight(AvatarIKGoal.LeftHand, WeightLeft);
        Ani.SetIKPosition(AvatarIKGoal.LeftHand, IKTargetLeftHandTemp.transform.position);

    }
    public virtual void TriggerIKRight()
    {

        if (ImportantRightTarget.magnitude != 0.0f)
            RightTarget = ImportantTargetRightHand;

        if (RightTarget == null)
            WeightRight += (0.0f - WeightRight) * Time.deltaTime * WeightGravity;
        else
        {
            WeightRight += (1.0f - WeightRight) * Time.deltaTime * WeightGravity;
            IKTargetRightHandTemp.transform.position += (RightTarget.transform.position - IKTargetRightHandTemp.transform.position) * IkGravityHands * Time.deltaTime;
        }


        Ani.SetIKPositionWeight(AvatarIKGoal.RightHand, WeightRight);
        Ani.SetIKPosition(AvatarIKGoal.RightHand, IKTargetRightHandTemp.transform.position);

    }


}
