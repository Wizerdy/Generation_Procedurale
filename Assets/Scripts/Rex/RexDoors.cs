using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RexDoors : MonoBehaviour {
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
        ChangeStateNoSide();
        RaycastHit2D[] hit = Physics2D.BoxCastAll(transform.position, Vector2.one * 2, 0, Vector2.zero);
        List<RexDoors> doors = new List<RexDoors>();
        foreach (var item in hit) {
            RexDoors doorNext = item.collider.gameObject.GetComponent<RexDoors>();
            if (null != doorNext && doorNext != this && !doors.Contains(doorNext)) {
                doorNext.ChangeStateNoSide();
                doors.Add(doorNext);
            }
        }
    }

    public void ChangeStateNoSide() {
        Debug.Log(gameObject.name + "  " + _isOpen);
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
