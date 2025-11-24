using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public GameObject bulletPrefab; 
    public Transform firePoint;     
    public Transform player;        
    
    public float attackInterval = 3f; // 공격 간격
    public float bulletSpeed = 10f;   

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= attackInterval)
        {
            Shoot();
            timer = 0f; 
        }
    }

    void Shoot()
    {
        if (player == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Vector3 direction = (player.position - firePoint.position).normalized;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }
        Destroy(bullet, 5f);
    }
}