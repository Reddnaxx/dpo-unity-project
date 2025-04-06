using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}