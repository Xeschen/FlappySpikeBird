using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class BirdController : MonoBehaviour
{
    public static event Action<float> OnDirectionChanged; // Event to notify direction changes

    public Sprite birdUp;
    public Sprite birdDown;

    public float velocityX;
    public float jumpForce;
    public float gravityScale; // Gravity scale for the Rigidbody2D component

    private RectTransform birdUI;
    private Rigidbody2D rb;
    private Image image;
    private Vector3 initialPosition;
    private bool positionInit = false;
    private float direction = 1f; // Direction multiplier for horizontal movement
    private bool jumpRequest = false;
    private bool isSpiked = false;
    private bool isDead = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        birdUI = GetComponent<RectTransform>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale; // Set the gravity scale for the Rigidbody2D component
        image = GetComponent<Image>();

        initialPosition = transform.position;
        positionInit = true;
        OnDirectionChanged?.Invoke(direction);
    }

    private void OnEnable()
    {
        InitBird();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpiked)
        {
            OnDead();
            return;
        }

        if (birdUI != null)
        {
            birdUI.position = transform.position;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame) // Check if the left mouse button was pressed this frame
        {
            AudioManager.Instance.PlayJump(); // Play jump sound
            jumpRequest = true;
        }

        UpdateSprite();
    }

    private void FixedUpdate()
    {
        if (isSpiked) return;

        if (jumpRequest)
        {
            Jump();
            jumpRequest = false;
        }

        HorizontalMove();
    }

    void InitBird()
    {
        GameManager.Instance.ResetScore();
        isSpiked = false;
        isDead = false;
        if (positionInit) transform.position = initialPosition;
        if (image != null)
        {
            image.color = Color.white;
        }
        transform.localRotation = new Quaternion(0, 0, 0, 0);
        direction = 1f;
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(velocityX * direction, 0); // 연속점프 초기화
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void HorizontalMove()
    {
        var velocity = rb.linearVelocity;
        velocity.x = velocityX * direction;
        rb.linearVelocity = velocity;
    }

    void UpdateSprite()
    {
        if (rb.linearVelocity.y > 0.1)
        {
            image.sprite = birdUp; // Upward movement
        }
        else if (rb.linearVelocity.y < -0.1)
        {
            image.sprite = birdDown; // Downward movement
        }

        var scale = transform.localScale;
        scale.x = rb.linearVelocityX < 0 ? -1 : 1;
        transform.localScale = scale;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") && isSpiked == false)
        {
            direction *= -1f;
            OnDirectionChanged?.Invoke(direction); // Notify subscribers of the direction change
            GameManager.Instance.AddScore(1); // Increase score by 1 on wall collision
            AudioManager.Instance.PlayBounce(); // Play bounce sound
        }
        else if (collision.gameObject.CompareTag("Spike"))
        {
            isSpiked = true;
            image.color = Color.Lerp(image.color, Color.gray, 0.5f);
            AudioManager.Instance.PlaySpikeHit(); // Play spike hit sound
        }
    }

    private void OnDead()
    {
        if (isDead) return;
        if (rb.linearVelocity.magnitude > 1) return;

        isDead = true;
        GameManager.Instance.EndGame();
        AudioManager.Instance.PlayGameEnd(); // Play game end sound
    }
}
