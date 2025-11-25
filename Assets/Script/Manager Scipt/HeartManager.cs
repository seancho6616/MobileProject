using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    PlayerStats playerStats;
    
    float maxPositionHealth = 9f;

    [Header("UI Setting")]
    [SerializeField] HeartUI heartPrefab;
    [SerializeField] Transform parentUI;

    private float heartPerContainer = 4f; // 목숨 한개에 칸수

    private List<HeartUI> healthContainerPool = new List<HeartUI>();
    
    private bool isDataLoaded = false;

    void Awake()
    {
        playerStats = FindAnyObjectByType<PlayerStats>();
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

        int requiredContainers = Mathf.CeilToInt((float)playerStats.MaxHealth / heartPerContainer);
        float healthToFill = playerStats.CurrentHealth;

        for (int i = 0; i < healthContainerPool.Count; i++)
        {
            HeartUI container = healthContainerPool[i];
            
            if (i < requiredContainers)
            {
                container.SetActive(true);
                float fillValue = Mathf.Clamp(healthToFill, 0, heartPerContainer);
                
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
        playerStats.CurrentHealth = playerStats.MaxHealth;
    }

    // Manager.cs에서 호출하는 함수
    public void SetHealthDirectly(int current, int max)
    {
        playerStats.CurrentHealth = (float)current; 
        playerStats.MaxHealth = (float)max;
        
        isDataLoaded = true; 
        
        UpdateHealth();
    }
}