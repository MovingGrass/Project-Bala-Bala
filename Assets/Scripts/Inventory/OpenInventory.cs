using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class OpenInventory : MonoBehaviour
{
    public static OpenInventory instance;

    public bool isPaused;
    public GameObject inventoryCanvas;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        inventoryCanvas.SetActive(false);
        isPaused = false;
    }

    public void OnPress(InputAction.CallbackContext context)
    {
        if (context.interaction is TapInteraction)
        {
            if (context.performed)
            {
                if (PauseManager.instance.isPaused) return;

                if(inventoryCanvas != null)
                {
                    interactCanvas();
                }
            }
        }
    }

    private void interactCanvas()
    {
        inventoryCanvas.SetActive(!inventoryCanvas.activeSelf);
        Debug.Log($"Inventory : {inventoryCanvas.activeSelf}");

        isPaused = !isPaused;
        Debug.Log($"Paused : {isPaused}");
    }
}
