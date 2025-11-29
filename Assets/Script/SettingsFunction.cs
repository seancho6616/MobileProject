using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Dropdown 사용을 위해 필요

public class SettingsFunction : MonoBehaviour
{
    [Header("--- [2] Sound & Controller ---")]
    public Slider volumeSlider;
    public TMP_Dropdown sizeDropdown;
    
    // 크기를 조절할 컨트롤러 UI들 (조이스틱, 공격버튼 등)
    public RectTransform[] controllerUIElements; 

    [Header("--- [4] System Settings ---")]
    public Button saveButton;
    public Button titleButton;
    public Button quitButton;

    void Start()
    {
        // 1. 소리 설정 초기화
        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume; // 현재 볼륨 가져오기
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }

        // 2. 컨트롤러 크기 설정 초기화
        if (sizeDropdown != null)
        {
            sizeDropdown.onValueChanged.AddListener(OnSizeChanged);
        }

        // 3. 버튼 기능 연결
        if (saveButton != null) saveButton.onClick.AddListener(OnClickSave);
        if (titleButton != null) titleButton.onClick.AddListener(OnClickTitle);
        if (quitButton != null) quitButton.onClick.AddListener(OnClickQuit);
    }

    // --- 기능 구현 ---

    // 볼륨 조절
    void OnVolumeChanged(float value)
    {
        AudioListener.volume = value; // 전체 소리 크기 조절 (0.0 ~ 1.0)
    }

    // 컨트롤러 크기 조절 (Small, Normal, Large)
    void OnSizeChanged(int index)
    {
        float scale = 1.0f;
        switch (index)
        {
            case 0: scale = 0.8f; break; // 작게
            case 1: scale = 1.0f; break; // 보통
            case 2: scale = 1.3f; break; // 크게
        }

        // 연결된 모든 컨트롤러 UI의 크기를 바꿈
        foreach (RectTransform rect in controllerUIElements)
        {
            if (rect != null)
                rect.localScale = Vector3.one * scale;
        }
    }

    // 저장
    void OnClickSave()
    {
        if (Manager.Instance != null)
        {
            Debug.Log("[설정] 게임 저장 시도");
            Manager.Instance.SaveGame();
        }
    }

    // 타이틀 화면으로
    void OnClickTitle()
    {
        Time.timeScale = 1f; // 멈춘 시간 다시 흐르게 하기
        SceneManager.LoadScene("StartScene"); // 씬 이름 확인 필수!
    }

    // 게임 종료
    void OnClickQuit()
    {
        Debug.Log("게임 종료");
        Application.Quit(); // 빌드된 게임에서만 작동함

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터에서도 종료되게 함
        #endif
    }
}