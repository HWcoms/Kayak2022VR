using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddlePositionManager : MonoBehaviour
{
    public bool isGrapped;
    private bool onceFlagChanged = false;

    public Transform paddlePlaceHolder;

    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        isGrapped = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isGrapped)
        {
            if(!onceFlagChanged)
            {
                onceFlagChanged = true;

                this.transform.position = paddlePlaceHolder.position;
                this.transform.rotation = paddlePlaceHolder.rotation;

            }

            rb.isKinematic = true;

            this.transform.position = paddlePlaceHolder.position;
            this.transform.rotation = paddlePlaceHolder.rotation;
        }
        else
        {
            rb.isKinematic = false;
        }
    }

    public void SetGrapped(bool flag)
    {
        isGrapped = flag;
        onceFlagChanged = false;
    }
}
