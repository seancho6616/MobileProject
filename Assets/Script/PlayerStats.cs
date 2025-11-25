using UnityEngine;

public class PlayerStats: MonoBehaviour  // 플레이어 정보 데이터
{
    [Header("Movement")]
    public float MoveSpeed = 5f; // 이동 속도 관리

    [Header("Heart")]
    [SerializeField] float maxHealth = 16f; 
    public float MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value; 
    }

    [SerializeField] float currentHealth = 16f;
    public float CurrentHealth
    {
        get => currentHealth;
        set => currentHealth = value;
    }

    [Header("Stamina")]
    [SerializeField] float maxStamina = 100f;
    public float MaxStamina
    {
        get => maxStamina;
        set => maxStamina = value;
    }

    [SerializeField] float currentStamina = 100f; 
    public float CurrentStamina
    {
        get => currentStamina;
        set => currentStamina = value; 
    }
    
    void Awake()
    {
        CurrentStamina = MaxStamina; // 현재 = 최대 (100)
    }
    
    [Header("Coin & Potion count")]
    public int CoinCount{get;set;}
    public int PotionCount{get; set;}

    public float attackDamage{get; set;}
    public float attackRange{get; set;}

    public ParticleSystem healParticle;
}