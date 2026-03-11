using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerMovement : MonoBehaviour
{
    public bool canDash;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDash(InputAction.CallbackContext context)
    {
        if (context.interaction is TapInteraction || context.interaction is PressInteraction)
        {
            if (context.performed)
            {
                Debug.Log("Start Dashing");
            }
        }
    }
}
