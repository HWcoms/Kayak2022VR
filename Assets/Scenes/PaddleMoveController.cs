using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMoveController : MonoBehaviour
{
    public BoatMoveController BMCscript;
    //public float moveSpeed;

    private Vector3 enterVector;
    private Vector3 currentVector;
    public Vector3 diffVector;

    public Transform handT;
    private Transform paddleT;
    [SerializeField] private float diffAngle;    //paddle-hand angle differnce
    [SerializeField] private Quaternion diffQuaternion; 
    private Vector3 handToPaddleVector;

    private Vector3 cur_HtoP_Vector;
    private Vector3 prev_HtoP_Vector;
    public GameObject testCube;

    // Start is called before the first frame update
    void Start()
    {
        BMCscript = GameObject.FindGameObjectWithTag("Kayak").GetComponent<BoatMoveController>();
        paddleT = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        currentVector = handT.position - transform.position;

        cur_HtoP_Vector = GetHandToPaddleDir();
    }

    void OnTriggerEnter(Collider cols)
    {
        enterVector = currentVector;
        prev_HtoP_Vector = cur_HtoP_Vector;
    }

    void OnTriggerStay (Collider cols)
    {
        if (cols.transform.tag == "Water")
        {
            if (Vector3.Distance(enterVector, currentVector) > 0.01f)
            //if (Vector3.Distance(enterVector, currentVector) > 1f)
            {
                print("move boat");
                diffVector = currentVector - enterVector;
                diffVector.y = 0;

                diffAngle = Vector3.Angle(prev_HtoP_Vector, cur_HtoP_Vector);
                diffQuaternion = Quaternion.FromToRotation(prev_HtoP_Vector, cur_HtoP_Vector);

                //print(diffAngle);

                diffQuaternion.x = 0; diffQuaternion.z = 0; diffQuaternion.y *= 1;
                testCube.transform.rotation = transform.rotation * diffQuaternion;

                print(diffQuaternion.y);
                BMCscript.MoveBoat(diffVector);
                BMCscript.RotateBoat(diffQuaternion.y);
                enterVector = currentVector;
                prev_HtoP_Vector = cur_HtoP_Vector;
            }

        }
    }

    Vector3 GetHandToPaddleDir()
    {
        handToPaddleVector = paddleT.position - handT.position;
        handToPaddleVector.Normalize();

        return handToPaddleVector;
    }
}
