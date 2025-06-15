using UnityEngine;
using UnityEngine.InputSystem;
public class CreditsInput : MonoBehaviour
{
    InputAction buttonAction;
    [SerializeField] private Animator creditAnimator;
    private void Start()
    {
        buttonAction = new InputAction(binding: "/*/<button>");
        buttonAction.started += OnButtonPressed;
        buttonAction.Enable();
        creditAnimator.SetBool("show", true);
    }

    void OnButtonPressed(InputAction.CallbackContext context)
    {
        buttonAction.started -= OnButtonPressed;
        buttonAction.Disable();
        creditAnimator.SetBool("show", false);
        GameManager.Instance.GoToMainMenu();
    }
}
