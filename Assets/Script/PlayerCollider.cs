using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Stamina")
        {
            Debug.Log("Hi Stamina");
            Destroy(other.gameObject);
        }
    }
}
