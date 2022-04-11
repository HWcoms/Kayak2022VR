using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMoveController : MonoBehaviour
{
    public Transform VRPlayer;
    private Transform playerSeatPos;
    private Rigidbody boatRigid;

    [SerializeField] private bool isWater;

    public float underWaterDrag = 3f;
    public float underWaterAngularDrag = 1f;

    public float airDrag = 0f;
    public float angularDrag = 0.05f;

    public float floatingPower = 15f;

    public float waterHeight = 0f;

    // Start is called before the first frame update
    void Start()
    {
        playerSeatPos = transform.Find("PlayerPos");
        boatRigid = this.GetComponent<Rigidbody>();
        waterHeight = GameObject.FindWithTag("Water").transform.Find("WaterHeightPos").position.y;
    }

    // Update is called once per frame
    void Update()
    {
      //  VRPlayer.transform.position = playerSeatPos.position;

        
    }

    public void MoveBoat(float power)
    {
        //transform.position += Vector3.forward * power * 0.01f;
        boatRigid.AddForce(Vector3.forward * power);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Water")
        {
            float difference = transform.position.y - waterHeight;
            print(difference);
            isWater = true;
            SwitchState(true);

            boatRigid.AddForceAtPosition(Vector3.up * floatingPower * Mathf.Abs(difference), transform.position, ForceMode.Force);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isWater)
        {
            isWater = false;
            SwitchState(false);
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
