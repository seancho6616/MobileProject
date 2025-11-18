using UnityEngine;

public class PlayerStats // 플레이어 정보 데이터 (아직 활용은 안함)
{
    [Header("Heart")]
    [SerializeField] float maxHealth = 16f; // 현재 최대 목숨
    public float MaxHealth
    {
        get => maxHealth;
        set => maxHealth += value;
    }
    [SerializeField] float currentHealth = 16f; // 현재 목숨
    public float CurrentHealth
    {
        get => currentHealth;
        set => currentHealth += value;
    }

    [Header("Stamina")]
    float maxStamina =100f;
    public float MaxStamina
    {
        get => maxStamina;
        set => maxStamina += value;
    }
    float currentStamina;
    public float CurrentStamina
    {
        get => currentStamina;
        set => currentStamina = value;
    }
    float staminaRegen = 8f;
    float regenTime =1.5f;
}
