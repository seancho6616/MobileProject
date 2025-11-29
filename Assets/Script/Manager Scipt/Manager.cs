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
        if (player == null)
        {
            // 1. "Player" 태그로 찾아보기
            player = GameObject.FindGameObjectWithTag("Player");

            // 2. 태그로 못 찾았으면 PlayerControl 스크립트를 가진 애를 찾아보기
            if (player == null)
            {
                PlayerControl pc = FindAnyObjectByType<PlayerControl>();
                if (pc != null) player = pc.gameObject;
            }
        }

        if (player != null)
        {
            if (playerStats == null) playerStats = player.GetComponent<PlayerStats>();
            if (playerManager == null) playerManager = player.GetComponent<PlayerManager>();
        }
        else
        {
            Debug.LogError("플레이어 찾기 실패");
        }
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
   // Manager.cs 안에 있는 SaveGame 함수 수정

public void SaveGame()
{
    if (player == null) 
    {
        Debug.LogError("저장 실패: Player 오브젝트가 연결되지 않았습니다.");
        return;
    }

    Debug.Log("--- 게임 데이터 저장 시작 ---");
    GameData data = new GameData();

    // 1. 위치 정보 저장
    data.position = new PositionData(
        player.transform.position.x,
        player.transform.position.y,
        player.transform.position.z
    );
    data.lastScene = SceneManager.GetActiveScene().name;

    // 2. PlayerStats 데이터 저장 (체력, 스태미너, 코인, 포션, **이동속도**)
    if (playerStats != null)
    {
        data.maxHeart = (int)playerStats.MaxHealth;
        data.currentHeart = (int)playerStats.CurrentHealth;

        data.maxStamina = (int)playerStats.MaxStamina;
        data.currentStamina = (int)playerStats.CurrentStamina;

        data.coins = playerStats.CoinCount;
        data.potionCount = playerStats.PotionCount;
        
        data.speed = (int)playerStats.MoveSpeed; 
        
        if(playerManager != null)
        {
            data.baseAttack = playerManager.baseAttack;
            data.baseAttackSpeed = playerManager.baseAttackSpeed;
            data.attackRange = playerManager.attackRange;
            data.equippedWeaponId = playerManager.equippedWeaponId;
        }
    }

    // 3. 서버로 전송
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