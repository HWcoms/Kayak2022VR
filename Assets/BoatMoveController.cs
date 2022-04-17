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

    [SerializeField] private Transform[] floaters;

    public float underWaterDrag = 3f;
    public float underWaterAngularDrag = 1f;

    public float airDrag = 0f;
    public float angularDrag = 0.05f;

    public float floatingPower = 15f;

    public float waterHeight = 0f;

    int floatersUnderWater;

    // Start is called before the first frame update
    void Start()
    {
        boatRigid = this.GetComponent<Rigidbody>();
        waterHeight = GameObject.FindWithTag("Water").transform.Find("WaterHeightPos").position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(isSeat)
        {
            VRPlayer.transform.position = playerSeatPos.position;
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

    public void MoveBoat(float power)
    {
        //transform.position += Vector3.forward * power * 0.01f;
        //boatRigid.AddForce(Vector3.forward * power);
        MoveBoat(Vector3.forward, power);
    }

    public void MoveBoat(Vector3 dir, float power)
    {
        dir.Normalize();

        boatRigid.AddForceAtPosition(dir * power, transform.position, ForceMode.Force);
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
