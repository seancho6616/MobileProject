using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    PlayerStats playerStats; // 여기서 데이터 불러옴

    [Header("UI Setting")]
    [SerializeField] HeartUI heartPrefab;
    [SerializeField] Transform parentUI;

    private float heartPerContainer = 4f; // 목숨 한개에 칸수
    float maxPositionHealth = 9f; // 최대 하트 칸 수 

    private List<HeartUI> healthContainerPool = new List<HeartUI>();
    

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

    public void MakeSameHeart()
    {
        // UI 새로고침
        UpdateHealth();
    }

    void Start()
    {
        // palyerStats가 데이터를 가지고 있음 -> 데이터에 있는 체력을 불러옴.
        if (playerStats == null)
        {
            playerStats = FindAnyObjectByType<PlayerStats>();
        }
        UpdateHealth();
    }
    
    void Update()
    {
        UpdateHealth();
    }
    
    void UpdateHealth() // 현재 목숨 개수의 맞게 ui 설정
    {
        if (playerStats == null || healthContainerPool.Count == 0) return;

        int requiredContainers = Mathf.CeilToInt(playerStats.MaxHealth / heartPerContainer);
        float healthToFill = playerStats.CurrentHealth;

        for (int i = 0; i < healthContainerPool.Count; i++)
        {
            HeartUI container = healthContainerPool[i];
            
            if (i < requiredContainers)
            {
                container.gameObject.SetActive(true);
                float fillValue = Mathf.Clamp(healthToFill, 0, heartPerContainer);
                container.SetFill(fillValue / heartPerContainer);
                healthToFill -= fillValue;
            }
            else
            {
                container.gameObject.SetActive(false);
            }
        }
    }
}