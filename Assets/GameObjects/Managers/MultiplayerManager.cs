using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> ShipPrefabs;

    private PlayerInputManager inputManager;
    private static Dictionary<string, GameObject> _shipPrefabDictionary;

    public static Dictionary<string, GameObject> shipPrefabDictionary
    {
        get { return _shipPrefabDictionary; }
    }

    private void Start()
    {
        if(!TryGetComponent(out inputManager))
        {
            Debug.LogError("Must have \"PlayerInputManager\" Unity Component on MultiplayerManager");
            this.enabled = false;
        }
        _shipPrefabDictionary = new Dictionary<string, GameObject>();
        _Init_PrefabDict();
        StartCoroutine(Ping());
    }
    
    private void _Init_PrefabDict()
    {
        string prefabNames = "";
        foreach(GameObject prefab in ShipPrefabs)
        {
            if(!_shipPrefabDictionary.ContainsKey(prefab.name))
            {
                _shipPrefabDictionary.Add(prefab.name, prefab);
                prefabNames += prefab.name + " ";
            }
            else
            {
                Debug.Log("Dictionary<string, GameObject> already contains an instance of \"" + prefab.name + "\".\n Discarding to prevent collisions.");
            }
        }
        Debug.Log(prefabNames);
    }

    private void Update()
    {
    }

    private IEnumerator Ping()
    {
        while(inputManager.enabled)
        {
            yield return new WaitForSeconds(3f);
            try
            {
                Debug.Log("Player Count: " + inputManager.playerCount);
            }
            catch(Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }
    }
}
