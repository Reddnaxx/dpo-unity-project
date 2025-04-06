using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.5f;
    
    private float nextFireTime; 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, transform.rotation);
        
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        
        rb.velocity = transform.right * projectileSpeed;
    }
}