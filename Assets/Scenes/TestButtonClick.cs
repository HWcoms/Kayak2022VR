using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class TestButtonClick : MonoBehaviour
{
    [SerializeField] bool menuToggle = true;
    bool started = false;

    public GameObject[] menuPrefabs;

    private GameStartManager gsmScript;

    // Start is called before the first frame update
    void Start()
    {
        gsmScript = GameObject.FindObjectOfType<GameStartManager>();
    }

    // Update is called once per frame
    void Update()
    {
        /**
        if(Keyboard.current.fKey.wasPressedThisFrame)
        {
            GameObject.FindObjectOfType<Checkbutton>().RunButton();
            print("click");
        }
        */

        started = gsmScript.isStarted;

        MenuRefresh();
    }

    public void RunButton()
    {
        GameObject.FindObjectOfType<Checkbutton>().RunButton();

        GameObject.FindObjectOfType<CameraTakePicture>().TakePicture();

        GameObject.FindObjectOfType<FishingRodGame>().ChangeRodLength();
    }

    public void MenuButtonToggle()
    {
        menuToggle = !menuToggle;
    }

    void MenuRefresh()
    {
        foreach (GameObject obj in menuPrefabs)
        {
             obj.SetActive(menuToggle);
        }

        if(!started)
        {
            menuToggle = true;

            menuPrefabs[0].SetActive(true);
            menuPrefabs[1].SetActive(false);
        }
    }

}
