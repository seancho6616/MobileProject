using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour // 목숨 한개의 칸수 조절
{
    [SerializeField] Image fillImage;
    

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

      public void SetFill(float amount)
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = amount;
        }
    }
    

}
