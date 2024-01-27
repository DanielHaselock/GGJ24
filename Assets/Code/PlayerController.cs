using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int m_maxSpeed, m_acceleration, m_deceleration, m_jumpForce;
    [SerializeField] private float m_lowJumpModifier, m_fallModifier;

    private bool m_isGrounded;
    private PlayerInputSystem m_playerInputSystem;
    private Rigidbody2D m_rb;
    private float m_jumpInput, m_xAxisInput;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_playerInputSystem = new PlayerInputSystem();
        m_playerInputSystem.Platforming.Enable();
    }

    private void Update()
    {
        m_jumpInput = m_playerInputSystem.Platforming.Jump.ReadValue<float>();
        m_xAxisInput = m_playerInputSystem.Platforming.Movement.ReadValue<float>();
    }

    private void FixedUpdate()
    {
        m_isGrounded = ComputeIsStandingOn("Solid");
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

        if (m_xAxisInput != 0)
        {
            xVelocity = Mathf.MoveTowards(m_rb.velocity.x, m_maxSpeed * m_xAxisInput, m_acceleration * Time.deltaTime);
        }
        else
        {
            xVelocity = Mathf.MoveTowards(m_rb.velocity.x, 0, m_deceleration * Time.deltaTime);
        }
        m_rb.velocity = new Vector2(xVelocity, m_rb.velocity.y);
    }

    private void ComputeYVelocity()
    {
        // Up / Down acceleration
        if (m_rb.velocity.y > 0 && m_jumpInput == 0 && m_rb.gravityScale <= 1) m_rb.gravityScale += m_lowJumpModifier;
        if (m_rb.velocity.y < 0 && m_rb.gravityScale <= 1.0f + m_lowJumpModifier) m_rb.gravityScale += m_fallModifier;
    }

    private bool ComputeIsStandingOn(string tag)
    {
        // Make three raycasts for more accuracy
        float epsilon = 0.0625f;
        Vector3 offset = new Vector3(GetComponent<BoxCollider2D>().offset.x, GetComponent<BoxCollider2D>().offset.y);
        Ray ray = new(transform.position - new Vector3(0, 0.46875f) + offset, Vector3.down);
        RaycastHit2D[] isMiddleTouching = Physics2D.RaycastAll(ray.origin, ray.direction, epsilon);
        RaycastHit2D[] isLeftTouching = Physics2D.RaycastAll(ray.origin - new Vector3(0.3125f - epsilon, 0), ray.direction, epsilon);
        RaycastHit2D[] isRightTouching = Physics2D.RaycastAll(ray.origin + new Vector3(0.3125f - epsilon, 0), ray.direction, epsilon);

        // Check if raycasts collide with ground
        foreach (RaycastHit2D collision in isMiddleTouching)
        {
            if (collision.collider.tag.Equals("Solid")) return true;
        }
        foreach (RaycastHit2D collision in isLeftTouching)
        {
            if (collision.collider.tag.Equals("Solid")) return true;
        }
        foreach (RaycastHit2D collision in isRightTouching)
        {
            if (collision.collider.tag.Equals("Solid")) return true;
        }

        return false;
    }

    private void Jump()
    {
        m_rb.velocity = new Vector2(m_rb.velocity.x, 0);
        m_rb.AddForce(new Vector2(0f, m_jumpForce), ForceMode2D.Impulse);
    }
}
