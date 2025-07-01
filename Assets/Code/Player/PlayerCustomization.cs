using UnityEngine;

public class PlayerCustomization : MonoBehaviour
{
    public SpriteRenderer m_spriteRenderer{ get; set; }

    public int animIndex{ get; private set; }

    public int texIndex{ get; private set; }

    public Material material{ get; private set; }

    public Texture2D palette{ get; private set; }

    public void Randomize()
    {
        Animator animator = GetComponent<Animator>();

        m_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        generalPlayerCustomization customizationManager = FindFirstObjectByType<generalPlayerCustomization>();

        animator.runtimeAnimatorController = customizationManager.getAnim();

        if (m_spriteRenderer != null)
        {
            palette = customizationManager.getPalette();

            material = new Material(m_spriteRenderer.material); // Avoid modifying shared material
            material.SetTexture("_GradientTexture", palette);

            m_spriteRenderer.enabled = false;
            m_spriteRenderer.material = material;
            m_spriteRenderer.enabled = true;
        }
    }
}

