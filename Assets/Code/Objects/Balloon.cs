using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : GenericObstacle
{
    private Rigidbody2D m_rb;
    private BoxCollider2D collider;
    private SpriteRenderer m_sr;
    public Material[] BalloonMaterials;
    [SerializeField] private GameObject confetti;

    protected override void Start()
    {
        base.Start();
        m_rb = GetComponent<Rigidbody2D>();
        m_sr = GetComponent<SpriteRenderer>();
        
        int randomIndex = Random.Range(0, BalloonMaterials.Length);
        m_sr.material = BalloonMaterials[randomIndex];
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.collider.tag.Equals("Player"))
        {
            Instantiate(confetti, transform.position,transform.rotation);
            m_animator.SetTrigger("Pop");
            AudioManager.Instance.Pop();
            m_rb.bodyType = RigidbodyType2D.Static;
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}