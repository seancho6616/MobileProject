using UnityEngine;
using UnityEngine.UI;

// 캐릭터 충돌 관련 코드
public class PlayerCollider : MonoBehaviour 
{
    PlayerStats playerStats; // PlayerStats.cd 에서 모든 데이터 관리
    PlayerControl playerControl; // 아이템 획득 상태 변경
    AttackImageChanger attackImageChanger;
    Item nearbyItem;
    Weapon nearbyWeapon;
    TextUI textUI;
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        playerControl = GetComponent<PlayerControl>();
        attackImageChanger = FindAnyObjectByType<AttackImageChanger>();
        textUI = FindAnyObjectByType<TextUI>();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Item itemComponent))
        {
            nearbyItem = itemComponent;
            UseItem(other);
        }
        else if(other.TryGetComponent(out Weapon weaponComponent))
        {
            nearbyWeapon = weaponComponent;
            attackImageChanger.ChangeSprite();
            playerControl.isWeapon = true;
            playerControl.triggrtWeapon = weaponComponent;
            playerControl.Item = other.gameObject;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Monster"))
        {
            attackImageChanger.BeforeChangeSprite();
            playerControl.MakeFalse(); // 모든 아이템 획득 상태 false로 초기화
            nearbyWeapon = null;
            nearbyItem = null;
        }
    }

    void UseItem(Collider other)
    {
        switch (nearbyItem.itemState.itemType)
        {
            case ItemState.Item.Coin:
                playerStats.CoinCount += 1;
                // UI 갱신
                textUI.CountCoin(playerStats.CoinCount);
                Destroy(other.gameObject);
                break;
            case ItemState.Item.Potion:
                attackImageChanger.ChangeSprite();
                playerControl.pickupPotion = true;
                playerControl.Item = other.gameObject;
                break;
            case ItemState.Item.Heart:
                attackImageChanger.ChangeSprite();
                playerControl.pickupHeart = true;
                playerControl.Item = other.gameObject;
                break;
            case ItemState.Item.Stamina:
                attackImageChanger.ChangeSprite(); // 무기 아이콘이 획득 아이콘으로 변경
                playerControl.pickupStamina = true;
                playerControl.Item = other.gameObject;
                break;
            default:
                Debug.Log("Nothing Item");
                break;
        }
    }
}

    // private void OnTriggerEnter(Collider other) {
    //     // 스태미너 아이템
    //     if(other.CompareTag("Stamina"))
    //     {
    //         attackImageChanger.ChangeSprite(); // 무기 아이콘이 획득 아이콘으로 변경
    //         playerControl.pickupStamina = true;
    //         playerControl.Item = other.gameObject;
    //     }
    //     // 목숨 아이템
    //     if(other.CompareTag("Heart"))
    //     {
    //         attackImageChanger.ChangeSprite();
    //         playerControl.pickupHeart = true;
    //         playerControl.Item = other.gameObject;
    //     }
    //     // 포션 아이템 -> 갯수 증가만 하고 사용은 원할 때
    //     if(other.CompareTag("Potion"))
    //     {
    //         attackImageChanger.ChangeSprite();
    //         playerControl.pickupPotion = true;
    //         playerControl.Item = other.gameObject;
    //     }
    //     // 코인 아이템 -> 즉시 획득
    //     if(other.CompareTag("Coin"))
    //     {
    //         playerStats.CoinCount += 1;
    //         // UI 갱신
    //         textUI.CountCoin(playerStats.CoinCount);
    //         Destroy(other.gameObject);
    //     }
    // }