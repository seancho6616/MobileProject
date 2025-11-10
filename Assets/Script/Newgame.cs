using UnityEngine;
using UnityEngine.SceneManagement;


public class Newgame : MonoBehaviour
{
    public void OnClickStart()
    {
        SceneManager.LoadScene("MainScene");
    }
}
