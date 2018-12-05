using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class IntroScrypt : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

      
        if (hInput.GetButtonDown("Joy1A") || hInput.GetButtonDown("Joy2A") || hInput.GetButtonDown("Joy3A") || hInput.GetButtonDown("Joy4A")
            || hInput.GetButtonDown("Joy5A") || hInput.GetButtonDown("Joy6A") || hInput.GetButtonDown("Joy37") || hInput.GetButtonDown("Joy8A"))
        {
            Debug.Log("A is Pressed");
            SceneManager.LoadScene("Lobby");
        }


    }
}
