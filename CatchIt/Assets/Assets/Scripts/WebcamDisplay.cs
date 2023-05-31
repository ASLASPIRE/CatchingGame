using UnityEngine;
using UnityEngine.UI;

public class WebcamDisplay : MonoBehaviour
{
    public RawImage displayImage; // Reference to the RawImage component where the webcam output will be displayed

    private WebCamTexture webcamTexture;

    void Start()
    {
        

        // Create a new WebCamTexture with the first available webcam device
        
        
    }

    public void turnOnWebcam(){

        // // Check if any webcams are available
        // if (WebCamTexture.devices.Length == 0)
        // {
        //     Debug.LogError("No webcams found.");
        //     return;
        // }
        
        // webcamTexture = new WebCamTexture(WebCamTexture.devices[0].name);
        // //webcamTexture.textureFormat = TextureFormat.RGB24;


        

        // // Start the webcam feed
        // webcamTexture.Play();

        // // Assign the webcam feed to the RawImage component for display
        // displayImage.texture = webcamTexture;

        // // Mirror the RawImage component horizontally by modifying the UV coordinates
        // displayImage.uvRect = new Rect(1f, 0f, -1f, 1f);
    }

    public void turnOffWebcam(){
        // Debug.Log("Should turn off webcam");
        // webcamTexture.Stop();
        // webcamTexture.Stop();
        // webcamTexture = null;
        // displayImage.texture = null;
    }
}
