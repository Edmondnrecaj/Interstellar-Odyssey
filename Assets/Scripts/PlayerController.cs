using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 6f;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 14f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Health")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private AudioSource damageAudio;
    private int currentHealth;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public System.Action<int> OnHealthChanged;

    private bool isDead = false;
    private bool isWon = false; 

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveInput;

    public static PlayerController Instance;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        Debug.Log($"Player Health: {currentHealth}/{maxHealth}");

        OnHealthChanged?.Invoke(currentHealth);
    }

    private void Update()
    {
        if (isDead || isWon) return;

        HandleMovement();
        HandleSpriteFlip();
    }

    private void HandleMovement()
    {
        Vector2 velocity = rb.linearVelocity;
        velocity.x = moveInput.x * walkSpeed;
        rb.linearVelocity = velocity;
    }

    private void HandleSpriteFlip()
    {
        if (moveInput.x > 0.01f)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveInput.x < -0.01f)
        {
            spriteRenderer.flipX = true;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (isDead || isWon) return;

        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (isDead || isWon) return;

        if (context.performed && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = IsGrounded() ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Alien"))
        {
            TakeDamage(1);
        }
    }

    private void TakeDamage(int damage)
    {
        damageAudio.Play();
        currentHealth -= damage;

        OnHealthChanged?.Invoke(currentHealth);

        Debug.Log($"Player hit! Health now: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

   private void Die()
    {
        currentHealth = 0;
        isDead = true;
        OnHealthChanged?.Invoke(0);

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Static;

        spriteRenderer.color = Color.red;
        Debug.Log("Player Died! Game stopped.");
    }

    public void Win()
    {
        if (isDead || isWon) return;

        isWon = true;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Static;

        spriteRenderer.color = Color.green;

        Debug.Log("LEVEL COMPLETE! You escaped through the portal!");
    }
}