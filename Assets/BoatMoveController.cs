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

    public float movePower = 100f;
    public float steerPower = 10.0f;
    public float steerPercent = 0.3f;

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
            //VRPlayer.transform.rotation = playerSeatPos.rotation;
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
        //transform.position += Vector3.forward * power * 0.01f;
        //boatRigid.AddForce(Vector3.forward * power);
        MoveBoat(Vector3.forward);
    }

    public void MoveBoat(Vector3 dir)
    {
        dir.Normalize();

        boatRigid.AddForceAtPosition(dir * movePower, transform.position, ForceMode.Force);
    }

    public void RotateBoat (Quaternion diffQuaternion)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, transform.rotation*diffQuaternion, steerPercent);
    }

    public void RotateBoat (float angle)
    {
        boatRigid.AddTorque(Vector3.up * angle * steerPower * 250, ForceMode.Force);
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
}
