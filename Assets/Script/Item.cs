using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { Coin, Potion, Heart } // 아이템 종류
    public ItemType type;
    public int value = 1; // 획득량

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager playerStat = other.GetComponent<PlayerManager>();
            
            if (playerStat != null)
            {
                switch (type)
                {
                    case ItemType.Coin:
                        playerStat.GetCoin(value); // PlayerManager의 코인 증가 함수 호출
                        Debug.Log("코인 획득! 현재 개수: " + playerStat.coins);
                        break;

                    case ItemType.Potion:
                        playerStat.GetPotion(value); // PlayerManager의 포션 증가 함수 호출
                        Debug.Log("포션 획득! 현재 개수: " + playerStat.potionCount);
                        break;
                    
                    // 다른 아이템 추가
                }
            }

            // 아이템 삭제
            Destroy(gameObject);
        }
    }
}