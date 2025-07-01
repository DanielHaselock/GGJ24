using UnityEngine;
using System.Collections.Generic;
public class generalPlayerCustomization : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController[] m_clownAnimators;
    [SerializeField] private Texture2D[] m_palettes;

    private List<RuntimeAnimatorController> m_clownAnimatorsList;
    private List<Texture2D> m_palettesList;

    private void Start()
    {
        m_clownAnimatorsList = new List<RuntimeAnimatorController>(m_clownAnimators);
        m_palettesList = new List<Texture2D>(m_palettes);
    }

    public RuntimeAnimatorController getAnim()
    {
        if (m_clownAnimatorsList.Count == 0)
            resetAnimList();

        int index = Random.Range(0, m_clownAnimatorsList.Count);
        RuntimeAnimatorController controller = m_clownAnimatorsList[index];
        m_clownAnimatorsList.Remove(controller);

        return controller;
    }

    public Texture2D getPalette()
    {
        if (m_palettesList.Count == 0)
            resetPaletteList();

        int index = Random.Range(0, m_palettesList.Count);
        Texture2D palette = m_palettesList[index];
        m_palettesList.Remove(palette);

        return palette;
    }

    private void resetAnimList()
    {
        m_clownAnimatorsList = new List<RuntimeAnimatorController>(m_clownAnimators);
    }

    private void resetPaletteList()
    {
        m_palettesList = new List<Texture2D>(m_palettes);
    }

}
