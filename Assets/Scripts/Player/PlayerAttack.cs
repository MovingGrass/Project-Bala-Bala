using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerAttack : MonoBehaviour
{
    bool _isCharging;

    public void SonicCharge(InputAction.CallbackContext context)
    {
        if (context.interaction is HoldInteraction)
        {
            if (context.performed)
            {
                _isCharging = true;
                Debug.Log("Charging");
            }
            else if (context.canceled && _isCharging)
            {
                _isCharging = false;
                Debug.Log("Sonic Slash");
            }
        }
    }
}
