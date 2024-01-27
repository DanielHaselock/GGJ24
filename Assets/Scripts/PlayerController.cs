using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int m_maxSpeed, m_acceleration, m_deceleration, m_jumpForce, m_lowJumpModifier, m_fallModifier;
    private bool m_isGrounded;
    private Rigidbody2D m_rb;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    virtual protected void FixedUpdate()
    {
        m_isGrounded = ComputeIsStandingOn("Solid");
        //if (Controls.CurrentController.GetAButton() && IsGrounded && !isClimbing) Jump();
        ComputeVelocity();
    }

    protected void ComputeVelocity()
    {
        ComputeXVelocity();
        ComputeYVelocity();
    }

    virtual protected void ComputeXVelocity()
    {
        // Left / Right acceleration
        //float input = Controls.CurrentController.GetHorizontalAxis();
        float input = 0;
        float xVelocity;
        if (input != 0)
        {
            xVelocity = Mathf.MoveTowards(m_rb.velocity.x, m_maxSpeed * input, m_acceleration * Time.deltaTime);
        }
        else
        {
            xVelocity = Mathf.MoveTowards(m_rb.velocity.x, 0, m_deceleration * Time.deltaTime);
        }
        m_rb.velocity = new Vector2(xVelocity, m_rb.velocity.y);
    }

    virtual protected void ComputeYVelocity()
    {
        // Up / Down acceleration
        if (m_rb.velocity.y > 0 && /*Controls.CurrentController.GetAButton()*/ m_rb.gravityScale <= 1) m_rb.gravityScale += m_lowJumpModifier;
        if (m_rb.velocity.y < 0 && m_rb.gravityScale <= 1.2f) m_rb.gravityScale += m_fallModifier;
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
}
