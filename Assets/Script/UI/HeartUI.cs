using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
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
