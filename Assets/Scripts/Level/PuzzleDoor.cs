using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PuzzleDoor : MonoBehaviour
{
    [SerializeField] Tilemap _tileMap;
    [SerializeField] Tile _openTile;
    [SerializeField] Tile _closedTile;
    [SerializeField] bool _isOpen;
    Collider2D _collider;
    public bool IsOpen => _isOpen;

    private void Start() {
        _collider = GetComponent<Collider2D>();
        PlaceTile();
        UpdateCollider();
    }

    public void ChangeState() {
        _isOpen = !_isOpen;
        PlaceTile();
        UpdateCollider();
    }

    void PlaceTile() {
        _tileMap.SetTile(Vector3Int.zero, _isOpen ? _openTile : _closedTile);
    }

    void UpdateCollider() {
        _collider.enabled = !_isOpen;
    }
}
