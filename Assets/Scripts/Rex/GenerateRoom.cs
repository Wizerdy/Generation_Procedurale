using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRoom : MonoBehaviour {
    public struct RoomData {
        public Vector2 Size;
        public int Doors;
    }

    [SerializeField] RoomData _data;
    public RoomData Data => _data;
}
