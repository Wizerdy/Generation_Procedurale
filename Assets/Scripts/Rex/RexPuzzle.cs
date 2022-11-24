using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RexPuzzle : MonoBehaviour {
    [SerializeField] protected List<RexDoors> _doors = new List<RexDoors>();

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
