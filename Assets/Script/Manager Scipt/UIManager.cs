 using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("canvas")]
    [SerializeField] CanvasGroup canvasSetting;
    [SerializeField] CanvasGroup canvasPlayer;
    [SerializeField] CanvasGroup canvasInGame;
    [SerializeField] CanvasGroup settingDs;
    [SerializeField] CanvasGroup programmerDs;
    [SerializeField] CanvasGroup soundDs;
    [SerializeField] CanvasGroup keyDs;
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

    void ShowCanvas(CanvasGroup can)
    {
        can.alpha = 1f;
        can.interactable = true;
        can.blocksRaycasts = true;
    }

    void HidCanvas(CanvasGroup can)
    {
        can.alpha = 0f;
        can.interactable = false;
        can.blocksRaycasts = false;
    }
    
}
