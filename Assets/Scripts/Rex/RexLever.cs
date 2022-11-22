using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RexLever : MonoBehaviour {
    [SerializeField] List<RexDoors> _doors = new List<RexDoors>();
    public bool a;

    private void Update() {
        if (a) {
            a = false;
            SwitchDoor();
        }
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
