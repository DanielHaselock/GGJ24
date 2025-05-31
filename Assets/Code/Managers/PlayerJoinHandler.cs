using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Handles player joining events.
/// - Connects the PlayerManager with the PlayerInputManager.
/// - Transfers the PLayerInput to the proper PlayerController.
/// </summary>
public class PlayerJoinHandler : MonoBehaviour
{
     public void OnPlayerJoined(PlayerInput input)
    {
        PlayerManager.Instance.RegisterPlayer(input);
    }
}
