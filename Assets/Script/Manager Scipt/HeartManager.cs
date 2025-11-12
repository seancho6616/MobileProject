
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [Header("Heart")]
    [SerializeField] float maxHealth = 16f; // 현재 최대 목숨
    [SerializeField] float currentHearlth = 16f; // 현재 목숨
    float maxPositionHearlth = 9f; //게임에서 가질 수 있는 최대 목숨 개수

    [Header("UI Setting")]
    [SerializeField] HeartUI heartPrefab;
    [SerializeField] Transform parentUI;

    private float heartPerContainer = 4f;

    private List<HeartUI> healthContainerPool = new List<HeartUI>();

    void Start()
    {
        for (int i = 0; i < maxPositionHearlth; i++)
        {
            HeartUI container = Instantiate(heartPrefab, parentUI);
            container.SetActive(false); // 4. 스크립트의 함수로 끄기
            healthContainerPool.Add(container); // 풀에 추가
        }
        //currentHearlth = maxHealth;
        UpdateHealth();
    }

    void UpdateHealth()
    {
        int requiredContainers = Mathf.CeilToInt((float)maxHealth / heartPerContainer);
        float healthToFill = currentHearlth;

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

    void Update()
    {
        UpdateHealth();
    }
}
