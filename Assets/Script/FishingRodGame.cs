using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRodGame : MonoBehaviour
{
    float originalRodLen;
    public float destRodLen = 3;
    public float currentRodLen;

    Rope rope;

    bool isChangeReady = true;

    public PaddlePositionManager pmcScript;

    // Start is called before the first frame update
    void Start()
    {
        rope = GetComponent<Rope>();
        originalRodLen = rope.ropeSegLen;
        destRodLen = destRodLen * originalRodLen;

        currentRodLen = originalRodLen;
    }

    // Update is called once per frame
    void Update()
    {
        rope.ropeSegLen = currentRodLen;
    }

    public void ChangeRodLength()
    {
        if(!pmcScript.isGrapped)
        {
            rope.ropeSegLen = originalRodLen;
            isChangeReady = true;
            return;
        }

        if (!isChangeReady) return;

        if(currentRodLen == originalRodLen)
        {
            StartCoroutine(RodLength(1));
        }
        else
        {
            StartCoroutine(RodLength(0));
        }
    }
    IEnumerator RodLength(int mode, float speed = 2.0f)
    {
        if (!isChangeReady) yield break;

        while (currentRodLen >= originalRodLen && currentRodLen <= destRodLen)
        {
            if(mode == 0)
            {
                currentRodLen -= Time.deltaTime * speed;
            }
            else
            {
                currentRodLen += Time.deltaTime * speed;
            }

            yield return new WaitForSeconds(0.05f);
        }

        if (currentRodLen <= originalRodLen)
            currentRodLen = originalRodLen;
        if (currentRodLen >= destRodLen)
            currentRodLen = destRodLen;

        isChangeReady = true;
    }
}
