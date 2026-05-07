using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;

    public bool isPaused;
    [Header("Panels")]
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
        Debug.Log($"Pause Canvas : {pausedCanvas.activeSelf}");

        isPaused = !isPaused;
        Debug.Log($"Paused : {isPaused}");
    }

    // -------------------------------------------------------

    public void OnContinueClicked()
    {
        pausedCanvas.SetActive(false);
        isPaused = false;
    }

    public void OnExitToMainMenuClicked()
    {
        isPaused = false;
        //Replace name nnti
        SceneManager.LoadScene("MainMenu");
    }

    public void OnExitGameClicked()
    {
        isPaused = false;
        Application.Quit();
    }
}