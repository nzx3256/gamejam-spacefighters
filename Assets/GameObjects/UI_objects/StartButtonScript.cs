using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StartButtonScript : MonoBehaviour
{
    private Button b; 
    private void Start()
    {
        b = GetComponent<Button>();
    }
    private void Update()
    {
        PlayerInputManager pim = PlayerInputManager.instance;
        if(pim != null)
        {
            if(pim.playerCount > 0)
            {
                b.interactable = true;
            }
            else
            {
                b.interactable = false;
            }
        }
    }
}
