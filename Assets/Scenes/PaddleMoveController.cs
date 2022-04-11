using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMoveController : MonoBehaviour
{
    public BoatMoveController BMCscript;
    public float moveSpeed;

    public Vector3 enterVector;
    public Vector3 currentVector;

    // Start is called before the first frame update
    void Start()
    {
        BMCscript = GameObject.FindGameObjectWithTag("Kayak").GetComponent<BoatMoveController>();
    }

    // Update is called once per frame
    void Update()
    {
        currentVector = transform.position;
    }

    void OnTriggerEnter(Collider cols)
    {
        enterVector = transform.position;
    }

    void OnTriggerStay (Collider cols)
    {
        if (cols.transform.tag == "Water")
        {
            if (Vector3.Distance(enterVector, currentVector) > 1.0f)
            {
                print("move boat");
                BMCscript.MoveBoat(moveSpeed);
                enterVector = currentVector;
            }

        }
    }
}
