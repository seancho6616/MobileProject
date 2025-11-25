using UnityEngine;
using UnityEngine.UI;

// 캐릭터 충돌 관련 코드
public class PlayerCollider : MonoBehaviour 
{
    PlayerStats playerStats;
    PlayerControl playerControl;
    AttackImageChanger attackImageChanger;
    TextUI textUI;
    // PlayerStemina playerStemina; 
    // HeartManager heartManager;
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        playerControl = GetComponent<PlayerControl>();
        attackImageChanger = FindAnyObjectByType<AttackImageChanger>();
        textUI = FindAnyObjectByType<TextUI>();
        // playerStemina = GetComponent<PlayerStemina>();
        // heartManager = GetComponent<HeartManager>();
    }
    private void OnTriggerEnter(Collider other) {
        
        if(other.CompareTag("Stamina"))
        {
            attackImageChanger.ChangeSprite(); // 이미지 변경
            playerControl.pickupStamina = true;
            playerControl.Item = other.gameObject;
            // if(playerStemina.MaxStamina > playerStemina.currentStamina)
            // {
            //     playerStemina.currentStamina += 25f;
            //     playerStemina.MaxStamina = 25f;    
            //     playerStemina.UpdateStamina();
            // }
            // Destroy(other.gameObject);
        }
        if(other.CompareTag("Heart"))
        {
            attackImageChanger.ChangeSprite();
            playerControl.pickupHeart = true;
            playerControl.Item = other.gameObject;
            // heartManager.MaxHealth =4f;
            // heartManager.MakeSameHeart();
            // Destroy(other.gameObject);
        }
        if(other.CompareTag("Potion"))
        {
            attackImageChanger.ChangeSprite();
            playerControl.pickupPotion = true;
            playerControl.Item = other.gameObject;

        }
        if(other.CompareTag("Coin"))
        {
            playerStats.CoinCount += 1;
            int coin = playerStats.CoinCount;
            textUI.CountCoin(coin);
            Destroy(other.gameObject);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            attackImageChanger.BeforeChangeSprite();
            playerControl.MakeFalse();
        }
    }
}
