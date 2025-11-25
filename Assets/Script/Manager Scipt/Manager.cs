using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public static Manager Instance; 

    [Header("Managers")]
    public PlayerManager playerManager; // 스탯 관리
    public HeartManager heartManager;   // 체력 관리
    public GameObject player;           // 플레이어 위치
    
    [Header("UI")]
    public TextUI textUI; // UI 표시

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {

        if (GameDataStore.Instance != null && GameDataStore.Instance.isContinue)
        {
            if (GameDataStore.Instance.cachedData != null)
            {
                Debug.Log("이어하기 데이터 도착! 게임에 적용합니다.");
                ApplyGameData(GameDataStore.Instance.cachedData);
            }
            
            GameDataStore.Instance.isContinue = false;
            GameDataStore.Instance.cachedData = null;
        }
        else
        {
            Debug.Log("새 게임입니다. (초기 상태로 시작)");
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
        if (heartManager != null)
        {
            data.maxHeart = (int)heartManager.MaxHealth;
            data.currentHeart = (int)heartManager.CurrentHealth;
        }

        // 3. 스탯 및 아이템 정보 저장
        data.coins = playerManager.coins;
        data.potionCount = playerManager.potionCount;
        data.maxStamina = playerManager.maxStamina;
        data.currentStamina = playerManager.currentStamina;
        data.speed = playerManager.speed;
        data.baseAttack = playerManager.baseAttack;
        data.baseAttackSpeed = playerManager.baseAttackSpeed;
        data.attackRange = playerManager.attackRange;
        data.equippedWeaponId = playerManager.equippedWeaponId;

        // 데이터를 서버로 전송함
        StartCoroutine(NetworkManager.Instance.SaveGameData(data));
    }

    // 적용
    public void ApplyGameData(GameData loadedData)
    {
        // 1. 스탯 및 아이템 적용
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
            
            // UI 업데이트
            if (textUI != null)
            {
                textUI.CountCoin(playerManager.coins);      
                textUI.CountPotion(playerManager.potionCount); 
            }
        }

        // 2. 위치 적용
        if (player != null && loadedData.position != null)
        {
            player.transform.position = new Vector3(
                loadedData.position.x,
                loadedData.position.y,
                loadedData.position.z
            );
        }

        // 3. 체력 적용 (HeartManager의 함수 호출)
        if (heartManager != null)
        {
            heartManager.SetHealthDirectly(loadedData.currentHeart, loadedData.maxHeart);
        }
    }
}