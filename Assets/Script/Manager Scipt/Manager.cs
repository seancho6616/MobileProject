using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public static Manager Instance; 

    [Header("Managers")]
    public PlayerManager playerManager; // 스탯 관리
    public HeartManager heartManager;   // 체력 관리
    PlayerStats playerStats;            // 데이터 관리
    public GameObject player;           // 플레이어 위치
    
    [Header("UI")]
    public TextUI textUI; // UI 표시

    void Awake()
    {
        if (Instance == null) Instance = this;
        if (playerStats == null && player != null)
            playerStats = player.GetComponent<PlayerStats>();
    }

    void Start()
    {

        if (GameDataStore.Instance != null && GameDataStore.Instance.isContinue)
        {
            if (GameDataStore.Instance.cachedData != null)
            {
                Debug.Log("이어하기 데이터 가져옴. 게임에 적용");
                ApplyGameData(GameDataStore.Instance.cachedData);
            }
            // 데이터 사용 후 초기화
            GameDataStore.Instance.isContinue = false;
            GameDataStore.Instance.cachedData = null;
        }
        else
        {
            Debug.Log("새 게임입니다. (초기 상태로 start)");
        }
    }

    // 저장
    public void SaveGame()
    {
        if (player == null || playerManager == null) return;

        GameData data = new GameData();

        // 1. 위치 정보 저장
        data.position = new PositionData(
            player.transform.position.x,
            player.transform.position.y,
            player.transform.position.z
        );
        data.lastScene = SceneManager.GetActiveScene().name;

        // 2. 체력 정보 저장
        data.maxHeart = (int)playerStats.MaxHealth;
        data.currentHeart = (int)playerStats.CurrentHealth;

        data.maxStamina = (int)playerStats.MaxStamina;
        data.currentStamina = (int)playerStats.CurrentStamina;

        data.coins = playerStats.CoinCount;
        data.potionCount = playerStats.PotionCount;

        // 3. 스탯 정보 저장
        if (playerManager != null)
        {
            data.speed = playerManager.speed;
            data.baseAttack = playerManager.baseAttack;
            data.baseAttackSpeed = playerManager.baseAttackSpeed;
            data.attackRange = playerManager.attackRange;
            data.equippedWeaponId = playerManager.equippedWeaponId;
        }

        // 데이터를 서버로 전송함
        StartCoroutine(NetworkManager.Instance.SaveGameData(data));
    }

    // 적용 (불러오기)
    public void ApplyGameData(GameData loadedData)
    {
        // 1. 아이템, 체력, 스테미너 적용
        if (playerStats != null)
        {

            playerStats.MaxHealth = loadedData.maxHeart;
            playerStats.CurrentHealth = loadedData.currentHeart;
            
            playerStats.MaxStamina = loadedData.maxStamina;
            playerStats.CurrentStamina = loadedData.currentStamina;
            
            playerStats.CoinCount = loadedData.coins;
            playerStats.PotionCount = loadedData.potionCount;
            
            // UI 업데이트
            if (textUI != null)
            {
                textUI.CountCoin(playerStats.CoinCount);      
                textUI.CountPotion(playerStats.PotionCount); 
            }
        }

        // 2. 전투 스탯 적용
        if (playerManager != null)
        {
            playerManager.speed = loadedData.speed;
            playerManager.baseAttack = loadedData.baseAttack;
            playerManager.baseAttackSpeed = loadedData.baseAttackSpeed;
            playerManager.attackRange = loadedData.attackRange;
            playerManager.equippedWeaponId = loadedData.equippedWeaponId;
        }

        // 3. 위치 적용
        if (player != null && loadedData.position != null)
        {
            player.transform.position = new Vector3(
                loadedData.position.x,
                loadedData.position.y,
                loadedData.position.z
            );
        }
    }


}