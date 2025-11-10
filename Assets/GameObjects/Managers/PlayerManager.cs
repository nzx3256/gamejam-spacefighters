using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInput))]
public class PlayerManager : MonoBehaviour
{
    public static int count = 0;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {
        count = 0;
    }

    [SerializeField]
    private GameObject playerPrefab;

    public GameObject playerReference;
    private PlayerInput inputComponent;

    public Color playerColor;
    public string fighterType;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLoadScene;
        UIButtonScript.gameStart += SetPrefabFromType;
    }

    private void Start()
    {
        gameObject.name = "PlayerManager" + count;
        if(++count > 2)
        {
            count = 0;
        }

        inputComponent = GetComponent<PlayerInput>();

        DontDestroyOnLoad(this);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLoadScene;
        UIButtonScript.gameStart -= SetPrefabFromType;
        if(playerReference != null)
        {
            PlayerController pc = playerReference.GetComponent<PlayerController>();
            SetInputAction("Move", pc.onMove,true); //unsetting move action
            SetInputAction("Fire", pc.onFire,true); //unsetting fire action
        }
    }

    private void OnLoadScene(Scene scene, LoadSceneMode node)
    {
        //Debug.Log("Found BoundingBox");
        CreatePlayer(Camera.main.transform);
        if(scene.buildIndex == 0)
        {
            Destroy(gameObject);
        }
    }

    private void SetPrefabFromType()
    {
        //Debug.Log("SetPrefabFromType called");
        if(PlayerInputManager.instance != null)
        {
            if(!MultiplayerManager.shipPrefabDictionary.TryGetValue(fighterType, out playerPrefab))
            {
                //Debug.Log("WARNING: Can't find key value \""+ fighterType + "\" in MultiplayerManager.shipPrefabDictionary.");
            }
            else
            {
                //Debug.Log("playerPrefab successfully set");
            }
        }
        else { 
            //Debug.Log("PlayerInstance is null"); 
        }
    }

    private void CreatePlayer(Transform b)
    {
        if(playerReference != null)
        {
            Destroy(playerReference);
        }
        playerReference = GameObject.Instantiate(playerPrefab, b.position, Quaternion.identity);
        playerReference.GetComponent<SpriteRenderer>().color = playerColor;
        PlayerController pc = playerReference.GetComponent<PlayerController>();
        SetInputAction("Move", pc.onMove);
        SetInputAction("Fire", pc.onFire);
    }

    private void DestroyPlayer()
    {
        if(playerReference != null)
        {
            PlayerController pc = playerReference.GetComponent<PlayerController>();
            SetInputAction("Move", pc.onMove,true); //unsetting move action
            SetInputAction("Fire", pc.onFire,true); //unsetting fire action
            GameObject.Destroy(playerReference);
        }
    }

    private void SetInputAction(string actionName, Action<InputAction.CallbackContext> function, bool unset = false)
    {
        if(!unset)
        {
            inputComponent.actions[actionName].started += function;
            inputComponent.actions[actionName].performed += function;
            inputComponent.actions[actionName].canceled += function;
        }
        else 
        {
            inputComponent.actions[actionName].started -= function;
            inputComponent.actions[actionName].performed -= function;
            inputComponent.actions[actionName].canceled -= function;
        }
    }
}
