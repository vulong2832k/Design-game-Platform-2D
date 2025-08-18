using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DeleteTilePalette : MonoBehaviour
{
    [Header("Tilemap settings")]
    [SerializeField] private List<Tilemap> _tilesmapsToClear;
    [SerializeField] private Vector2Int _areaStart;
    [SerializeField] private Vector2Int _areaSize;

    private bool _isTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isTriggered || !collision.CompareTag("Player")) return;

        ClearTiles();
        _isTriggered = true;
    }

    private void ClearTiles()
    {
        foreach (var tilemap in _tilesmapsToClear)
        {
            for (int x = _areaSize.x - 1; x >= 0; x--)
            {
                for (int y = _areaSize.y - 1; y >= 0; y--)
                {
                    Vector3Int pos = new Vector3Int(_areaStart.x - x, _areaStart.y - y, 0);
                    Debug.Log($"Xóa tile tại: {pos} trong tilemap: {tilemap.name}");
                    tilemap.SetTile(pos, null);
                }
            }
        }
    }
}
