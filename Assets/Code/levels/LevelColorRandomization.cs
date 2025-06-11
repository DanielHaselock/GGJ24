using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelColorRandomization : MonoBehaviour
{
    [SerializeField] private GameObject bg;
    [SerializeField] private GameObject ground;
    [SerializeField] private GameObject platforms;
    [SerializeField] private Material[] bg_palettes;
    [SerializeField] private Material[] level_palettes;
    
    private void Start()
    {
        Tilemap ground_tilemap = ground.GetComponent<Tilemap>();
        Tilemap platforms_tilemap = platforms.GetComponent<Tilemap>();

        SpriteRenderer bg_spriteRenderer = bg.GetComponent<SpriteRenderer>();
        TilemapRenderer ground_tilemapRenderer = ground_tilemap.GetComponent<TilemapRenderer>();
        TilemapRenderer platforms_tilemapRenderer = platforms_tilemap.GetComponent<TilemapRenderer>();

        int randomIndex = Random.Range(0, bg_palettes.Length);
        bg_spriteRenderer.material = bg_palettes[randomIndex];
        ground_tilemapRenderer.material = level_palettes[randomIndex];
        platforms_tilemapRenderer.material = level_palettes[randomIndex];
    }
}

