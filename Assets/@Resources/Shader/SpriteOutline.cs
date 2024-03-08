using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


[ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour
{

    [Range(0, 16)]
    private int outlineSize = 1;
    private Color color = Color.green;
    private bool IsDraw = false;

    private SpriteRenderer spriteRenderer;
    private Coroutine coroutine;

    void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //UpdateOutline(true);
    }

    //void OnDisable()
    //{
    //    UpdateOutline(false);
    //}

    //void Update()
    //{
    //    UpdateOutline(true);
    //}

    void UpdateOutline(bool outline)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        if (spriteRenderer != null)
            spriteRenderer.GetPropertyBlock(mpb);

        mpb.SetFloat("_Outline", outline ? 1f : 0);
        mpb.SetColor("_OutlineColor", color);
        mpb.SetFloat("_OutlineSize", outlineSize);

        if (spriteRenderer != null)
            spriteRenderer.SetPropertyBlock(mpb);
    }

    public void DrawOutline()
    {
        if (!IsDraw)
        {
            coroutine = StartCoroutine(UpdateOutline());
            IsDraw = true;
        }
    }

    public void UndrawOutline()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
            UpdateOutline(false);
            coroutine = null;
            IsDraw = false;
        }
    }

    public IEnumerator UpdateOutline()
    {
        while (true)
        {
            UpdateOutline(true);
            yield return null;
        }
    }
}