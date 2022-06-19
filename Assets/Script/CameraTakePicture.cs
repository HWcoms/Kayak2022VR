using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CameraTakePicture : MonoBehaviour
{
    [SerializeField] RawImage LiveDisplayImage;
    [SerializeField] RawImage LastTakenImage;
    [SerializeField] Camera LinkedCamera;
    [SerializeField] RenderTexture CameraRT;

    [SerializeField] float showLastImageFor = 2.0f;
    Texture2D LastImage;

    float LastImageShowTimeRemaining = -1f;

    PaddlePositionManager isGrabbedScript;

    //audio
    public AudioClip shutterAudio;


    // Start is called before the first frame update
    void Start()
    {
        LastImage = new Texture2D(CameraRT.width, CameraRT.height,
            CameraRT.graphicsFormat, 
            UnityEngine.Experimental.Rendering.TextureCreationFlags.None);

        isGrabbedScript = GetComponent<PaddlePositionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(LastImageShowTimeRemaining > 0f)
        {
            LastImageShowTimeRemaining -= Time.deltaTime;

            if(LastImageShowTimeRemaining <= 0f)
            {
                LiveDisplayImage.gameObject.SetActive(true);
                LastTakenImage.gameObject.SetActive(false);
            }
        }
    }

    public void TakePicture()
    {
        if (!isGrabbedScript.isGrapped) return;

        //transfer the texture - GPU side only
        //Graphics.CopyTexture(CameraRT, LastImage);
        AsyncGPUReadback.Request(CameraRT, 0, (AsyncGPUReadbackRequest action) =>
        {
            LastImage.SetPixelData(action.GetData<byte>(), 0);
            LastImage.Apply();

            //update the last taken image display
            LastTakenImage.texture = LastImage;

            //file
            string fileName = $"Shot_{System.DateTime.Now.ToString("dd/MM/yyyy_HHmmssff")}.jpeg";
            fileName = System.IO.Path.Combine(Application.persistentDataPath, fileName);
            System.IO.File.WriteAllBytes(fileName, LastImage.EncodeToJPG());

            //Debug.Log("fileName: "+fileName);

            //show the last iamge
            LiveDisplayImage.gameObject.SetActive(false);
            LastTakenImage.gameObject.SetActive(true);

            //show the image for a time
            LastImageShowTimeRemaining = showLastImageFor;

            AudioSource.PlayClipAtPoint(shutterAudio, transform.position);
        });
    }
}
