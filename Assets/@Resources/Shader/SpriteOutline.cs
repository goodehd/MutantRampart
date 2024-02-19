using UnityEngine;
using UnityEngine.Tilemaps;


[ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour
{
    public Color color = Color.white;

    [Range(0, 16)]
    public int outlineSize = 1;

    private SpriteRenderer spriteRenderer;
    private TilemapRenderer tilemapRenderer;

    void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        tilemapRenderer = GetComponent<TilemapRenderer>();

        UpdateOutline(true);
    }

    void OnDisable()
    {
        UpdateOutline(false);
    }

    void Update()
    {
        UpdateOutline(true);
    }

    void UpdateOutline(bool outline)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        if(spriteRenderer != null)
            spriteRenderer.GetPropertyBlock(mpb);

        if(tilemapRenderer != null)
            tilemapRenderer.GetPropertyBlock(mpb);

        mpb.SetFloat("_Outline", outline ? 1f : 0);
        mpb.SetColor("_OutlineColor", color);
        mpb.SetFloat("_OutlineSize", outlineSize);

        if (spriteRenderer != null)
            spriteRenderer.SetPropertyBlock(mpb);

        if (tilemapRenderer != null)
            tilemapRenderer.SetPropertyBlock(mpb);
    }
}