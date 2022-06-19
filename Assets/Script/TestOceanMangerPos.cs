using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOceanMangerPos : MonoBehaviour
{
    public OceanManager oceanManager;
    public Transform posDebug;
    // Start is called before the first frame update
    void Start()
    {
        oceanManager = FindObjectOfType<OceanManager>();
    }

    // Update is called once per frame
    void Update()
    {
        posDebug.transform.position = new Vector3(this.transform.position.x, oceanManager.WaterHeightAtPosition(this.transform.position), this.transform.position.z);
    }
}
