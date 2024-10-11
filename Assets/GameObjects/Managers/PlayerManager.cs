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

    public GameObject playerObject;
    private PlayerInput inputComponent;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLoadScene;
    }

    private void Start()
    {
        gameObject.name = "PlayerManager" + count;
        count++;

        inputComponent = GetComponent<PlayerInput>();

        DontDestroyOnLoad(this);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLoadScene;
    }

    private void OnLoadScene(Scene scene, LoadSceneMode node)
    {
        BoundingBoxScript boundingBox = FindObjectOfType<BoundingBoxScript>();
        if(boundingBox != null) //Meaning the scene is in a playable game state on load
        {
            Debug.Log("Found BoundingBox");
            CreatePlayer(boundingBox.transform);
        }
    }

    private void CreatePlayer(Transform b)
    {
        GameObject PlayerRef = Instantiate(playerObject, b.position, Quaternion.identity);
        PlayerInput inputPresets = PlayerRef.GetComponent<PlayerInput>();
        PlayerController controller = PlayerRef.GetComponent<PlayerController>();
        if(inputPresets != null){
            if(inputPresets.actions != null)
            {
                inputComponent.actions = inputPresets.actions;
            }
            foreach(InputAction action in inputComponent.actions)
            {
                try
                {
                    string name = action.name;
                    action.actionMap.actionTriggered = inputPresets[name].actionMap.actionTriggered.GetInvocationList()[0];
                    action.Enable();
                }
                catch(Exception ex)
                {
                    Debug.LogError(ex.Message);
                }
            }
            inputPresets.enabled = false;
        }
    }
}
