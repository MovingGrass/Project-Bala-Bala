using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PauseManager : MonoBehaviour
{
    public bool isPaused;

    public void OnPress(InputAction.CallbackContext context)
    {
        if (context.interaction is TapInteraction)
        {
            if (context.performed)
            {
                ChangePauseState();
                openCanvas();
                Debug.Log("Paused");
            }
        }
    }

    public void ChangePauseState()
    {
        isPaused = !isPaused;
    }
    private void openCanvas()
    {

    }
}

