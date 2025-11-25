using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Player Stats")]
    public int coins = 0;
    public int potionCount = 0;
    
    public int maxStamina = 100;
    public int currentStamina = 100;
    
    public int speed = 10;
    public int baseAttack = 0;
    public int baseAttackSpeed = 1;
    public int attackRange = 10;
    
    public int equippedWeaponId = 0;


    public void GetCoin(int amount)
    {
        coins += amount;

        if (Manager.Instance != null && Manager.Instance.textUI != null)
        {
            Manager.Instance.textUI.CountCoin(coins);
        }
    }

    public void GetPotion(int amount)
    {
        potionCount += amount;

        // UI 갱신
        if (Manager.Instance != null && Manager.Instance.textUI != null)
        {
            Manager.Instance.textUI.CountPotion(potionCount);
        }
    }

    void Update()
    {
        // 스태미너 회복 로직 등은 나중에 여기에 추가
    }
}