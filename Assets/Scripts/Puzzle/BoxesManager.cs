using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxesManager : MonoBehaviour
{
    private List<Boxe> boxes = new List<Boxe>();
    [SerializeField] List<RexDoors> _doors = new List<RexDoors>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform) {
            boxes.Add(child.GetComponent<Boxe>());
        }

        foreach (var boxe in boxes) {
            boxe.SetManager(this);
        }

        SetWinningBoxe();
    }

    private void SetWinningBoxe() {
        boxes[Random.Range(0, boxes.Count)].isWinningBoxe = true;
    }
    public void OnWinningBoxeDestroy() {
        SwitchDoor();

        Debug.Log("BOX PUZZLE WIN");
    }

    public void SwitchDoor() {
        for (int i = 0; i < _doors.Count; i++) {
            _doors[i].ChangeState();
        }
    }

    public void AddDoor(RexDoors door) {
        if (!_doors.Contains(door)) {
            _doors.Add(door);
        }
    }

    public void RemoveDoor(RexDoors door) {
        if (_doors.Contains(door)) {
            _doors.Remove(door);
        }
    }
}
