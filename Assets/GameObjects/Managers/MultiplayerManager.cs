using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MultiplayerManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> ShipPrefabs;
    [SerializeField]
    private GameObject Player1Panel;
    [SerializeField]
    private GameObject Player2Panel;

    private static Dictionary<string, GameObject> _shipPrefabDictionary;
    public static Dictionary<string, GameObject> shipPrefabDictionary
    {
        get { return _shipPrefabDictionary; }
    }

    public void OnPlayerJoin(PlayerInput comp)
    {
        int pCount = PlayerInputManager.instance.playerCount;
        GameObject obj = null;
        //Set startbutton to interactable
        if(pCount == 1)
        {
            obj = Player1Panel;
        }
        else if(pCount == 2)
        {
            obj = Player2Panel;
        }
        if(obj != null)
        {
            obj.SetActive(true);
            obj.GetComponent<PlayerSetupPanel>().pm = comp.gameObject.GetComponent<PlayerManager>();
        }
    }

    public void OnPlayerLeft(PlayerInput comp)
    {
        Debug.Log("Player Left");
        int pCount = PlayerInputManager.instance.playerCount;
        if(pCount < 1)
        {
            //Disable StartButton
        }
    }

    private void Start()
    {
        if(PlayerInputManager.instance == null)
        {
            //Debug.Log("WARNING: Disabling \"" + this.name
            //        + "\" because there is no active PlayerInputManager in the scene.");
            this.enabled = false;
        }
        _shipPrefabDictionary = new Dictionary<string, GameObject>();
        _Init_PrefabDict();
        //StartCoroutine(Ping());
    }

    private void _Init_PrefabDict()
    {
        string prefabNames = "";
        foreach(GameObject prefab in ShipPrefabs)
        {
            string name = prefab.name.Split("_")[0].ToLower();
            if(!_shipPrefabDictionary.ContainsKey(name))
            {
                //Debug.Log("Dict _Init_ :: " + name + "");
                _shipPrefabDictionary.Add(name, prefab);
                prefabNames += prefab.name + " ";
            }
            else
            {
                //Debug.Log("Dictionary<string, GameObject> already contains an instance of \"" + name + "\".\n Discarding to prevent collisions.");
            }
        }
        //Debug.Log(prefabNames);
    }

    private IEnumerator Ping()
    {
        while(PlayerInputManager.instance.enabled)
        {
            yield return new WaitForSeconds(3f);
            try
            {
                //Debug.Log("Player Count: " + PlayerInputManager.instance.playerCount);
            }
            catch(Exception ex)
            {
                //Debug.Log(ex.Message);
            }
        }
    }
}
