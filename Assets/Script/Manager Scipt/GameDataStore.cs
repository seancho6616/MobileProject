using UnityEngine;

// 씬 이동 간 데이터 전달 해줌
public class GameDataStore : MonoBehaviour
{
    public static GameDataStore Instance;

    [Header("Delivery Data")]
    public GameData cachedData;     // 불러온 게임 데이터 임시 보관
    public bool isContinue = false; // 이어하기 버튼을 눌렀는지

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            // 이미 데이터가 있으면 삭제
            Destroy(gameObject);
        }
    }
}