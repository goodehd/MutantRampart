using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BatPoint : MonoBehaviour
{
    private List<Tilemap> _batTiles = new List<Tilemap>();
    private Color _unableBatcolor = Color.red;
    private Color _ableBatcolor = Color.white;

    void Start()
    {
        foreach(Transform t in transform)
        {
            _batTiles.Add(t.GetComponent<Tilemap>());
        }

        gameObject.SetActive(false);
    }

    public void SetSlotColor(int maxUnitCoutn)
    {
        foreach (Tilemap tile in _batTiles)
        {
            tile.color = _unableBatcolor;
        }

        for(int i = 0; i < maxUnitCoutn; i++)
        {
            _batTiles[i].color = _ableBatcolor;
        }
    }
}
