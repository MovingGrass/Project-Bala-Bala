using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;

    public bool isPaused;
    public GameObject pausedCanvas;

    [Header("Panels")]
    [SerializeField] private GameObject settingsPanel;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        pausedCanvas.SetActive(false);
        isPaused = false;

        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void OnPress(InputAction.CallbackContext context)
    {
        if (context.interaction is TapInteraction)
        {
            if (context.performed)
            {
                // Only allow toggling pause if settings panel is NOT open
                if (settingsPanel != null && settingsPanel.activeSelf)
                    return;

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

    public void OnOpenSettingsClicked()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    public void OnCloseSettingsClicked()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void OnExitToMainMenuClicked()
    {
        isPaused = false;
        // Replace "MainMenu" with your actual main menu scene name
        SceneManager.LoadScene("MainMenu");
    }
}