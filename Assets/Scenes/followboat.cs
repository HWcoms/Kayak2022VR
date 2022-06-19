using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followboat : MonoBehaviour
{
    GameObject boat_parent;

    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        boat_parent = GameObject.Find("BoatParent");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = boat_parent.transform.position + offset;
    }
}
