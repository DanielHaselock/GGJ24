using UnityEngine;

public class PlayerCustomization : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController[] m_clownAnimators;
    [SerializeField] private Texture2D[] m_palettes;
    public SpriteRenderer m_spriteRenderer{ get; set; }

    public int animIndex{ get; private set; }

    public int texIndex{ get; private set; }

    public Material material{ get; private set; }

    public Texture2D palette{ get; private set; }

    public void Randomize()
    {
        Animator animator = GetComponent<Animator>();

        m_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        if (m_clownAnimators.Length > 0)
        {
            animIndex = Random.Range(0, m_clownAnimators.Length);
            animator.runtimeAnimatorController = m_clownAnimators[animIndex];
        }

        if (m_palettes.Length > 0 && m_spriteRenderer != null)
        {
            texIndex = Random.Range(0, m_palettes.Length);
            palette = m_palettes[texIndex];

            material = new Material(m_spriteRenderer.material); // Avoid modifying shared material
            material.SetTexture("_GradientTexture", palette);

            m_spriteRenderer.enabled = false;
            m_spriteRenderer.material = material;
            m_spriteRenderer.enabled = true;
        }
    }
}

