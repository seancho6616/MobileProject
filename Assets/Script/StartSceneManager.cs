using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    // 이어하기
    public void OnClickContinue()
    {
        Debug.Log("이어하기, 게임으로 이동");
        SceneManager.LoadScene("MainScene");
    }

    // 새 게임
    public void OnClickNewGame()
    {
        SceneManager.LoadScene("MainScene");
    }
}