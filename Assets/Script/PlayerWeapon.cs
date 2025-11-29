using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeapon : MonoBehaviour
{
    Dictionary<int, GameObject> objectRegistry = new Dictionary<int, GameObject>();
    Dictionary<int, Image> objectUI = new Dictionary<int, Image>();
    public GameObject[] managedWeapon;
    public Image[] manageUI;


    void Awake()
    {
        for(int i = 0; i<managedWeapon.Length; i++)
        {
            Debug.Log(i+":"+managedWeapon[i]);
            objectRegistry.Add(i+2031, managedWeapon[i]);
            objectUI.Add(i+2031, manageUI[i]);
        }
    }

    public void ActivateWeaponID(int weaponID)
    {
        if(objectRegistry.TryGetValue(weaponID, out GameObject targetWeapon) && objectUI.TryGetValue(weaponID, out Image targetUI))
        {
            targetWeapon.SetActive(true);
            targetUI.gameObject.SetActive(true);
            Debug.Log($"ID {weaponID}의 오브젝트 ({targetWeapon.name})를 활성화했습니다.");
        }
        else
        {
            Debug.LogWarning($"경고: ID {weaponID}를 가진 오브젝트가 딕셔너리에 없습니다.");
        }
    }
    public void DeactivateWeaponID(int weaponID)
    {
        if(objectRegistry.TryGetValue(weaponID, out GameObject targetWeapon)&& objectUI.TryGetValue(weaponID, out Image targetUI))
        {
            targetWeapon.SetActive(false);
            targetUI.gameObject.SetActive(false);
            Debug.Log($"ID {weaponID}의 오브젝트 ({targetWeapon.name})를 비활성화했습니다.");
        }
        else
        {
            Debug.LogWarning($"경고: ID {weaponID}를 가진 오브젝트가 딕셔너리에 없습니다.");
        }
    }
}
