using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttoncolor : MonoBehaviour
{
    public int mode = 0;    //0 start 1 quit   2 mainmenu

    public bool hovered;

    public MeshRenderer renderer;

    public Color colorHover = Color.red;
    Color original;

    // Start is called before the first frame update
    void Start()
    {
        original = renderer.materials[1].color;
    }

    // Update is called once per frame
    void Update()
    {
        //renderer.materials[1].color = colorHover;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "laser")
        {
            hovered = true;
            renderer.materials[1].color = colorHover;
        }
    }
            
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "laser")
        {
            renderer.materials[1].color = original;
            hovered = false;
        }
    }
}
