using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddlePositionManager : MonoBehaviour
{
    public bool isGrapped;
    private bool onceFlagChanged = false;

    public Transform paddlePlaceHolder;

    // Start is called before the first frame update
    void Start()
    {
        isGrapped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGrapped)
        {
            if(!onceFlagChanged)
            {
                onceFlagChanged = true;

                this.transform.position = paddlePlaceHolder.position;
                this.transform.rotation = paddlePlaceHolder.rotation;
            }
        }
        else
        {
        }
    }

    public void SetGrapped(bool flag)
    {
        isGrapped = flag;
        onceFlagChanged = false;
    }
}
