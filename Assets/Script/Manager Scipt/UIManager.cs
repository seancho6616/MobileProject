using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("canvas")]
    [SerializeField] CanvasGroup canvasSetting;
    [SerializeField] CanvasGroup canvasPlayer;
    [SerializeField] CanvasGroup canvasInGame;

    public void OnClickShowSetting()
    {
        HidCanvas(canvasPlayer);
        HidCanvas(canvasInGame);
        ShowCanvas(canvasSetting);
    }
    public void OnClickHitdeetting()
    {
        HidCanvas(canvasSetting);
        ShowCanvas(canvasInGame);
        ShowCanvas(canvasPlayer);
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
