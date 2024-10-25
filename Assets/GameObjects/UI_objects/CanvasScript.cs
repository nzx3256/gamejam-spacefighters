using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScript : MonoBehaviour
{
    [HideInInspector]
    public GameObject currentPanel;
    [SerializeField]
    private List<GameObject> panels;
    private Stack previousPanels;

    private void Start()
    {
        GetActivePanel();
        previousPanels = new Stack();
    }

    private void GetActivePanel(){
        foreach(GameObject panel in panels)
        {
            if(panel.activeInHierarchy) 
            {
                currentPanel = panel;
            }
        }
        foreach(GameObject panel in panels)
        {
            panel.SetActive(false);
        }
        currentPanel.SetActive(true);
        //Debug.Log("\"" + currentPanel.name + "\" is the active panel");
    }
    
    public void SetPanel(GameObject panel)
    {
        previousPanels.Push(currentPanel);
        currentPanel.SetActive(false); // becasuse last panel hasn't be overwritten yet
        currentPanel = panel;
        currentPanel.SetActive(true);
    }

    public void GoBack()
    {
        if(previousPanels.Count != 0)
        {
            SetPanel((GameObject)previousPanels.Pop());
        }
    }
}
