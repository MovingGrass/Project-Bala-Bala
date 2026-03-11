using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerParry : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Parry(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Parry");
        }
    }
}
