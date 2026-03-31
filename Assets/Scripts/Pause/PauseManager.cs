using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;

    public bool isPaused;
    public GameObject pausedCanvas;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        pausedCanvas.SetActive(false);
        isPaused = false;
    }

    public void OnPress(InputAction.CallbackContext context)
    {
        if (context.interaction is TapInteraction)
        {
            if (context.performed)
            {
                interactCanvas();
            }
        }
    }

    private void interactCanvas()
    {
        pausedCanvas.SetActive(!pausedCanvas.activeSelf);
        Debug.Log($"Inventory : {pausedCanvas.activeSelf}");

        isPaused = !isPaused;
        Debug.Log($"Paused : {isPaused}");
    }
}

