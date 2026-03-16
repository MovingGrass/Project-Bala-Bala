
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGetMousePosition : MonoBehaviour
{
    public Vector2 screenSpace;

    public void OnMouseUpdate(InputAction.CallbackContext context)
    {
        screenSpace = context.ReadValue<Vector2>();
        Debug.Log(screenSpace);
    }
}