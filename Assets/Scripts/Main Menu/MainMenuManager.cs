using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject settingsPanel;

    private void Start()
    {
        // Ensure settings panel is closed on menu load
        settingsPanel.SetActive(false);
    }

    // --- Button Callbacks ---

    public void OnPlayClicked()
    {
        // Replace "GameScene" with your actual scene name
        SceneManager.LoadScene("GameplayScene");
    }

    public void OnQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnSettingsClicked()
    {
        settingsPanel.SetActive(true);
    }

    public void OnSettingsCloseClicked()
    {
        settingsPanel.SetActive(false);
    }
}