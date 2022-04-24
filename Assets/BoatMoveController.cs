using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMoveController : MonoBehaviour
{
    public Transform VRPlayer;
    public Transform playerFloorPos;
    public Transform playerSeatPos;
    private Rigidbody boatRigid;

    [SerializeField] bool isSeat = false;

    [SerializeField] private bool isWater;

    [SerializeField] private List<Transform> floaters = new List<Transform>();
    public Transform floaterParent;

    public float underWaterDrag = 3f;
    public float underWaterAngularDrag = 1f;

    public float airDrag = 0f;
    public float angularDrag = 0.05f;

    public float floatingPower = 15f;

    public float waterHeight = 0f;

    int floatersUnderWater;

    //boat values
    public float movePower = 100f;
    public float steerPower = 10.0f;
    public float steerPercent = 0.3f;

    public Transform HeadPosition;  //for torque rotation

    public float testangle;

    public Vector3 velocity;
    public float yAngle;

    public float velocityRubberBend = 2.0f;
    public float yAngleRubberBend = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        boatRigid = this.GetComponent<Rigidbody>();
        waterHeight = GameObject.FindWithTag("Water").transform.Find("WaterHeightPos").position.y;

        foreach(Transform child in floaterParent.transform)
        {
            floaters.Add(child);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isSeat)
        {
            VRPlayer.transform.position = playerSeatPos.position;

            MoveBoat(velocity);
            RotateBoat(yAngle);
            ZeroVelocity();
            ZeroRotate();

            RotatePlayer();
        }
        else
        {
            VRPlayer.transform.position = playerFloorPos.position;
        }
    }

    public void ChangeSeat()
    {
        isSeat = !isSeat;
    }

    public void MoveBoat()
    {
        MoveBoat(Vector3.forward);
    }

    public void MoveBoat(Vector3 dir)
    {
        boatRigid.AddForce(dir * movePower, ForceMode.Acceleration);
    }

    public void MoveBoat(Vector3 dir, float scalar)
    {
        dir.Normalize();

        boatRigid.AddForce(-dir * Mathf.Pow(scalar, 2) * movePower * 10, ForceMode.Acceleration);
    }

    public void RotateBoat (Quaternion diffQuaternion)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, transform.rotation*diffQuaternion, steerPercent);
    }

    public void RotateBoat (float angle)
    {
        boatRigid.AddForceAtPosition(transform.right * angle * steerPower * 0.05f, HeadPosition.position, ForceMode.Acceleration);
    }

    private void OnTriggerStay(Collider other)
    {
        floatersUnderWater = 0;
        foreach(Transform floater in floaters)
        {
            if (other.tag == "Water")
            {
                float difference = floater.position.y - waterHeight;
                //print(difference);
                isWater = true;
                

                if(difference < 0)
                {
                    boatRigid.AddForceAtPosition(Vector3.up * floatingPower * Mathf.Abs(difference), floater.position, ForceMode.Force);
                    SwitchState(true);
                    floatersUnderWater++;
                }
            }
        }

        if (isWater && floatersUnderWater == 0)
        {
            SwitchState(false);
        }

    }

    void ZeroVelocity()
    {
        //print(velocity);
        if(velocity == Vector3.zero || Vector3.Distance(velocity, Vector3.zero) < 0.001f)
        {
            velocity = Vector3.zero;
            return;
        }

        velocity = Vector3.Lerp(velocity, Vector3.zero, Time.deltaTime * velocityRubberBend);
    }

    void ZeroRotate()
    {
        if(yAngle == 0|| Mathf.Abs(yAngle) < 0.001f)
        {
            yAngle = 0;
            return;
        }

        yAngle = Mathf.Lerp(yAngle, 0, Time.deltaTime * yAngleRubberBend);
    }

    private void OnTriggerExit(Collider other)
    {
        if (isWater)
        {
            isWater = false;
        }
    }

    public void SwitchState(bool isWater)
    {
        if (isWater)
        {
            boatRigid.drag = underWaterDrag;
            boatRigid.angularDrag = underWaterAngularDrag;
        }
        else
        {
            boatRigid.drag = airDrag;
            boatRigid.angularDrag = angularDrag;
        }
    }

    void RotatePlayer()
    {
        Quaternion DestAngle;
        DestAngle = transform.rotation;
        DestAngle.x = VRPlayer.rotation.x;
        DestAngle.z = VRPlayer.rotation.z;

        VRPlayer.rotation = Quaternion.Slerp(VRPlayer.rotation, DestAngle, Time.deltaTime);
    }
}
