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
            case 2: scale = 1.2f; break; // 크게
        }

        // 연결된 모든 컨트롤러 UI의 크기를 바꿈
        foreach (RectTransform rect in controllerUIElements)
        {
            if (rect != null)
                rect.localScale = Vector3.one * scale;
        }
    }

    // 저장 버튼 (수동 저장)
    void OnClickSave()
    {
        if (Manager.Instance != null)
        {
            Debug.Log("[설정] 수동 저장 시도");
            Manager.Instance.SaveGame();
        }
    }

    // 타이틀 화면으로 (자동 저장 기능 포함)
    void OnClickTitle()
    {
        Debug.Log("타이틀 버튼 클릭: 자동 저장 후 이동합니다.");

        // 1. 멈췄던 시간 다시 흐르게 하기 (중요!)
        Time.timeScale = 1f; 

        // 2. 매니저가 있으면 저장 후 이동, 없으면 그냥 이동
        if (Manager.Instance != null)
        {
            // "저장이 완료되면 -> 씬을 이동시켜줘" 라고 부탁(Callback)합니다.
            Manager.Instance.SaveGame(() => 
            {
                Debug.Log("저장 완료. StartScene으로 이동합니다.");
                SceneManager.LoadScene("StartScene"); 
            });
        }
        else
        {
            // 매니저가 없는 경우 (테스트 상황 등) 그냥 이동
            SceneManager.LoadScene("StartScene"); 
        }
    }

    // 게임 종료 (자동 저장 기능 추가)
    void OnClickQuit()
    {
        Debug.Log("종료 버튼 클릭: 자동 저장 후 종료합니다.");

        // 1. 멈춘 시간 다시 흐르게 하기 (안전장치)
        Time.timeScale = 1f;

        // 2. 매니저가 있으면 저장 후 종료
        if (Manager.Instance != null)
        {
            Manager.Instance.SaveGame(() => 
            {
                Debug.Log("저장 완료. 게임을 종료합니다.");
                QuitApplication(); // 저장 끝나면 진짜 종료
            });
        }
        else
        {
            // 매니저 없으면 그냥 종료
            QuitApplication();
        }
    }

    // 실제 종료 기능을 하는 함수 (중복 코드를 줄이기 위해 분리함)
    void QuitApplication()
    {
        Application.Quit(); // 빌드된 게임 종료

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터 종료
        #endif
    }
}