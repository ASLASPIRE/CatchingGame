using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuggingTools : MonoBehaviour
{
    public BasketController basketController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            basketController.AddScore();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            basketController.LoseLife();
        }
    }
}
