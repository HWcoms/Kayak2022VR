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
    public float scalarVector;

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
        currentVector = transform.position;

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
            diffVector = currentVector - enterVector;
            scalarVector = diffVector.magnitude;

            diffVector.y = 0;

            diffAngle = Vector3.SignedAngle(prev_HtoP_Vector, cur_HtoP_Vector, Vector3.up);

            //unneccesary
            diffQuaternion = Quaternion.FromToRotation(prev_HtoP_Vector, cur_HtoP_Vector);
            testCube.transform.rotation = transform.rotation * diffQuaternion;


            //add power and angle
            if(scalarVector != 0)
            {
                //print("addpower");
                BMCscript.velocity -= diffVector * 0.02f;
            }

            if (Mathf.Abs(diffAngle) != 0)
            {
                //print("addrotation");
                BMCscript.yAngle -= diffAngle * 0.01f;
            }

            //reset Vectors
            enterVector = currentVector;
            prev_HtoP_Vector = cur_HtoP_Vector;
        }
    }

    Vector3 GetHandToPaddleDir()
    {
        handToPaddleVector = paddleT.position - handT.position;
        handToPaddleVector.Normalize();

        return handToPaddleVector;
    }
}
