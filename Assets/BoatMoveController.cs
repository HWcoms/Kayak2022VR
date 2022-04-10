using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMoveController : MonoBehaviour
{
    public Transform VRPlayer;
    private Transform playerSeatPos;

    // Start is called before the first frame update
    void Start()
    {
        playerSeatPos = transform.Find("PlayerPos");
    }

    // Update is called once per frame
    void Update()
    {
      //  VRPlayer.transform.position = playerSeatPos.position;
    }

    public void MoveBoat(float power)
    {
        transform.position += Vector3.forward * power * 0.01f;
    }
}
