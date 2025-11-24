using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    [Header("Heart")]
    [SerializeField] float maxHealth = 16f; // 현재 최대 목숨
    public float MaxHealth
    {
        get => maxHealth;
        set => maxHealth += value;
    }
    
    [SerializeField] float currentHealth; // 현재 목숨
    public float CurrentHealth
    {
        get => currentHealth;
        set => currentHealth += value;
    }
    
    float maxPositionHealth = 9f;

    [Header("UI Setting")]
    [SerializeField] HeartUI heartPrefab;
    [SerializeField] Transform parentUI;

    private float heartPerContainer = 4f; // 목숨 한개에 칸수

    private List<HeartUI> healthContainerPool = new List<HeartUI>();
    
    private bool isDataLoaded = false;

    void Awake()
    {
        for (int i = 0; i < maxPositionHealth; i++)
        {
            if (heartPrefab != null && parentUI != null)
            {
                HeartUI container = Instantiate(heartPrefab, parentUI);
                container.SetActive(false); 
                healthContainerPool.Add(container);
            }
        }
    }

    void Start()
    {
        // 데이터를 서버에서 불러오면 초기화 X
        if (!isDataLoaded)
        {
            MakeSameHeart();
        }
        
        UpdateHealth();
    }
    
    void Update()
    {
        UpdateHealth();
    }
    
    void UpdateHealth() // 현재 목숨 개수의 맞게 ui 설정
    {
        if (healthContainerPool.Count == 0) return;

        int requiredContainers = Mathf.CeilToInt((float)maxHealth / heartPerContainer);
        float healthToFill = currentHealth;

        for (int i = 0; i < healthContainerPool.Count; i++)
        {
            HeartUI container = healthContainerPool[i];
            
            if (i < requiredContainers)
            {
                container.SetActive(true);
                float fillValue = Mathf.Clamp(healthToFill, 0, heartPerContainer);
                
                // HeartUI에 SetFill 함수가 있다고 가정
                container.SetFill(fillValue / heartPerContainer);
                
                healthToFill -= fillValue;
            }
            else
            {
                container.SetActive(false);
            }
        }
    }

    public void MakeSameHeart() // 현재 목숨 초기 설정
    {
        currentHealth = maxHealth;
    }

    // Manager.cs에서 호출하는 함수
    public void SetHealthDirectly(int current, int max)
    {
        currentHealth = (float)current; 
        maxHealth = (float)max;
        
        isDataLoaded = true; 
        
        UpdateHealth();
    }
}