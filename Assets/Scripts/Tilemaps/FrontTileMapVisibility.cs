using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
[RequireComponent(typeof(TilemapCollider2D))]
public class FrontTileMapVisibility : MonoBehaviour
{
    private Tilemap tilemap;


    void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null || !collision.transform.parent.CompareTag("Player"))
            return;

        tilemap.color = new Color(tilemap.color.r, tilemap.color.g, tilemap.color.b, 0.5f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null || !collision.transform.parent.CompareTag("Player"))
            return;

        tilemap.color = new Color(tilemap.color.r, tilemap.color.g, tilemap.color.b, 1.0f);
    }
}
