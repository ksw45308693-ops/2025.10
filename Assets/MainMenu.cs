using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Buttons")]
    public Button startButton;
    public Button tutorialButton;
    public Button exitButton;

    [Header("Tutorial Panel")]
    public GameObject tutorialPanel; // 튜토리얼 선택 UI

    [Header("Scene Names")]
    public string gameSceneName = "GameScene";
    public string tutorialSceneName = "TutorialScene";

    private void Awake()
    {
        if (startButton == null) startButton = GameObject.Find("StartButton")?.GetComponent<Button>();
        if (tutorialButton == null) tutorialButton = GameObject.Find("TutorialButton")?.GetComponent<Button>();
        if (exitButton == null) exitButton = GameObject.Find("ExitButton")?.GetComponent<Button>();
        if (tutorialPanel == null) tutorialPanel = GameObject.Find("TutorialPanel");

        if (startButton != null)
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(OnStartClicked);
        }
        if (tutorialButton != null)
        {
            tutorialButton.onClick.RemoveAllListeners();
            tutorialButton.onClick.AddListener(OnTutorialClicked);
        }
        if (exitButton != null)
        {
            exitButton.onClick.RemoveAllListeners();
            exitButton.onClick.AddListener(OnExitClicked);
        }

        if (tutorialPanel != null) tutorialPanel.SetActive(false);
    }

    public void OnStartClicked()
    {
        if (!string.IsNullOrEmpty(gameSceneName))
            SceneManager.LoadScene(gameSceneName);
        else
            Debug.LogWarning("Game scene name is empty. Set a scene name in the inspector.");
    }

    public void OnTutorialClicked()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
        }
        else if (!string.IsNullOrEmpty(tutorialSceneName))
        {
            SceneManager.LoadScene(tutorialSceneName);
        }
        else
        {
            Debug.LogWarning("튜토리얼 패널이 없고 튜토리얼 씬 이름도 비어있습니다.");
        }
    }

    public void CloseTutorialPanel()
    {
        if (tutorialPanel != null) tutorialPanel.SetActive(false);
    }

    // UI 버튼에서 문자열 인수로 호출 가능 (예: "TutorialScene1")
    public void SelectTutorial(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
            SceneManager.LoadScene(sceneName);
        else
            Debug.LogWarning("선택한 튜토리얼의 씬 이름이 비어있습니다.");
    }

    public void OnExitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}