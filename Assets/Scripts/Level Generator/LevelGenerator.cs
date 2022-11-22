using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
    [SerializeField] List<RoomData> _rooms;
    [SerializeField] List<RoomData> _startingRooms;

    [Header("Weights")]
    [SerializeField] string _seed = "";
    [SerializeField] int _maxIteration = 200;
    [SerializeField] int _floorSize = 20;
    [SerializeField, Range(0f, 1f)] float _oneDoor = 0.25f;
    [SerializeField, Range(0f, 1f)] float _twoDoor = 0.25f;
    [SerializeField, Range(0f, 1f)] float _threeDoor = 0.25f;
    [SerializeField, Range(0f, 1f)] float _fourDoor = 0.25f;

    [Header("System")]
    [SerializeField] Vector2 _roomSize = new Vector2Int(4, 2);
    [SerializeField] Color _wayColor = Color.red;
    [SerializeField] Color _wallColor = Color.yellow;

    Dictionary<Vector2Int, RoomData> _floor;
    Vector2Int _startPosition;

    int _currentSeed = 0;

    private void Start() {
        _startPosition = Vector2Int.zero;
        _floor = GenerateFloor(Vector2Int.zero);
        //StartCoroutine(GenerateFloor(Vector2Int.zero));
    }

    public void GenerateTheFloor() {
        _startPosition = Vector2Int.zero;
        _floor = GenerateFloor(Vector2Int.zero);
    }

    //public Dictionary<Vector2Int, RoomData> GenerateFloor(Vector2Int startPosition) {
    public Dictionary<Vector2Int, RoomData> GenerateFloor(Vector2Int startPosition) {
        if (_seed == "") {
            _currentSeed = Random.Range(0, int.MaxValue);
        } else {
            try {
                System.Int32.TryParse(_seed, out _currentSeed);
            } catch (System.FormatException e) {
                _currentSeed = _seed.ToCharArray().Aggregate((char char1, char char2) => (char)((int)char1 + (int)char2));
            }
        }
        Random.InitState(_currentSeed);

        Dictionary<Vector2Int, RoomData> output = new Dictionary<Vector2Int, RoomData>();
        List<(Vector2Int, Gate)> toSpawn = new List<(Vector2Int, Gate)>();

        int open = 0;

        // Start
        Vector2Int currentPosition = startPosition;

        RoomData startRoom = _startingRooms[Random.Range(0, _startingRooms.Count)];

        open += AddRoom(ref output, ref toSpawn, startPosition, startRoom);

        //Debug.Log("First : " + startRoom.name + " g:" + startRoom.gates + " td:" + toSpawn.Count);
        //DrawRoom(startRoom, startPosition, Color.red, Color.yellow, 1000f);

        int debugCount = 0;

        while (toSpawn.Count > 0 && debugCount < _maxIteration) {
            // Find Next Room
            (Vector2Int, Gate) next = toSpawn[0];
            toSpawn.RemoveAt(0);
            currentPosition = next.Item1;

            int nextDoors = Tools.Ponder(_oneDoor, _twoDoor, _threeDoor, _fourDoor) + 1;
            Gates meanFriends = AskNeighborhood(in output, currentPosition, false);
            Gates happyFriends = AskNeighborhood(in output, currentPosition, true);
            //Debug.Log("Searching for : " + next.Item2 + " / " + nextDoors);
            if (nextDoors > 4 - meanFriends.Count) {
                nextDoors = 4 - meanFriends.Count;
            }

            List<RoomData> nextRooms = FindMatchingRoom(_rooms, 
                (RoomData data) => data.gates[next.Item2] &&
                    data.DoorCount == nextDoors &&
                    (data.gates.Value & meanFriends.Value) == 0 &&
                    (data.gates.Value.Contains(happyFriends.Value)));

            if (nextRooms.Count == 0) {
                Debug.LogWarning("Skipped : " + next.Item1);
                continue;
            }
            int random = Random.Range(0, nextRooms.Count);
            
            RoomData nextRoom = nextRooms[random];

            --open;
            open += AddRoom(ref output, ref toSpawn, currentPosition, nextRoom);

            //DrawRoom(nextRoom, currentPosition, _wayColor, _wallColor, 1000f);
            //DrawRoom(nextRoom, currentPosition, Color.blue, Color.magenta, Time.deltaTime);
            ++debugCount;
        }

        //foreach (KeyValuePair<Vector2Int, RoomData> room in output) {
        //    DrawRoom(room.Value, room.Key, _wayColor, _wallColor, 1000f);
        //}
        //DrawRoom(startRoom, startPosition, Color.red, Color.yellow, 1000f);
        Debug.Log("End : " + _currentSeed);
        return output;
    }

    public void Clear() {
        _floor.Clear();
    }

    // Poids par nombre de porte
    // Compter les ouvertures
    // Nombre de salle voulue

    private List<RoomData> FindMatchingRoom(List<RoomData> list, Gate gate) {
        return FindMatchingRoom(list, (RoomData data) => data.gates[gate]);
    }

    private List<RoomData> FindMatchingRoom(List<RoomData> list, System.Func<RoomData, bool> predicate) {
        return list.Where(predicate).ToList();
    }

    private Gates AskNeighborhood(in Dictionary<Vector2Int, RoomData> currentFloor, Vector2Int position, bool happy) {
        Gates output = new Gates();
        Gate gate;
        Gate inverseGate;

        for (int i = 0; i < 4; i++) {
            gate = Tools.ToGate(i);
            inverseGate = Tools.ToGate((i + 2) % 4);

            output[gate] = false;
            if (currentFloor.ContainsKey(position + Tools.ToDirection(i)) &&
                (currentFloor[position + Tools.ToDirection(i)].gates[inverseGate] == happy)) {
                output[gate] = true;
            }
        }

        return output;
    }

    private int AddRoom(ref Dictionary<Vector2Int, RoomData> currentFloor, ref List<(Vector2Int, Gate)> toSpawn, Vector2Int position, RoomData room) {
        if (currentFloor.ContainsKey(position)) {
            Debug.LogWarning("Replacing");
            currentFloor[position] = room;
        } else {
            currentFloor.Add(position, room);
        }
        return AddToSpawn(in currentFloor, ref toSpawn, position, room);
    }

    private int AddToSpawn(in Dictionary<Vector2Int, RoomData> floor, ref List<(Vector2Int, Gate)> toSpawn, Vector2Int position, RoomData room) {
        for (int i = 0; i < toSpawn.Count; i++) {
            if (toSpawn[i].Item1 == position) {
                toSpawn.RemoveAt(i);
            }
        }

        int count = 0;
        for (int i = 0; i < 4; i++) {
            //Debug.Log(!floor.ContainsKey(position + Tools.ToDirection(i)) + " .. " + room.gates[Tools.ToGate(i)]);
            if (!floor.ContainsKey(position + Tools.ToDirection(i)) && room.gates[Tools.ToGate(i)]) {
                toSpawn.Add(new (position + Tools.ToDirection(i), Tools.ToGate((i + 2) % 4)));
                ++count;
            }
        }
        return count;
    }

    private void DrawRoom(RoomData room, Vector2Int position, Color wayColor, Color roomColor, float time) {
        for (int i = 0; i < 4; i++) {
            //Debug.Log(i + " .. " + Tools.ToGate(i) + " .. " + room.gates[Tools.ToGate(i)] + " .. " + Tools.ToDirection(i));
            if (room.gates[Tools.ToGate(i)]) {
                Debug.DrawLine((Vector2)position * _roomSize, position * _roomSize + Tools.ToDirection(i) * (_roomSize / 2f), wayColor, time);
            }
        }
        DrawRect((Vector2)position * _roomSize, _roomSize, roomColor, time);
    }

    private void DrawRect(Vector2 center, Vector2 size, Color color, float time) {
        size /= 2f;
        Vector2[] corners = { new Vector2(center.x - size.x, center.y - size.y),
            new Vector2(center.x + size.x, center.y - size.y),
            new Vector2(center.x + size.x, center.y + size.y),
            new Vector2(center.x - size.x, center.y + size.y) };
        Debug.DrawLine(corners[0], corners[1], color, time);
        Debug.DrawLine(corners[1], corners[2], color, time);
        Debug.DrawLine(corners[2], corners[3], color, time);
        Debug.DrawLine(corners[3], corners[0], color, time);
    }

    private void OnDrawGizmos() {
        if (_floor == null) { return; }
        foreach(KeyValuePair<Vector2Int, RoomData> room in _floor) {
            DrawGizmoRoom(room.Value, room.Key, _wayColor, _wallColor);
        }
        if (_floor.ContainsKey(_startPosition)) {
            DrawGizmoRoom(_floor[_startPosition], _startPosition, Color.red, Color.yellow);
        }
    }

    private void DrawGizmoRoom(RoomData room, Vector2Int position, Color wayColor, Color wallColor) {
        Gizmos.color = wayColor;
        for (int i = 0; i < 4; i++) {
            if (room.gates[Tools.ToGate(i)]) {
                Gizmos.DrawLine((Vector2)position * _roomSize, position * _roomSize + Tools.ToDirection(i) * (_roomSize / 2f));
            }
        }
        Gizmos.color = wallColor;
        Gizmos.DrawWireCube(position * _roomSize, _roomSize);
    }
}
