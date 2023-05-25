using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input : MonoBehaviour
{

    public TouchScreenKeyboard keyboard;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OpenSystemKeyboard()
    {
        Debug.Log("se abre");
        keyboard = TouchScreenKeyboard.Open("text to edit");
    }   

    // Update is called once per frame
    void Update()
    {
        
    }
}
