using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.InputSystem;

public class OpenSpiritMenu : MonoBehaviour
{

    public void OnPress(InputAction.CallbackContext context)
    {
        if (context.interaction is TapInteraction)
        {
            if (context.performed)
            {
                openCanvas();
                Debug.Log("Open Spirit Menu");
            }
        }
    }

    private void openCanvas()
    {

    }
}
