using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private int m_maxSpeed, m_acceleration, m_deceleration, m_jumpForce;
    [SerializeField] private float m_lowJumpModifier, m_fallModifier;
    private Animator m_animator;
    private BoxCollider2D m_collider;
    private bool m_isGrounded;
    private Rigidbody2D m_rb;
    private float m_jumpInput, m_xAxisInput, m_yAxisInput;

    public int PlayerIndex { get; private set; }
    public string PlayerName { get; private set; }

    public int score { get; private set; }

    private PlayerInput _playerInput;
    private PlayerCustomization _customization;

    public void Setup(PlayerInput input)
    {
        _playerInput = input;
        switchActionMap();
        
    }

    public void switchActionMap()
    {
        if (_playerInput == null)
        {
            Debug.LogError("PlayerInput component is not assigned.");
            return;
        }
        switch (GameManagerRemake.Instance.CurrentGameState)
        {
            case GameManagerRemake.GameStates.MainMenu:
            case GameManagerRemake.GameStates.Lobby:
                _playerInput.SwitchCurrentActionMap("UI");
                break;
            case GameManagerRemake.GameStates.Scoreboard:
            case GameManagerRemake.GameStates.Level:
                _playerInput.SwitchCurrentActionMap("Keyboard");
                break;
        }
        Debug.Log($"Switched action map to {_playerInput.currentActionMap.name} for player {PlayerIndex}");
    }

    private void Awake()
    {
        //_playerInput = GetComponent<PlayerInput>();
        _customization = GetComponent<PlayerCustomization>();
    }

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_collider = GetComponent<BoxCollider2D>();
        m_rb = GetComponent<Rigidbody2D>();
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        Debug.Log("Player is submitting");
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        Debug.Log("Player is canceling");
    }

    public void InitializePlayer(int index, string name)
    {
        PlayerIndex = index;
        PlayerName = name;

        // Randomize visual customization
        _customization?.Randomize();
    }

    public void SetVisible(bool visible)
    {
        foreach (var r in GetComponentsInChildren<Renderer>()) r.enabled = visible;
        foreach (var c in GetComponentsInChildren<Collider2D>()) c.enabled = visible;
        GetComponent<Rigidbody2D>().simulated = visible;
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        m_xAxisInput = context.ReadValue<Vector2>().x;

        float previousYAxisInput = m_yAxisInput;
        m_yAxisInput = context.ReadValue<Vector2>().y;

        if (previousYAxisInput != m_yAxisInput && previousYAxisInput == -1)
            m_animator.SetBool("crouching", false);

        if (m_yAxisInput == -1 && m_isGrounded)
            m_animator.SetBool("crouching", true);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        m_jumpInput = context.ReadValue<float>();
        if (m_isGrounded)
        {
            m_rb.gravityScale = 1.0f;

            if (m_jumpInput != 0) Jump();
        }
    }

    private void Update()
    {


    }

    private void FixedUpdate()
    {
        if (ComputeIsStandingOn("Solid") && !m_isGrounded)
            m_animator.SetTrigger("Impact");

        m_isGrounded = ComputeIsStandingOn("Solid");
        m_animator.SetBool("grounded", m_isGrounded);

        ComputeVelocity();
    }

    private void ComputeVelocity()
    {
        ComputeXVelocity();
        ComputeYVelocity();
    }

    private void ComputeXVelocity()
    {
        float xVelocity = 0;

        if (m_xAxisInput != 0) // Accelerate
        {
            xVelocity = Mathf.MoveTowards(m_rb.linearVelocity.x, m_maxSpeed * m_xAxisInput, m_acceleration * Time.deltaTime);
        }
        else // Decelerate
        {
            xVelocity = Mathf.MoveTowards(m_rb.linearVelocity.x, 0, m_deceleration * Time.deltaTime);
        }
        m_rb.linearVelocity = new Vector2(xVelocity, m_rb.linearVelocity.y);
    }

    private void ComputeYVelocity()
    {
        // Up / Down acceleration
        if (m_rb.linearVelocity.y > 0 && m_jumpInput == 0 && m_rb.gravityScale <= 1) m_rb.gravityScale += m_lowJumpModifier;
        if (m_rb.linearVelocity.y < 0 && m_rb.gravityScale <= 1.0f + m_lowJumpModifier) m_rb.gravityScale += m_fallModifier;
    }

    private bool ComputeIsStandingOn(string tag)
    {
        // Make three raycasts for more accuracy
        float epsilon = 0.0625f;
        Vector3 offset = new Vector3(GetComponent<BoxCollider2D>().offset.x, GetComponent<BoxCollider2D>().offset.y);
        Ray ray = new(transform.position - new Vector3(0, m_collider.size.y / 2.0f) + offset, Vector3.down);
        Debug.DrawRay(ray.origin, Vector2.down, Color.yellow);
        RaycastHit2D[] isMiddleTouching = Physics2D.RaycastAll(ray.origin, ray.direction, epsilon);
        RaycastHit2D[] isLeftTouching = Physics2D.RaycastAll(ray.origin - new Vector3(m_collider.size.x / 2.0f - epsilon, 0), ray.direction, epsilon);
        RaycastHit2D[] isRightTouching = Physics2D.RaycastAll(ray.origin + new Vector3(m_collider.size.x / 2.0f - epsilon, 0), ray.direction, epsilon);

        // Check if raycasts collide with ground
        foreach (RaycastHit2D collision in isMiddleTouching)
        {
            if (collision.collider.tag.Equals(tag)) return true;
        }
        foreach (RaycastHit2D collision in isLeftTouching)
        {
            if (collision.collider.tag.Equals(tag)) return true;
        }
        foreach (RaycastHit2D collision in isRightTouching)
        {
            if (collision.collider.tag.Equals(tag)) return true;
        }

        return false;
    }

    public void Hurt()
    {
        m_animator.SetTrigger("Hurt");
    }

    private void Jump()
    {
        m_rb.linearVelocity = new Vector2(m_rb.linearVelocity.x, 0);
        m_rb.AddForce(new Vector2(0f, m_jumpForce), ForceMode2D.Impulse);
        m_animator.SetTrigger("Jump");
    }

    public void AddScore(int score)
    {
        int scoreTest = Random.Range(0,999);//Testing
        this.score += scoreTest;
    }
}