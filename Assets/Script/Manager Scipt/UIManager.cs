 using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("canvas")] //메인 게임 ui 설정들
    [SerializeField] CanvasGroup canvasSetting; //설정들 캔버스
    [SerializeField] CanvasGroup canvasPlayer; // 플레이어 버튼 관련 캔버스
    [SerializeField] CanvasGroup canvasInGame; // 플레이어 정보 ui 캠버스
    
    [Header("Settings UI")]
    [SerializeField] CanvasGroup settingDs; //설정 설명 UI
    [SerializeField] CanvasGroup programmerDs; // 프로그래머 설명 ui
    [SerializeField] CanvasGroup soundDs; // 사운드 조절 ui
    [SerializeField] CanvasGroup keyDs; // 키 설명 ui
    void Start()
    {
        canvasSetting.alpha = 0f;
        canvasPlayer.alpha = 1f;
        canvasInGame.alpha = 1f;
    }

    public void OnClickShowSetting()
    {
        Time.timeScale = 0f;
        HidCanvas(canvasPlayer);
        HidCanvas(canvasInGame);
        ShowCanvas(canvasSetting);
    }
    public void OnClickHideSetting()
    {
        Time.timeScale = 1f;
        HidCanvas(canvasSetting);
        ShowCanvas(canvasInGame);
        ShowCanvas(canvasPlayer);
    }

    public void OnKeyGuide()
    {
        ShowCanvas(keyDs);
        HidCanvas(soundDs);
        HidCanvas(programmerDs);
        HidCanvas(settingDs);
    }
    public void OnSound()
    {
        ShowCanvas(soundDs);
        HidCanvas(keyDs);
        HidCanvas(programmerDs);
        HidCanvas(settingDs);
    }
    public void OnProgrammer()
    {
        ShowCanvas(programmerDs);
        HidCanvas(soundDs);
        HidCanvas(keyDs);
        HidCanvas(settingDs);
    }
    public void OnSetting()
    {
        ShowCanvas(settingDs);
        HidCanvas(soundDs);
        HidCanvas(programmerDs);
        HidCanvas(keyDs);
    }

    void ShowCanvas(CanvasGroup can) // 캔버스 활성화
    {
        can.alpha = 1f;
        can.interactable = true;
        can.blocksRaycasts = true;
    }

    void HidCanvas(CanvasGroup can) // 캔버스 비활성화
    {
        can.alpha = 0f;
        can.interactable = false;
        can.blocksRaycasts = false;
    }
    
    
}
