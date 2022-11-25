using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType {
    NONE, RIDDLE, SECRET, OBJECT, EMPTY_OBJECT
}

[System.Serializable]
public class RoomData {
    [SerializeField] RoomType _type;
    [SerializeField, HideInInspector] Gates _gates;
    [SerializeField, HideInInspector] Gates _blockingGates;
    [SerializeField, HideInInspector] Gates _leverGates;
    [SerializeField, HideInInspector] Gates _secretGates;

    public int DoorCount => _gates.Count;
    public RoomType Type { get => _type; set => _type = value; }
    public Gates Gates { get => _gates; set => _gates = value; }
    public Gates BlockingGates { get => _blockingGates; set => _blockingGates = value; }
    public Gates LeverGates { get => _leverGates; set => _leverGates = value; }
    public Gates SecretGates { get => _secretGates; set => _secretGates = value; }

    public RoomData() {
        _type = RoomType.NONE;
        _gates = new Gates();
        _blockingGates = new Gates();
        _leverGates = new Gates();
    }

    public RoomData(RoomData copy) {
        _type = copy._type;
        _gates = copy._gates;
        _blockingGates = copy._blockingGates;
        _leverGates = new Gates();
        _secretGates = new Gates();
    }

    public RoomData(Gates gate) {
        _type = RoomType.NONE;
        _gates = new Gates(gate);
        _blockingGates = new Gates();
        _leverGates = new Gates();
        _secretGates = new Gates();
    }
}