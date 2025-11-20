
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour // 목숨 관련 코드
{
    [Header("Heart")]
    [SerializeField] float maxHealth = 16f; // 현재 최대 목숨
    public float MaxHealth // 이 변수를 활용해서 maxHealth 값을 수정 및 활용
    {
        get => maxHealth;
        set => maxHealth += value;
    }
    [SerializeField] float currentHealth; // 현재 목숨
    public float CurrentHealth // 이 변수를 활용해서 currentHealth 값을 수정 및 활용
    {
        get => currentHealth;
        set => currentHealth += value;
    }
    float maxPositionHearlth = 9f; //게임에서 가질 수 있는 최대 목숨 개수

    [Header("UI Setting")]
    [SerializeField] HeartUI heartPrefab;
    [SerializeField] Transform parentUI;

    private float heartPerContainer = 4f; //목숨 한개에 칸수

    private List<HeartUI> healthContainerPool = new List<HeartUI>();

    void Start()
    {
        MakeSameHeart();
        for (int i = 0; i < maxPositionHearlth; i++)
        {
            HeartUI container = Instantiate(heartPrefab, parentUI);
            container.SetActive(false); // 4. 스크립트의 함수로 끄기
            healthContainerPool.Add(container); // 풀에 추가
        }
        UpdateHealth();
    }
    void Update()
        {
            UpdateHealth();
        }
    
    void UpdateHealth() // 현재 목숨 개수의 맞게 ui 설정
    {
        int requiredContainers = Mathf.CeilToInt((float)maxHealth / heartPerContainer);
        float healthToFill = currentHealth;

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
        currentHealth = maxHealth;
    }

    public void SetHealthDirectly(int current, int max)
    {
        currentHealth = (float)current; 
        maxHealth = (float)max;
        UpdateHealth();
    }
}
