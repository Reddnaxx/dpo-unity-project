using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); 
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(moveX, moveY).normalized * moveSpeed;
        rb.velocity = movement;
    }
}