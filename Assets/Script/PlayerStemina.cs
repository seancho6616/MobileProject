using UnityEngine;
using UnityEngine.UI;


public class PlayerStemina : MonoBehaviour
{

    [Header("Stamina Image")]
    [SerializeField] Slider staminaIm;
    
    float maxStamina =100f;
    public float MaxStamina
    {
        get => maxStamina;
        set => maxStamina += value;
    }
    public float currentStamina;
    float staminaRegen = 8f;
    float regenTime =1.5f;
    float lateUseStamina;

    

    void Start()
    {
        currentStamina = maxStamina;
        UpdateStamina();
    }
    void Update()
    {
        RegenStamina();
    }

    private void RegenStamina()
    {
        if (currentStamina < maxStamina && Time.time >= lateUseStamina + regenTime)
        {
            currentStamina += staminaRegen * Time.deltaTime; // 회복
            currentStamina = Mathf.Min(currentStamina, maxStamina); // 최대치를 넘지 않게 함
            UpdateStamina();
        }
    }

    public bool UseStamina(float stamina)
    {
        if(currentStamina >= stamina)
        {
            currentStamina -= stamina;
            lateUseStamina = Time.time;
            Debug.Log(lateUseStamina);
            UpdateStamina();
            return true;
        }
        return false;
    } 

    public void UpdateStamina()
    {
        if (staminaIm != null)
        {
            staminaIm.value = currentStamina / maxStamina;
        }
    }
}
