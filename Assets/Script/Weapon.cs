using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponState weaponState;
    [SerializeField] float rotateSpeed;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);        
        
    }
}
