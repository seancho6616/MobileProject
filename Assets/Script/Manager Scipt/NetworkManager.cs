using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance; 

    // 서버 주소 - 로컬 테스트
    private string baseUrl = "https://10.20.33.14/api"; 
    
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
        // 보낼 데이터 JSON 생성
        UserRegisterData registerData = new UserRegisterData 
        { 
            username = username, 
            password = password, 
            nickname = nickname 
        };
        
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
                Debug.Log("회원가입 성공: " + req.downloadHandler.text);
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
        // 보낼 데이터 JSON 생성
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
                Debug.Log("로그인 성공: " + req.downloadHandler.text);
                
                // 응답 JSON에서 토큰 추출
                var response = JsonUtility.FromJson<LoginResponse>(req.downloadHandler.text);
                this.authToken = response.token; // 토큰 저장
                
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
        if (string.IsNullOrEmpty(authToken))
        {
            Debug.LogError("토큰이 없습니다. 로그인 먼저 하세요.");
            yield break;
        }

        string json = JsonUtility.ToJson(data);
        
        using (UnityWebRequest req = new UnityWebRequest(baseUrl + "/my-data", "PATCH"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            req.uploadHandler = new UploadHandlerRaw(bodyRaw);
            req.downloadHandler = new DownloadHandlerBuffer();
            
            // 헤더 설정 (토큰 포함 필수)
            req.SetRequestHeader("Content-Type", "application/json");
            req.SetRequestHeader("Authorization", "Bearer " + authToken);

            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("게임 데이터 저장 완료!");
            }
            else
            {
                Debug.LogError("저장 실패: " + req.downloadHandler.text);
            }
        }
    }

    // 데이터 불러오기
    public IEnumerator LoadGameData(System.Action<GameData> onDataLoaded)
    {
        if (string.IsNullOrEmpty(authToken))
        {
            Debug.LogError("토큰이 없습니다. 로그인 상태가 아닙니다.");
            yield break;
        }

        using (UnityWebRequest req = UnityWebRequest.Get(baseUrl + "/my-data"))
        {
            // 헤더 설정 (토큰 포함 필수)
            req.SetRequestHeader("Authorization", "Bearer " + authToken);

            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("서버에서 받은 원본 데이터: " + req.downloadHandler.text);
                Debug.Log("데이터 로드 성공");
                // JSON -> C# 객체 변환
                GameData data = JsonUtility.FromJson<GameData>(req.downloadHandler.text);
                onDataLoaded?.Invoke(data);
            }
            else
            {
                Debug.LogError("로드 실패: " + req.error);
            }
        }
    }

    // 데이터 클래스 정의

    [System.Serializable]
    class UserLoginData 
    { 
        public string username; 
        public string password; 
    }
    
    [System.Serializable]
    class UserRegisterData 
    { 
        public string username; 
        public string password; 
        public string nickname; 
    }
    
    [System.Serializable]
    class LoginResponse 
    { 
        public string message; 
        public string token; 
    }
}