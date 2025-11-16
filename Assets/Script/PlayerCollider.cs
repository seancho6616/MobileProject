using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    PlayerStemina playerStemina;
    void Start()
    {
        playerStemina = GetComponent<PlayerStemina>();
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Stamina")
        {
            Debug.Log("Hi Stamina");
            if(playerStemina.MaxStamina > playerStemina.currentStamina)
            {
                playerStemina.currentStamina += 25f;
                playerStemina.UpdateStamina();
                Debug.Log("11111");
            }
            
            Destroy(other.gameObject);
        }
    }
}
