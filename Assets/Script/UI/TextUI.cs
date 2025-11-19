using TMPro;
using UnityEngine;

public class TextUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinCountText;
    [SerializeField] TextMeshProUGUI potionCountText;

    public void CountCoin(int count)
    {
        coinCountText.text = count.ToString();
    }

    public void CountPotion(int count)
    {
        potionCountText.text = count.ToString();
    }
}
