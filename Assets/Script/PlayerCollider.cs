using UnityEngine;
using UnityEngine.UI;

// 캐릭터 충돌 관련 코드
public class PlayerCollider : MonoBehaviour 
{
    PlayerStemina playerStemina; 
    HeartManager heartManager;
    AttackImageChanger attackImageChanger;
    void Start()
    {
        playerStemina = GetComponent<PlayerStemina>();
        heartManager = GetComponent<HeartManager>();
        attackImageChanger = FindAnyObjectByType<AttackImageChanger>();
    }
    private void OnTriggerEnter(Collider other) {
        
        if(other.gameObject.tag == "Stamina")
        {
            attackImageChanger.ChangeSprite(); // 이미지 변경
            // if(playerStemina.MaxStamina > playerStemina.currentStamina)
            // {
            //     playerStemina.currentStamina += 25f;
            //     playerStemina.MaxStamina = 25f;    
            //     playerStemina.UpdateStamina();
            // }
            // Destroy(other.gameObject);
        }
        if(other.gameObject.tag == "Heart")
        {
            attackImageChanger.ChangeSprite();
            Debug.Log("Heart");
            // heartManager.MaxHealth =4f;
            // heartManager.MakeSameHeart();
            // Destroy(other.gameObject);
        }
        if(other.gameObject.tag == "Potion")
        {
            attackImageChanger.ChangeSprite();
        }
        if(other.gameObject.tag == "Coin")
        {
            
        }
    }
    void OnTriggerExit(Collider other)
    {
        attackImageChanger.BeforeChangeSprite();
    }
}
