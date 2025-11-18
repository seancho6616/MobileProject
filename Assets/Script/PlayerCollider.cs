using UnityEngine;


// 캐릭터 충돌 관련 코드
public class PlayerCollider : MonoBehaviour 
{
    PlayerStemina playerStemina; 
    HeartManager heartManager;
    void Start()
    {
        playerStemina = GetComponent<PlayerStemina>();
        heartManager = GetComponent<HeartManager>();
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Stamina")
        {
            Debug.Log("Hi Stamina");
            if(playerStemina.MaxStamina > playerStemina.currentStamina)
            {
                playerStemina.currentStamina += 25f;
                playerStemina.MaxStamina = 25f;    
                playerStemina.UpdateStamina();
            }
            Destroy(other.gameObject);
        }
        if(other.gameObject.tag == "Heart")
        {
            Debug.Log("Heart");
            heartManager.MaxHealth =4f;
            heartManager.MakeSameHeart();
            Destroy(other.gameObject);
        }
        if(other.gameObject.tag == "Coin")
        {
            
        }
        if(other.gameObject.tag == "Potion")
        {
            
        }
    }
}
