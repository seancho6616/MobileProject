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

    void Update()
    {
        // 회복 로직 어케 넣지?
    }
}