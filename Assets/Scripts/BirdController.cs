using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class BirdController : MonoBehaviour
{
    public static BirdController Instance; // Singleton instance for easy access

    public static event Action OnDirectionChanged; // Event to notify direction changes

    public static event Action OnDead;

    public Sprite birdUp;
    public Sprite birdDown;

    public float velocityX;
    public float jumpForce;
    public float gravityScale; // Gravity scale for the Rigidbody2D component
    public float direction = 1f; // Direction multiplier for horizontal movement

    [SerializeField] private GameObject birdUI;
    [SerializeField] private ParticleSystem trailParticle;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector3 initialPosition;
    private bool positionInit = false;
    private bool isPlayable = false;
    private bool isSpiked = false;
    private bool isDead = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        initialPosition = transform.position;
        positionInit = true;

        SetPlayable(false);
    }

    private void OnEnable()
    {
        InitBird();
    }

    private void Update()
    {
        if (isPlayable == false) return;

        if (isSpiked)
        {
            if (isDead) return;
            if (rb.linearVelocity.magnitude > 1) return;

            isDead = true;
            OnDead.Invoke();

            return;
        }

        UpdateSprite();
    }

    private void FixedUpdate()
    {
        if (isPlayable && !isSpiked)
        {
            HorizontalMove();
        }
    }

    public void SetPlayable(bool playable)
    {
        if (playable)
        {
            InitBird(); // 시작 시 위치 등 초기화
        }

        isPlayable = playable;

        gameObject.SetActive(isPlayable);

        if (birdUI != null)
        {
            birdUI.SetActive(!isPlayable);
        }

        // 중력 설정
        if (rb != null)
        {
            rb.gravityScale = isPlayable ? gravityScale : 0f;
        }
    }

    private void InitBird()
    {
        isSpiked = false;
        isDead = false;

        if (positionInit)
        {
            transform.position = initialPosition;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white; // Reset color to white
        }
        transform.localRotation = Quaternion.identity;
        direction = 1f;
        if (transform.localScale.x < 0)
        {
            FlipBird();
        }
    }

    public void Jump()
    {
        if (!isPlayable || isSpiked) return;

        rb.linearVelocity = new Vector2(velocityX * direction, 0); // 연속점프 초기화
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        if (trailParticle != null)
        {
            trailParticle.transform.position = transform.position;
            trailParticle.Play();
        }

        AudioManager.Instance.PlayJump();
    }

    private void HorizontalMove()
    {
        var velocity = rb.linearVelocity;
        velocity.x = velocityX * direction;
        rb.linearVelocity = velocity;
    }

    private void UpdateSprite()
    {
        if (spriteRenderer == null) return; // Ensure image is not null before accessing it
        if (rb.linearVelocity.y > 0.1)
        {
            spriteRenderer.sprite = birdUp; // Upward movement
        }
        else if (rb.linearVelocity.y < -0.1)
        {
            spriteRenderer.sprite = birdDown; // Downward movement
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") && isSpiked == false)
        {
            direction *= -1f;
            FlipBird();
            OnDirectionChanged?.Invoke();
        }
        else if (collision.gameObject.CompareTag("Spike") && isDead == false)
        {
            isSpiked = true;
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, Color.gray, 0.5f);
            AudioManager.Instance.PlaySpikeHit(); // Play spike hit sound
        }
    }

    private void FlipBird()
    {
        var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        transform.localRotation = Quaternion.identity;
    }
}