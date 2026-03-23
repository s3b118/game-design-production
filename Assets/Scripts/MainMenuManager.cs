using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] CanvasGroup _mainMenuButtonsCG;
    [SerializeField] CanvasGroup _quitConfirmationCG;
    [SerializeField] CanvasGroup _settingsMenuCG;
    CanvasGroup _mainMenuCG;

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit!");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    void CanvasGroupSetState(CanvasGroup canvasGroup, bool state)
    {
        canvasGroup.alpha = state ? 1.0f : 0.0f;
        canvasGroup.interactable = state;
        canvasGroup.blocksRaycasts = state;
    }

    public void OpenQuitConfirmation()
    {
        CanvasGroupSetState(_mainMenuButtonsCG, false);
        CanvasGroupSetState(_quitConfirmationCG, true);
    }

    public void CloseQuitConfirmation()
    {
        CanvasGroupSetState(_quitConfirmationCG, false);
        CanvasGroupSetState(_mainMenuButtonsCG, true);
    }

    void Awake()
    {
        _mainMenuCG = GetComponent<CanvasGroup>();
        OpenMainMenu();
    }

    public void OpenMainMenu()
    {
        CanvasGroupSetState(_mainMenuCG, true);
    }

    public void CloseMainMenu()
    {
        CanvasGroupSetState(_mainMenuCG, false);
    }

    public void Play()
    {
        CloseMainMenu();
        GameManager.Instance.StartGame();
    }

    public void OpenSettingsMenu()
    {
        CanvasGroupSetState(_mainMenuButtonsCG, false);
        CanvasGroupSetState(_settingsMenuCG, true);
    }

    public void CloseSettingsMenu()
    {
        CanvasGroupSetState(_settingsMenuCG, false);
        CanvasGroupSetState(_mainMenuButtonsCG, true);
    }
}
