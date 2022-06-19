using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanManager : MonoBehaviour
{
    public float waveHeight = 0.5f;
    public float waveFrequency = 1f;
    public float waveSpeed = 1f;
    public Transform ocean;

    Material oceanMat;
    Texture2D wavesDisplacement;

    void Start()
    {
        
    }

    void SetVariables()
    {
        oceanMat = ocean.GetComponent<Renderer>().sharedMaterial;
        wavesDisplacement = (Texture2D)oceanMat.GetTexture("_WavesDisplacement");
    }

    public float WaterHeightAtPosition(Vector3 position)
    {
        return ocean.position.y + wavesDisplacement.GetPixelBilinear(position.x * waveFrequency, position.z * waveFrequency + Time.time * waveSpeed).g * waveHeight * ocean.localScale.x;
    }

    private void OnValidate()
    {
        if (!oceanMat)
            SetVariables();

        UpdateMaterial();
    }

    void UpdateMaterial()
    {
        oceanMat.SetFloat("_WavesHeight", waveHeight);
        oceanMat.SetFloat("_WavesFrequency", waveFrequency);
        oceanMat.SetFloat("_WavesSpeed", waveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
