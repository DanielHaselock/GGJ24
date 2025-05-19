using System.Collections.Generic;
using UnityEditor.DeviceSimulation;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public List<InputDevice> inputDevices;

    private PlayerInputManager inputManager;

    public int playerCount = 0;

    [SerializeField] private int MAX_PLAYERS = 4;

    public void OnPlayerJoined(PlayerInput input)
    {
        Debug.Log("Player Joined: " + input.user + " with device: " + input.devices[0]);
        JoinPlayer(input.devices[0]);
    }

    

    void JoinPlayer(InputDevice device)
    {
        Debug.Log("Joining player with device: " + device);
        
    }

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this.gameObject);
        else Instance = this;
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
