using UnityEngine;
using UnityEngine.UI;

public class TouchHereFade : MonoBehaviour
{
    public float fadeSpeed = 1f;        // Speed of the fading effect
    public Color startColor;            // Initial color of the image
    public Color endColor;              // Color to fade towards

    private Image image;
    private float t = 0f;               // Interpolation parameter

    private void Start()
    {
        image = GetComponent<Image>();
        image.color = startColor;
    }

    private void Update()
    {
        t = Mathf.PingPong(Time.time * fadeSpeed, 1f);  // Use PingPong function to oscillate t between 0 and 1
        image.color = Color.Lerp(startColor, endColor, t);
    }
}

