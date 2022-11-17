using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
    [SerializeField] List<Room> _rooms;
    [SerializeField] List<Room> _startingRooms;

    public Dictionary<Vector2Int, Room> GenerateFloor(Vector2Int startPosition) {
        Dictionary<Vector2Int, Room> output = new Dictionary<Vector2Int, Room>();

        Room start = _startingRooms[Random.Range(0, _startingRooms.Count)];
        start.gates = 1;
        output.Add(startPosition, _startingRooms[Random.Range(0, _startingRooms.Count)]);

        return output;
    }

    //private Room FindMatchingRoom(int gate) {

    //}
}
