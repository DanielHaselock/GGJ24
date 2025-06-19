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
                lobbyUIManager.FillCancelBar();
            }
        }
        else if(context.canceled)
        {
            Debug.Log("CANCELLING WITH LOBBYLEAVE");
            LobbyUIManager lobbyUIManager = FindAnyObjectByType<LobbyUIManager>();
            if (lobbyUIManager != null)
            {
                lobbyUIManager.CancelFillCancelBar();
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
