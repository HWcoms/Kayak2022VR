using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMoveController : MonoBehaviour
{
    public Collider collider;
    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay (Collider cols)
    {
        print(cols.transform.tag);
        if (cols.transform.tag == "Water")
        {
            print("paddle is colliding with water");

        }
    }

    void OnCollisionStay(Collision collision)
    {   
        //unnecessary
        print(collision.transform.tag);
    }
}
