using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonScript : MonoBehaviour
{
    [SerializeField]
    private CanvasScript canvasScript;
    public delegate void GameStart();
    public static event GameStart gameStart;

    private void Start()
    {
        if(canvasScript == null)
        {
            canvasScript = GameObject.FindWithTag("UI_Canvas").GetComponent<CanvasScript>();
        }
    }

    public void PanelChange(GameObject targetPanel)
    {
        //Debug.Log("Button \"" + gameObject.name + "\" Clicked");
        canvasScript.SetPanel(targetPanel);
    }

    public void BackButton()
    {
        canvasScript.GoBack();
    }

    public void StartButton()
    {
        gameStart?.Invoke();
        SceneManager.LoadScene("TesterScene", LoadSceneMode.Single);
    }
}
