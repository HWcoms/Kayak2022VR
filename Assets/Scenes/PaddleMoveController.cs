using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMoveController : MonoBehaviour
{
    public BoatMoveController BMCscript;
    public float moveSpeed;

    private Vector3 enterVector;
    private Vector3 currentVector;
    public Vector3 diffVector;

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
            if (Vector3.Distance(enterVector, currentVector) > 0.01f)
            {
                print("move boat");
                diffVector = currentVector - enterVector;
                diffVector.y = 0;

                BMCscript.MoveBoat(-diffVector, moveSpeed);
                enterVector = currentVector;
            }

        }
    }
}
