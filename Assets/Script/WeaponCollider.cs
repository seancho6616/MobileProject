using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    PlayerControl playerControl;
    void Start()
    {
        playerControl = FindAnyObjectByType<PlayerControl>();
    }
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Monster"))
        {
            playerControl.isMonster = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            playerControl.isMonster = false;
        }
    }

}
