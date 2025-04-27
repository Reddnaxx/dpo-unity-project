using UnityEngine;

namespace Assets._00_Scripts.Input
{
    public class EnemyFollow : MonoBehaviour
    {
        [SerializeField] public Transform player;
        [SerializeField] public float speed = 2f;

        private Rigidbody2D rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (player == null) return;

            Vector3 direction = (player.position - transform.position).normalized;

            rb.linearVelocity = direction * speed;
        }
    }
}