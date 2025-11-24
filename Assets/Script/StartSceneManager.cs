using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    // 이어하기
    public void OnClickContinue()
    {
        Debug.Log("이어하기 시도: 서버에서 데이터를 불러옵니다...");

        StartCoroutine(NetworkManager.Instance.LoadGameData((data) =>
        {
            if (data != null)
            {
                Debug.Log("데이터 로드 성공! GameDataStore에 싣습니다.");

                // 2. GameDataStore가 없으면 임시로 생성
                if (GameDataStore.Instance == null)
                {
                    GameObject go = new GameObject("GameDataStore");
                    go.AddComponent<GameDataStore>();
                }

                GameDataStore.Instance.cachedData = data;
                GameDataStore.Instance.isContinue = true;

                // 저장된 마지막 씬으로 이동 (저장된 게 없으면 MainScene)
                string sceneToLoad = string.IsNullOrEmpty(data.lastScene) ? "MainScene" : data.lastScene;
                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                Debug.LogError("서버에 저장된 데이터가 없습니다.");
            }
        }));
    }

    // 새 게임
    public void OnClickNewGame()
    {
        Debug.Log("새 게임 시작");

        if (GameDataStore.Instance != null)
        {
            GameDataStore.Instance.isContinue = false;
            GameDataStore.Instance.cachedData = null;
        }

        // 바로 게임 씬으로 이동
        SceneManager.LoadScene("MainScene");
    }
}