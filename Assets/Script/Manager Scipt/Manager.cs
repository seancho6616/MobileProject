using UnityEngine;

public class Manager : MonoBehaviour
{
    [Header("Managers")]
    public PlayerManager playerManager; // 스탯
    public HeartManager heartManager;   // 체력
    public GameObject player;           // 위치
    
    [Header("UI")]
    public TextUI textUI;

    void Start()
    {
        LoadGame();
    }

    public void SaveGame()
    {
        GameData data = new GameData();

        // 위치 저장
        data.position = new PositionData(
            player.transform.position.x,
            player.transform.position.y,
            player.transform.position.z
        );
        
        data.lastScene = "MainScene";
        data.maxHeart = (int)heartManager.MaxHealth;
        data.currentHeart = (int)heartManager.CurrentHealth;

        data.coins = playerManager.coins;
        data.potionCount = playerManager.potionCount;
        data.maxStamina = playerManager.maxStamina;
        data.currentStamina = playerManager.currentStamina;
        data.speed = playerManager.speed;
        data.baseAttack = playerManager.baseAttack;
        data.baseAttackSpeed = playerManager.baseAttackSpeed;
        data.attackRange = playerManager.attackRange;
        data.equippedWeaponId = playerManager.equippedWeaponId;

        StartCoroutine(NetworkManager.Instance.SaveGameData(data));
    }

    public void LoadGame()
    {
        StartCoroutine(NetworkManager.Instance.LoadGameData((loadedData) =>
        {
            Debug.Log($"[로드] 코인: {loadedData.coins}");

            // 1. 스탯 적용
            if (playerManager != null)
            {
                playerManager.coins = loadedData.coins;
                playerManager.potionCount = loadedData.potionCount;
                playerManager.maxStamina = loadedData.maxStamina;
                playerManager.currentStamina = loadedData.currentStamina;
                playerManager.speed = loadedData.speed;
                playerManager.baseAttack = loadedData.baseAttack;
                playerManager.baseAttackSpeed = loadedData.baseAttackSpeed;
                playerManager.attackRange = loadedData.attackRange;
                playerManager.equippedWeaponId = loadedData.equippedWeaponId;
                
                if (textUI != null)
                {
                    textUI.CountCoin(playerManager.coins);      // 코인 텍스트 갱신
                    textUI.CountPotion(playerManager.potionCount); // 포션 텍스트 갱신
                    Debug.Log("UI 텍스트 업데이트 완료!");
                }
                else
                {
                    Debug.LogError("TextUI가 연결되지 않았습니다!");
                }
            }

            // 2. 위치 적용
            if (player != null)
            {
                player.transform.position = new Vector3(
                    loadedData.position.x,
                    loadedData.position.y,
                    loadedData.position.z
                );
            }

            // 3. 체력 적용
            if (heartManager != null)
            {
                heartManager.SetHealthDirectly(loadedData.currentHeart, loadedData.maxHeart);
            }
        }));
    }
}