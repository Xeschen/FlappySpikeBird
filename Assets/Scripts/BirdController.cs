using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class BirdController : MonoBehaviour
{
    public static event Action<float> OnDirectionChanged; // Event to notify direction changes

    public float velocityX;
    public float jumpForce;
    public float gravityScale; // Gravity scale for the Rigidbody2D component

    private RectTransform birdUI;
    private Rigidbody2D rb;
    private float direction = 1f; // Direction multiplier for horizontal movement
    private bool jumpRequest = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        birdUI = GetComponent<RectTransform>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale; // Set the gravity scale for the Rigidbody2D component

        OnDirectionChanged?.Invoke(direction);
    }

    // Update is called once per frame
    void Update()
    {
        if (birdUI != null)
        {
            birdUI.position = transform.position;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame) // Check if the left mouse button was pressed this frame
        {
            jumpRequest = true;
        }
    }

    private void FixedUpdate()
    {
        if (jumpRequest)
        {
            Jump();
            jumpRequest = false;
        }

        HorizontalMove();
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(velocityX * direction, 0); // 연속점프 초기화
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void HorizontalMove()
    {
        Vector2 velocity = rb.linearVelocity;
        velocity.x = velocityX * direction;
        rb.linearVelocity = velocity;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            direction *= -1f;
            OnDirectionChanged?.Invoke(direction); // Notify subscribers of the direction change
        }
        else if (collision.gameObject.CompareTag("Spike"))
        {
            rb.simulated = false; // Disable physics simulation on collision with spikes
        }
    }
}
