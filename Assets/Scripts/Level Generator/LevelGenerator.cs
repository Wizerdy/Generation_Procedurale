using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
    [SerializeField] List<RoomData> _rooms;
    [SerializeField] List<RoomData> _startingRooms;

    public Dictionary<Vector2Int, RoomData> GenerateFloor(Vector2Int startPosition) {
        Dictionary<Vector2Int, RoomData> output = new Dictionary<Vector2Int, RoomData>();



        RoomData start = _startingRooms[Random.Range(0, _startingRooms.Count)];
        start.gates = 1;
        output.Add(startPosition, _startingRooms[Random.Range(0, _startingRooms.Count)]);

        return output;
    }

    // Poids par nombre de porte
    // Compter les ouvertures
    // Nombre de salle voulue

    //private Room FindMatchingRoom(int gate) {

    //}
}
