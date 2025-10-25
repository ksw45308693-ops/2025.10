#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.UI;

public static class CreateMainMenuScene
{
    [MenuItem("Tools/Create Main Menu Scene")]
    public static void CreateScene()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        var canvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        var canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var cs = canvasGO.GetComponent<CanvasScaler>();
        cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        cs.referenceResolution = new Vector2(1920, 1080);

        if (GameObject.FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            new GameObject("EventSystem", typeof(UnityEngine.EventSystems.EventSystem), typeof(UnityEngine.EventSystems.StandaloneInputModule));
        }

        Font GetEditorFont()
        {
            var f = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            if (f != null) return f;
            try
            {
                f = Font.CreateDynamicFontFromOSFont("Arial", 14);
            }
            catch
            {
                f = null;
            }
            return f;
        }

        var defaultFont = GetEditorFont();

        Button CreateButton(string name, string label, Vector2 anchoredPos)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
            go.transform.SetParent(canvasGO.transform, false);
            var rt = go.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(400, 100);
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = anchoredPos;

            var img = go.GetComponent<Image>();
            img.color = Color.white;

            var btn = go.GetComponent<Button>();

            var textGO = new GameObject("Text", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            textGO.transform.SetParent(go.transform, false);
            var trt = textGO.GetComponent<RectTransform>();
            trt.anchorMin = new Vector2(0, 0);
            trt.anchorMax = new Vector2(1, 1);
            trt.offsetMin = trt.offsetMax = Vector2.zero;

            var txt = textGO.GetComponent<Text>();
            txt.text = label;
            txt.alignment = TextAnchor.MiddleCenter;
            if (defaultFont != null) txt.font = defaultFont;
            txt.color = Color.black;
            txt.raycastTarget = false;

            return btn;
        }

        float spacing = 120f;
        var startBtn = CreateButton("StartButton", "게임 시작", new Vector2(0, spacing));
        var tutorialBtn = CreateButton("TutorialButton", "튜토리얼 선택", new Vector2(0, 0));
        var exitBtn = CreateButton("ExitButton", "종료", new Vector2(0, -spacing));

        var panelGO = new GameObject("TutorialPanel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        panelGO.transform.SetParent(canvasGO.transform, false);
        var prt = panelGO.GetComponent<RectTransform>();
        prt.sizeDelta = new Vector2(800, 600);
        prt.anchorMin = prt.anchorMax = new Vector2(0.5f, 0.5f);
        prt.anchoredPosition = new Vector2(0, 0);
        var panelImg = panelGO.GetComponent<Image>();
        panelImg.color = new Color(0.95f, 0.95f, 0.95f, 0.95f);
        panelGO.SetActive(false);

        Button CreatePanelButton(string name, string label, Vector2 pos)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
            go.transform.SetParent(panelGO.transform, false);
            var rt = go.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(300, 80);
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = pos;

            var txtGO = new GameObject("Text", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            txtGO.transform.SetParent(go.transform, false);
            var trt = txtGO.GetComponent<RectTransform>();
            trt.anchorMin = new Vector2(0, 0);
            trt.anchorMax = new Vector2(1, 1);
            trt.offsetMin = trt.offsetMax = Vector2.zero;

            var txt = txtGO.GetComponent<Text>();
            txt.text = label;
            txt.alignment = TextAnchor.MiddleCenter;
            if (defaultFont != null) txt.font = defaultFont;
            txt.color = Color.black;
            txt.raycastTarget = false;

            return go.GetComponent<Button>();
        }

        CreatePanelButton("Tutorial1Button", "튜토리얼 1", new Vector2(0, 100));
        CreatePanelButton("Tutorial2Button", "튜토리얼 2", new Vector2(0, -20));

        var controllerGO = new GameObject("MainMenuController");
        var mainMenu = controllerGO.AddComponent<MainMenu>();
        mainMenu.startButton = startBtn;
        mainMenu.tutorialButton = tutorialBtn;
        mainMenu.exitButton = exitBtn;
        mainMenu.tutorialPanel = panelGO;
        mainMenu.gameSceneName = "GameScene";
        mainMenu.tutorialSceneName = "TutorialScene";

        // 닫기 버튼 (튜토리얼 패널 우측 상단)
        var closeBtnGO = new GameObject("CloseButton", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        closeBtnGO.transform.SetParent(panelGO.transform, false);
        var closeRt = closeBtnGO.GetComponent<RectTransform>();
        closeRt.sizeDelta = new Vector2(60, 60);
        closeRt.anchorMin = new Vector2(1, 1);
        closeRt.anchorMax = new Vector2(1, 1);
        closeRt.pivot = new Vector2(1, 1);
        closeRt.anchoredPosition = new Vector2(-10, -10);

        var closeImg = closeBtnGO.GetComponent<Image>();
        closeImg.color = new Color(0.9f, 0.4f, 0.4f, 1f);

        var closeTextGO = new GameObject("Text", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
        closeTextGO.transform.SetParent(closeBtnGO.transform, false);
        var closeTrt = closeTextGO.GetComponent<RectTransform>();
        closeTrt.anchorMin = new Vector2(0, 0);
        closeTrt.anchorMax = new Vector2(1, 1);
        closeTrt.offsetMin = closeTrt.offsetMax = Vector2.zero;

        var closeTxt = closeTextGO.GetComponent<Text>();
        closeTxt.text = "닫기";
        closeTxt.alignment = TextAnchor.MiddleCenter;
        if (defaultFont != null) closeTxt.font = defaultFont;
        closeTxt.color = Color.white;
        closeTxt.raycastTarget = false;

        // 버튼 이벤트를 MainMenu.CloseTutorialPanel에 연결 (씬에 영구 연결)
        var closeButton = closeBtnGO.GetComponent<Button>();
        UnityEventTools.AddPersistentListener(closeButton.onClick, mainMenu.CloseTutorialPanel);

        EditorSceneManager.MarkSceneDirty(scene);

        string scenePath = "Assets/MainMenu.unity";
        EditorSceneManager.SaveScene(scene, scenePath);
        Debug.Log("MainMenu 씬 생성 및 저장됨: " + scenePath);
    }
}
#endif