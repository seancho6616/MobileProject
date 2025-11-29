using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance; 

    [Header("Data Cache")]
    public GameData cachedData; 
    public bool isContinue = false;

    [Header("Network Settings")]
    private string baseUrl = "http://localhost:3000/api"; 
    public string authToken; 

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    //1. 핵심 기능

    public IEnumerator Register(string username, string password, string nickname, Action<bool> onResult)
    {
        var data = new UserRegisterData { username = username, password = password, nickname = nickname };
        yield return SendRequest("/register", "POST", data, (json) => {
            Debug.Log("회원가입 성공");
            onResult?.Invoke(true);
        }, () => onResult?.Invoke(false));
    }

    public IEnumerator Login(string username, string password, Action<bool> onResult)
    {
        var data = new UserLoginData { username = username, password = password };
        yield return SendRequest("/login", "POST", data, (json) => {
            var res = JsonUtility.FromJson<LoginResponse>(json);
            this.authToken = res.token; // 토큰 저장
            Debug.Log("로그인 성공");
            onResult?.Invoke(true);
        }, () => onResult?.Invoke(false));
    }

    public IEnumerator SaveGameData(GameData data, Action onComplete = null)
    {
        Debug.Log("--- 2. [네트워크] 서버에 PATCH 요청 시작 ---");
        if (string.IsNullOrEmpty(authToken))
        {
            Debug.LogError("[SAVE] 저장 실패: AuthToken이 없습니다. 로그인 상태를 확인하세요.");
            yield break;
        }
        Debug.Log("[SAVE] AuthToken 확인 완료. 서버에 데이터 전송 시작...");

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
            {
                Debug.Log("게임 데이터 저장 완료");
                onComplete?.Invoke();
            }
            else
            {
                Debug.LogError("저장 실패: " + req.downloadHandler.text);
                onComplete?.Invoke();
            }
        }
    }

    public IEnumerator LoadGameData(Action<GameData> onDataLoaded)
    {
        if (string.IsNullOrEmpty(authToken)) { onDataLoaded?.Invoke(null); yield break; }
        yield return SendRequest("/my-data", "GET", null, (json) => {
            var data = JsonUtility.FromJson<GameData>(json);
            onDataLoaded?.Invoke(data);
        }, () => onDataLoaded?.Invoke(null));
    }

    // 2. 통신 도우미
    
    private IEnumerator SendRequest(string path, string method, object data, Action<string> onSuccess, Action onFail = null)
    {
        string url = baseUrl + path;
        string json = data != null ? JsonUtility.ToJson(data) : null;

        using (UnityWebRequest req = new UnityWebRequest(url, method))
        {
            if (json != null)
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
                req.uploadHandler = new UploadHandlerRaw(bodyRaw);
            }
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            
            if (!string.IsNullOrEmpty(authToken))
                req.SetRequestHeader("Authorization", "Bearer " + authToken);

            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(req.downloadHandler.text);
            }
            else
            {
                Debug.LogError($"[{method}] {path} 실패: {req.error} / {req.downloadHandler.text}");
                onFail?.Invoke();
            }
        }
    }

    

    // 데이터 클래스들
    [Serializable] class UserLoginData { public string username; public string password; }
    [Serializable] class UserRegisterData { public string username; public string password; public string nickname; }
    [Serializable] class LoginResponse { public string message; public string token; }
}