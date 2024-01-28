using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustomization : MonoBehaviour
{
    [SerializeField] Texture2D[] m_palettes;
    [SerializeField] SpriteRenderer m_spriteRenderer;

    public void Randomize()
    {
        Material material = m_spriteRenderer.material;
        m_spriteRenderer.enabled = false;
        m_spriteRenderer.material = null;

        int random = Random.Range(0, m_palettes.Length);
        material.SetTexture("_GradientTexture", m_palettes[random]);

        m_spriteRenderer.materials = new Material[1] { material };
        m_spriteRenderer.enabled = true;
    }
}
