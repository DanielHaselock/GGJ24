using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinHandler : MonoBehaviour
{
     public void OnPlayerJoined(PlayerInput input)
    {
        PlayerManager.Instance.RegisterPlayer(input);
    }
}
