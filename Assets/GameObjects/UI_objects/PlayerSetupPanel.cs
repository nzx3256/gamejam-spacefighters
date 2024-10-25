using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSetupPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown fighterDDown;
    [SerializeField]
    private TMP_Dropdown colorDDown;

    [HideInInspector]
    public PlayerManager pm;

    private void Start()
    { 
        ChangeImageColor();
        ExposeFighterType();
    }

    public void ChangeImageColor()
    {
        Image previewImage = fighterDDown.captionImage;
        Color c = new Color();
        switch(colorDDown.captionText.text)
        {
            case "None":
                c = Color.white;
                break;
            case "Red":
                c = Color.red;
                break;
            case "Yellow":
                c = Color.yellow;
                break;
            case "Green":
                c = Color.green;
                break;
            case "Blue":
                c = Color.blue;
                break;
            case "Purple":
                c = Color.magenta;
                break;
            default:
                break;
        }
        previewImage.color = c;
        pm.playerColor = c;
    }

    public void ExposeFighterType()
    {
        pm.fighterType = fighterDDown.captionText.text.ToLower();
    }
}
