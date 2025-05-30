using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : GenericObstacle
{
    private Rigidbody2D m_rb;
    private BoxCollider2D collider;
    [SerializeField] private Texture2D[] m_palettes;
    [SerializeField] private SpriteRenderer m_spriteRenderer;
    [SerializeField] private GameObject confetti;
    
    private AudioSource audioSource;

    [SerializeField] private AudioClip popSound;

    protected override void Start()
    {
        base.Start();
        m_rb = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = popSound;

        // randomize the balloon's colors
        if (m_palettes.Length > 0 && m_spriteRenderer != null)
        {
            int texIndex = Random.Range(0, m_palettes.Length);
            Texture2D palette = m_palettes[texIndex];

            Material material = new Material(m_spriteRenderer.material); // Avoid modifying shared material
            material.SetTexture("_GradientTexture", palette);

            m_spriteRenderer.enabled = false;
            m_spriteRenderer.material = material;
            m_spriteRenderer.enabled = true;
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.collider.tag.Equals("Player"))
        {
            Instantiate(confetti, transform.position,transform.rotation);
            m_animator.SetTrigger("Pop");
            StartCoroutine(Pop());
            m_rb.bodyType = RigidbodyType2D.Static;
        }
    }

    IEnumerator Pop()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(gameObject);
    }
}