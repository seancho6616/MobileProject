using UnityEngine;

public class Manager : MonoBehaviour
{
    [Header("Managers")]
    public PlayerManager playerManager; // 스탯
    public HeartManager heartManager;   // 체력
    public GameObject player;           // 위치

    void Start()
    {
        // 게임 시작 시 데이터 불러오기 추가
        // 이어하기 했을 시에
    }

    // 저장하기
    void SaveGame()
    {
        GameData data = new GameData();

        // 위치 저장
        data.position = new PositionData(
            player.transform.position.x,
            player.transform.position.y,
            player.transform.position.z
        );
        
        data.lastScene = "MainScene";

        // 체력 불러오기 (float -> int) -> HeartManager
        data.maxHeart = (int)heartManager.MaxHealth;
        data.currentHeart = (int)heartManager.CurrentHealth;

        // 스탯 불러오기 -> PlayerManager
        data.coins = playerManager.coins;
        data.potionCount = playerManager.potionCount;
        data.maxStamina = playerManager.maxStamina;
        data.currentStamina = playerManager.currentStamina;
        data.speed = playerManager.speed;
        data.baseAttack = playerManager.baseAttack;
        data.baseAttackSpeed = playerManager.baseAttackSpeed;
        data.attackRange = playerManager.attackRange;
        data.equippedWeaponId = playerManager.equippedWeaponId;

        // 서버로 전송
        StartCoroutine(NetworkManager.Instance.SaveGameData(data));
    }

    // 이어하기
    public void LoadGame()
    {
        StartCoroutine(NetworkManager.Instance.LoadGameData((loadedData) =>
        {

            // 위치 적용
            player.transform.position = new Vector3(
                loadedData.position.x,
                loadedData.position.y,
                loadedData.position.z
            );

            // 체력 적용
            heartManager.SetHealthDirectly(loadedData.currentHeart, loadedData.maxHeart);

            // 스탯 적용
            playerManager.coins = loadedData.coins;
            playerManager.potionCount = loadedData.potionCount;
            playerManager.maxStamina = loadedData.maxStamina;
            playerManager.currentStamina = loadedData.currentStamina;
            playerManager.speed = loadedData.speed;
            playerManager.baseAttack = loadedData.baseAttack;
            playerManager.baseAttackSpeed = loadedData.baseAttackSpeed;
            playerManager.attackRange = loadedData.attackRange;
            playerManager.equippedWeaponId = loadedData.equippedWeaponId;

            Debug.Log("게임 데이터 로드 완료");
        }));
    }
}
