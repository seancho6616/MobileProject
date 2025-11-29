using UnityEngine;
using UnityEngine.UI;

public class SaveButtonConnector : MonoBehaviour
{
    // 저장 버튼 클릭 시 이 함수를 실행
    public void OnClickSave()
    {
        if (Manager.Instance != null)
        {
            Debug.Log("저장을 시작합니다...");
            Manager.Instance.SaveGame(); // Manager에 있는 진짜 저장 함수 호출
        }
        else
        {
            Debug.LogError("저장을 할 수 없습니다.");
        }
    }
}