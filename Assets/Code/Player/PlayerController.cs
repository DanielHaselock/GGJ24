using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int m_maxSpeed, m_acceleration, m_deceleration, m_jumpForce;
    [SerializeField] private float m_lowJumpModifier, m_fallModifier;

    private Animator m_animator;
    private BoxCollider2D m_collider;
    private bool m_isGrounded;
    private PlayerInputSystem m_playerInputSystem;
    private Rigidbody2D m_rb;
    private float m_jumpInput, m_xAxisInput, m_yAxisInput;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_collider = GetComponent<BoxCollider2D>();
        m_rb = GetComponent<Rigidbody2D>();
        m_playerInputSystem = new PlayerInputSystem();
        m_playerInputSystem.Platforming.Enable();
    }

    private void Update()
    {
        m_jumpInput = m_playerInputSystem.Platforming.Jump.ReadValue<float>();
        m_xAxisInput = m_playerInputSystem.Platforming.Movement.ReadValue<Vector2>().x;

        float previousYAxisInput = m_yAxisInput;
        m_yAxisInput = m_playerInputSystem.Platforming.Movement.ReadValue<Vector2>().y;

        if (previousYAxisInput != m_yAxisInput && previousYAxisInput == -1)
            m_animator.SetBool("crouching", false);

        if (m_yAxisInput == -1 && m_isGrounded)
            m_animator.SetBool("crouching", true);
            
    }

    private void FixedUpdate()
    {
        if (ComputeIsStandingOn("Solid") && !m_isGrounded)
            m_animator.SetTrigger("Impact");

        m_isGrounded = ComputeIsStandingOn("Solid");
        m_animator.SetBool("grounded", m_isGrounded);

        if (m_isGrounded)
        {
            m_rb.gravityScale = 1.0f;

            if (m_jumpInput != 0) Jump();
        }

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
}