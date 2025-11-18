using UnityEngine;
using UnityEngine.UI;

public class AttackImageChanger : MonoBehaviour
{
    [Header("Change Image")]
    public Image targetIm; //변경할 버튼
    public Sprite newImage; //변경할 이미지
    Sprite originIm; //기존 이미지
    void Awake()
    {
        if(targetIm != null)
        {
            originIm = targetIm.sprite;
        }
    }

    public void ChangeSprite() //이미지 변경
    {
        targetIm.sprite = newImage;
    }
    public void BeforeChangeSprite() //기존 이미지로 변경
    {
        targetIm.sprite = originIm;
    }
    
}
