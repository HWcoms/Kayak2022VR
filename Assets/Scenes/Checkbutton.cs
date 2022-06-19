using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkbutton : MonoBehaviour
{
    public GameObject[] buttons;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RunButton()
    {
        foreach(GameObject button in buttons)
        {
            buttoncolor buttonC = button.GetComponent<buttoncolor>();

            if(buttonC.hovered)
            {
                if(buttonC.mode == 0)   //start
                {
                    print("start");
                    GameObject.FindObjectOfType<GameStartManager>().StartGame();
                }
                else if(buttonC.mode == 1) //quit game
                {
                    print("end");
                    Application.Quit();
                }
                else //mainmenu
                {
                    print("reload");
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }

        }
    }
}
