using UnityEngine;

public class PlayerCustomization : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController[] m_clownAnimators;
    [SerializeField] private Texture2D[] m_palettes;
    [SerializeField] private SpriteRenderer m_spriteRenderer;

    public void Randomize()
    {
        Animator animator = GetComponent<Animator>();

        if (m_clownAnimators.Length > 0)
        {
            int animIndex = Random.Range(0, m_clownAnimators.Length);
            animator.runtimeAnimatorController = m_clownAnimators[animIndex];
        }

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
}

