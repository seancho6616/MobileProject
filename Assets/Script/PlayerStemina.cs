using UnityEngine;
using UnityEngine.UI;


public class PlayerStemina : MonoBehaviour
{

    [Header("Stamina Image")]
    [SerializeField] Slider staminaIm;

    PlayerStats playerStats;
    
    float staminaRegen = 8f;
    float regenTime =1.5f;
    float lateUseStamina;


    void Awake()
    {
        // PlayerStats 컴포넌트 찾아오기
        playerStats = GetComponent<PlayerStats>();
    
    }


    void Start()
    {
        // palyerStats에 있는 데이터 불러옴
        UpdateStamina();
    }
    void Update()
    {
        RegenStamina();
    }

    private void RegenStamina() // 스테미너 리젠
    {
        if (playerStats.CurrentStamina < playerStats.MaxStamina && Time.time >= lateUseStamina + regenTime)
        {
            // 스태미너 회복
            playerStats.CurrentStamina += staminaRegen * Time.deltaTime;
            
            // max 안넘게 고정
            if (playerStats.CurrentStamina > playerStats.MaxStamina)
            {
                playerStats.CurrentStamina = playerStats.MaxStamina;
            }

            UpdateStamina();
        }
    }

    public bool UseStamina(float stamina) // 스테미너 사용
    {
        if(playerStats.CurrentStamina >= stamina)
        {
            playerStats.CurrentStamina -= stamina;
            lateUseStamina = Time.time;
            
            UpdateStamina();
            return true;
        }
        return false;
    } 

    public void UpdateStamina() // 현재 스테미나 값 시각화 ui
    {
        if (staminaIm != null && playerStats != null)
        {
            // 0으로 나누기 방지
            if (playerStats.MaxStamina > 0)
            {
                staminaIm.value = playerStats.CurrentStamina / playerStats.MaxStamina;
            }
        }
    }
}
