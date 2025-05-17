using System.Collections.Generic;
using UnityEditor.DeviceSimulation;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public List<InputDevice> inputDevices;

    private PlayerInputManager inputManager;

    [SerializeField] private int MAX_PLAYERS = 4;

    public void OnPlayerJoined(PlayerInput input)
    {
        Debug.Log("Player joined");

        // Example: Call JoinPlayer with the first device (most setups have only one device per player)
        InputDevice triggeringDevice = input.devices[0];
        JoinPlayer(triggeringDevice);
        
    }

    

    void JoinPlayer(InputDevice device)
    {
        Debug.Log("Attempting to Join");

        if (inputDevices.Contains(device) || device.name == "Mouse")
            return;

        Debug.Log("Joining with device: " + device.name);
    
        /*string controlScheme = "Gameplay";

        Debug.Log("Attempt to Instantiate");
        
        PlayerInput player = PlayerInput.Instantiate(
            inputManager.playerPrefab,
            controlScheme: controlScheme,
            pairWithDevice: device);
        */
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputManager = GetComponent<PlayerInputManager>();
        // Get all connected input devices at start
        inputDevices = new List<InputDevice>(InputSystem.devices);
        Debug.Log("Devices at start: " + inputDevices.Count);
        foreach(InputDevice device in inputDevices){
            Debug.Log(device);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Optionally, update the list of devices every frame
        // inputDevices = new List<InputDevice>(InputSystem.devices);
    }
}
