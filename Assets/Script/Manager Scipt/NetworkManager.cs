using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance; 

    // 씬이 바뀔 때 데이터 들고있기
    public GameData cachedData; 
    public bool isContinue = false;

    // 서버 주소 - 로컬 테스트
    private string baseUrl = "http://localhost:3000/api"; 
    
    // 로그인 발급 토큰
    public string authToken; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

        // 회원가입
    public IEnumerator Register(string username, string password, string nickname, System.Action<bool> onResult)
    {
        UserRegisterData registerData = new UserRegisterData { username = username, password = password, nickname = nickname };
        string json = JsonUtility.ToJson(registerData);

        using (UnityWebRequest req = new UnityWebRequest(baseUrl + "/register", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            req.uploadHandler = new UploadHandlerRaw(bodyRaw);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("회원가입 성공");
                onResult?.Invoke(true);
            }
            else
            {
                Debug.LogError("회원가입 실패: " + req.downloadHandler.text);
                onResult?.Invoke(false);
            }
        }
    }

    // 로그인
    public IEnumerator Login(string username, string password, System.Action<bool> onResult)
    {
        UserLoginData loginData = new UserLoginData { username = username, password = password };
        string json = JsonUtility.ToJson(loginData);

        using (UnityWebRequest req = new UnityWebRequest(baseUrl + "/login", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            req.uploadHandler = new UploadHandlerRaw(bodyRaw);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                var response = JsonUtility.FromJson<LoginResponse>(req.downloadHandler.text);
                this.authToken = response.token; // 토큰 저장
                Debug.Log("로그인 성공");
                onResult?.Invoke(true);
            }
            else
            {
                Debug.LogError("로그인 실패: " + req.downloadHandler.text);
                onResult?.Invoke(false);
            }
        }
    }

    // 데이터 저장
    public IEnumerator SaveGameData(GameData data)
    {
        if (string.IsNullOrEmpty(authToken)) yield break;

        string json = JsonUtility.ToJson(data);
        
        using (UnityWebRequest req = new UnityWebRequest(baseUrl + "/my-data", "PATCH"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            req.uploadHandler = new UploadHandlerRaw(bodyRaw);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            req.SetRequestHeader("Authorization", "Bearer " + authToken);

            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
                Debug.Log("게임 데이터 저장 완료!");
            else
                Debug.LogError("저장 실패: " + req.downloadHandler.text);
        }
    }

    // 데이터 불러오기
    public IEnumerator LoadGameData(System.Action<GameData> onDataLoaded)
    {
        if (string.IsNullOrEmpty(authToken)) yield break;

        using (UnityWebRequest req = UnityWebRequest.Get(baseUrl + "/my-data"))
        {
            req.SetRequestHeader("Authorization", "Bearer " + authToken);
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                // JSON -> C# 객체 변환
                GameData data = JsonUtility.FromJson<GameData>(req.downloadHandler.text);
                onDataLoaded?.Invoke(data);
            }
            else
            {
                Debug.LogError("로드 실패: " + req.error);
                onDataLoaded?.Invoke(null);
            }
        }
    }
}

    [System.Serializable] class UserLoginData { public string username; public string password; }
    [System.Serializable] class UserRegisterData { public string username; public string password; public string nickname; }
    [System.Serializable] class LoginResponse { public string message; public string token; }