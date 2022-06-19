using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    public bool isStarted;

    public GameObject[] menuButtons;

    public Transform VRPlayer;
    public Transform destPos;
    public Transform playerSeatPos;

    public Coroutine currentC;

    //fade
    public MeshRenderer fadeRenderer;
    public float fadeDuration = 2f;
    public Color fadeColor;

    // Start is called before the first frame update
    void Start()
    {
        isStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStarted)
        {
            foreach (GameObject g in menuButtons)
            {
                g.SetActive(false);
                VRPlayer.transform.position = playerSeatPos.position;
            }
        }
        else
        {
            foreach (GameObject g in menuButtons)
            {
                g.SetActive(true);
                VRPlayer.transform.position = destPos.position;
            }
        }

    }

    public void StartGame()
    {
        if (currentC == null)
        {
            currentC = StartCoroutine(StartAnimation());
            print("start coroutine");
        }
    }

    IEnumerator StartAnimation()
    {
        FadeOut();
        yield return new WaitForSeconds(fadeDuration);

        isStarted = true;
        GameObject.FindObjectOfType<TestButtonClick>().MenuButtonToggle();

        yield return new WaitForSeconds(0.5f);

        FadeIn();
    }

    void FadeIn()
    {
        Fade(1, 0);
    }
    void FadeOut()
    {
        Fade(0, 1);
    }

    void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    public IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        float timer = 0;
        while (timer <= fadeDuration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);

            fadeRenderer.material.SetColor("_BaseColor", newColor);

            timer += Time.deltaTime;
            yield return null;
        }

        Color newColor2 = fadeColor;
        newColor2.a = alphaOut;

        fadeRenderer.material.SetColor("_BaseColor", newColor2);
    }
}
