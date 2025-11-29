using UnityEngine;

public class MonsterCollider : MonoBehaviour
{
    MonsterAI monsterAI;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            monsterAI.isWeapon = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            monsterAI.isWeapon = false;
        }
    }
}
