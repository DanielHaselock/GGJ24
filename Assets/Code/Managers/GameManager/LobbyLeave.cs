using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyLeave : MonoBehaviour
{
    [SerializeField] private InputAction leaveAction;
    private void Start()
    {
        leaveAction.started += OnButtonPressed;
        leaveAction.canceled += OnButtonPressed;
        leaveAction.Enable();
    }

    void OnButtonPressed(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            // If in the lobby, reset the start bar
            LobbyUIManager lobbyUIManager = FindAnyObjectByType<LobbyUIManager>();
            if (lobbyUIManager != null)
            {
                lobbyUIManager.FillCancelBar(context.control.device);
            }
        }
        else if(context.canceled)
        {
            LobbyUIManager lobbyUIManager = FindAnyObjectByType<LobbyUIManager>();
            if (lobbyUIManager != null)
            {
                lobbyUIManager.CancelFillCancelBar(context.control.device);
            }
        }
    }

    private void OnDestroy()
    {
        leaveAction.started -= OnButtonPressed;
        leaveAction.canceled -= OnButtonPressed;
        leaveAction.Disable();
    }
}
