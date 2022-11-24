using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour {
    [SerializeField] GameObject _objectRoom;
    [SerializeField] GameObject _riddleRoom;
    [SerializeField] GameObject _secretRoom;
    [SerializeField] GameObject _wallPrefab;
    [SerializeField] RexLever _leverPrefab;
    [SerializeField] RexDoors _doorsPrefab;
    [SerializeField] RexDoors _blockingDoorsPrefab;
    [SerializeField] Grid _grid;
    [SerializeField] List<RoomData> _rooms;
    //[SerializeField] List<RoomData> _startingRooms;
    [SerializeField] List<Gates> _startingRooms;

    [Header("Weights")]
    [SerializeField] string _seed = "";
    [SerializeField] int _floorSize = 20;
    [SerializeField, Range(0f, 1f)] float[] _doors = new float[4];
    [SerializeField] int _distSecretRoomFromStart = 2;

    [Header("System")]
    [SerializeField] int _maxIteration = 1000;
    [SerializeField] bool _closeDoor = false;
    [SerializeField] bool _instantiate = false;
    [SerializeField] Vector2 _roomSize = new Vector2Int(4, 2);
    [SerializeField] Color _wayColor = Color.blue;
    [SerializeField] Color _blockedWayColor = Color.red;
    [SerializeField] Color _leverRoomColor = Color.blue;
    [SerializeField] Color _secretRoomColor = Color.black;
    [SerializeField] Color _objectRoomColor = Color.cyan;
    [SerializeField] Color _wallColor = Color.white;

    Dictionary<Vector2Int, RoomData> _floor = new Dictionary<Vector2Int, RoomData>();
    Vector2Int _startPosition;

    int _currentSeed = -1;

    Gates[] _allGates;

    private void Init() {
        if (_allGates != null && _allGates.Length > 0) { return; }
        _allGates = new Gates[16];
        for (int i = 0; i < _allGates.Length; i++) {
            _allGates[i] = new Gates((uint)i);
        }
    }

    private void Start() {
        GenerateTheFloor(Vector2Int.zero);
    }

    public void GenerateTheFloor(Vector2Int position) {
        Clear();
        _currentSeed = -1;

        _startPosition = position;
        try {
            _floor = new Dictionary<Vector2Int, RoomData>();
            int randomFloorSize = Random.Range(_floorSize/2, _floorSize/4 * 3);
            _floor = GenerateFloor(_startPosition, randomFloorSize, _floor);

            Vector2Int farest = Farest(_floor, _startPosition);

            Vector2Int? secretSanta = PutSecretRoom(ref _floor, _startPosition);
            if (secretSanta == null) {
                throw new GenerationException("Can't spawn secret room");
            }

            Dictionary<Vector2Int, RoomData> otherFloor;
            otherFloor = GenerateFloor(farest, _floorSize, _floor);
            _floor = otherFloor;
        } catch (GenerationException e) {
            Debug.LogWarning("Rebuilding! (Cause:" + e + ")");
            GenerateTheFloor(position);
        }

        Debug.Log("End ! Seed:" + _currentSeed + " Size:" + _floor.Count);
    }

    public Dictionary<Vector2Int, RoomData> GenerateFloor(Vector2Int startPosition, int size, in Dictionary<Vector2Int, RoomData> currentFloor = null) {
        Init();

        LoadSeed(_seed);

        List<(Vector2Int, Gate)> toSpawn = new List<(Vector2Int, Gate)>();
        Vector2Int currentPosition = startPosition;
        int open = 0;

        Dictionary<Vector2Int, RoomData> floor = new Dictionary<Vector2Int, RoomData>(currentFloor);

        if (floor == null) {
            floor = new Dictionary<Vector2Int, RoomData>();
        }

        RoomData startRoom;
        if (!floor.ContainsKey(startPosition)) {
            // Start Room
            startRoom = new RoomData(_startingRooms[Random.Range(0, _startingRooms.Count)]);
        } else {
            startRoom = new RoomData(floor[startPosition]);
            floor.Remove(startPosition);
            Gate newGate = FreePlace(floor, startPosition).Random(true);
            startRoom.Gates[newGate] = true;
            startRoom.BlockingGates[newGate] = true;
        }

        open += AddRoom(ref floor, ref toSpawn, startPosition, startRoom);

        int debugCount = 0;

        while (toSpawn.Count > 0 && (size > 0 ? floor.Count < size : true) && debugCount < _maxIteration) {
            ++debugCount;

            // Find Next Room
            (Vector2Int, Gate) next = toSpawn[0];
            toSpawn.RemoveAt(0);
            currentPosition = next.Item1;

            Vector2Int bounds = new Vector2Int(0, 4);

            Gates meanFriends = AskNeighborhood(in floor, currentPosition, false);
            Gates happyFriends = AskNeighborhood(in floor, currentPosition, true);

            bounds.y -= meanFriends.Count;
            bounds.x = happyFriends.Count;

            List<float> toPonder = new List<float>();
            for (int i = bounds.x; i < bounds.y; i++) {
                toPonder.Add(_doors[i]);
            }
            int nextDoors = Tools.Ponder(toPonder);
            nextDoors = bounds.x + nextDoors + 1;

            List<Gates> nextGates = _allGates.Where((Gates gate) => gate[next.Item2] &&
                    gate.Count == nextDoors &&
                    (gate.Value & meanFriends.Value) == 0 &&
                    (gate.Value.Contains(happyFriends.Value))).ToList();

            if (nextGates.Count == 0) {
                Debug.LogWarning("Skipped : " + next.Item1 +
                    " e:" + next.Item2 +
                    " dc:" + nextDoors +
                    " m:" + meanFriends.Value +
                    " f:" + happyFriends.Value);
                continue;
            }
            int random = Random.Range(0, nextGates.Count);

            RoomData nextRoom = new RoomData(nextGates[random]);

            --open;
            open += AddRoom(ref floor, ref toSpawn, currentPosition, nextRoom);
        }

        if (_closeDoor) {
            CloseFloor(ref floor, in toSpawn);
        }

        //Debug.Log("End:" + _currentSeed + " Size:" + floor.Count);
        if (_instantiate)
            InstantiateLevel(in floor);
        return floor;
    }

    private void CloseFloor(ref Dictionary<Vector2Int, RoomData> floor, in List<(Vector2Int, Gate)> toSpawn) {
        for (int i = 0; i < toSpawn.Count; i++) {
            (Vector2Int, Gate) toClose = new(toSpawn[i].Item1 + Tools.ToDirection(toSpawn[i].Item2), toSpawn[i].Item2.Inverse());
            floor[toClose.Item1].Gates[toClose.Item2] = false;
        }
    }

    public void Clear() {
        _floor.Clear();
        for (int i = _grid.transform.childCount; i > 0; i--) {
            DestroyImmediate(_grid.transform.GetChild(i - 1).gameObject);
        }
    }

    private List<Gates> FindMatchingRoom(Gate gate) {
        return FindMatchingRoom((Gates data) => data[gate]);
    }

    private List<Gates> FindMatchingRoom(System.Func<Gates, bool> predicate) {
        return _allGates.Where(predicate).ToList();
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
                (currentFloor[position + Tools.ToDirection(i)].Gates[inverseGate] == happy)) {
                output[gate] = true;
            }
        }

        return output;
    }

    private Gates FreePlace(in Dictionary<Vector2Int, RoomData> currentFloor, Vector2Int position) {
        Gates output = new Gates();
        Gate gate;

        for (int i = 0; i < 4; i++) {
            gate = Tools.ToGate(i);

            output[gate] = false;
            if (!currentFloor.ContainsKey(position + Tools.ToDirection(i))) {
                output[gate] = true;
            }
        }

        return output;
    }

    private int AddRoom(ref Dictionary<Vector2Int, RoomData> currentFloor, ref List<(Vector2Int, Gate)> toSpawn, Vector2Int position, RoomData room) {
        if (currentFloor.ContainsKey(position)) {
            Debug.LogWarning("Replacing " + room + " p:" + position);
            currentFloor[position] = room;
        } else {
            currentFloor.Add(position, room);
        }
        return AddToSpawn(in currentFloor, ref toSpawn, position, room);
    }

    private int AddToSpawn(in Dictionary<Vector2Int, RoomData> floor, ref List<(Vector2Int, Gate)> toSpawn, Vector2Int position, RoomData room) {
        for (int i = toSpawn.Count - 1; i > -1; --i) {
            if (toSpawn[i].Item1 == position) {
                toSpawn.RemoveAt(i);
            }
        }

        int count = 0;
        for (int i = 0; i < 4; i++) {
            //Debug.Log(!floor.ContainsKey(position + Tools.ToDirection(i)) + " .. " + room.gates[Tools.ToGate(i)]);
            if (!floor.ContainsKey(position + Tools.ToDirection(i)) && room.Gates[Tools.ToGate(i)]) {
                toSpawn.Add(new(position + Tools.ToDirection(i), Tools.ToGate((i + 2) % 4)));
                ++count;
            }
        }
        return count;
    }

    Vector2Int Farest(in Dictionary<Vector2Int, RoomData> floor, Vector2Int startPosition) {
        int max = 0;
        List<Vector2Int> farest = new List<Vector2Int>();

        foreach (KeyValuePair<Vector2Int, RoomData> tiles in floor) {
            int distance = RoomDistance(startPosition, tiles.Key);
            if (max < distance) {
                farest.Clear();
                max = distance;
                farest.Add(tiles.Key);
            } else if (max == distance) {
                farest.Add(tiles.Key);
            }
        }

        return farest[Random.Range(0, farest.Count)];
    }

    int RoomDistance(Vector2Int dist1, Vector2Int dist2) {
        return Mathf.Abs(dist1.x - dist2.x) + Mathf.Abs(dist1.y - dist2.y);
    }

    bool AtLeastOneSpace(ref Dictionary<Vector2Int, RoomData> currentFloor, Vector2Int roomPos) {
        for (int i = 0; i < 4; i++) {
            if (!currentFloor.ContainsKey(roomPos + Tools.ToDirection(i))) {
                return true;
            }
        }
        return false;
    }

    Vector2Int? PutSecretRoom(ref Dictionary<Vector2Int, RoomData> currentFloor, Vector2Int startPosition) {
        (Vector2Int, RoomData) emptyRoom = new(Vector2Int.zero, null);
        foreach (var item in currentFloor) {
            if ((item.Value.DoorCount == 2 || item.Value.DoorCount == 3)
                && RoomDistance(item.Key, startPosition) >= _distSecretRoomFromStart
                && AtLeastOneSpace(ref currentFloor, item.Key)) {
                emptyRoom = new(item.Key, item.Value);
                break;
            }
        }

        if (emptyRoom.Item2 == null) { return null; }

        emptyRoom.Item2.Type = RoomType.OBJECT;

        Vector2Int? output = null;
        for (int i = 0; i < 4; i++) {
            Vector2Int position = emptyRoom.Item1 + Tools.ToDirection(i);
            emptyRoom.Item2.Gates[Tools.ToGate(i)] = true;
            if (_floor.ContainsKey(position)) {
                currentFloor[emptyRoom.Item1 + Tools.ToDirection(i)].Gates[Tools.ToGate((i + 2) % 4)] = true;
            } else {
                RoomData newRoom = new RoomData(FindMatchingRoom((Gates room) => room[Tools.ToGate((i + 2) % 4)] && room.Count == 1)[0]);
                currentFloor.Add(emptyRoom.Item1 + Tools.ToDirection(i), newRoom);
                if (output == null) {
                    newRoom.Type = RoomType.SECRET;
                    output = emptyRoom.Item1 + Tools.ToDirection(i);
                } else {
                    newRoom.Type = RoomType.RIDDLE;
                }
            }
        }
        return output;
    }

    private void DrawRoom(RoomData room, Vector2Int position, Color wayColor, Color roomColor, float time) {
        for (int i = 0; i < 4; i++) {
            //Debug.Log(i + " .. " + Tools.ToGate(i) + " .. " + room.gates[Tools.ToGate(i)] + " .. " + Tools.ToDirection(i));
            if (room.Gates[Tools.ToGate(i)]) {
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
        foreach (KeyValuePair<Vector2Int, RoomData> room in _floor) {
            DrawGizmoRoom(room.Value, room.Key, _wayColor, _blockedWayColor, _wallColor);
        }
        if (_floor.ContainsKey(_startPosition)) {
            DrawGizmoRoom(_floor[_startPosition], _startPosition, Color.red, _blockedWayColor, Color.yellow);
        }
    }

    private void DrawGizmoRoom(RoomData room, Vector2Int position, Color wayColor, Color blockedWayColor, Color wallColor) {
        for (int i = 0; i < 4; i++) {
            if (room.Gates[Tools.ToGate(i)]) {
                Gizmos.color = wayColor;
                if (room.BlockingGates[Tools.ToGate(i)]) {
                    Gizmos.color = blockedWayColor;
                }
                Gizmos.DrawLine((Vector2)position * _roomSize, position * _roomSize + Tools.ToDirection(i) * (_roomSize / 2f));
            }
        }

        switch (room.Type) {
            case RoomType.NONE:
            case RoomType.RIDDLE:
            default:
                Gizmos.color = wallColor;
                break;
            case RoomType.SECRET:
                Gizmos.color = _secretRoomColor;
                break;
            case RoomType.OBJECT:
                Gizmos.color = _objectRoomColor;
                break;
        }
        Gizmos.DrawWireCube(position * _roomSize, _roomSize);
    }

    private void LoadSeed(string seed) {
        if (_currentSeed != -1) { return; }

        if (_seed == "") {
            _currentSeed = Random.Range(0, int.MaxValue);
        } else {
            try {
                bool success = System.Int32.TryParse(seed, out _currentSeed);
                if (!success) { throw new System.FormatException(); }
            } catch (System.FormatException) {
                _currentSeed = seed.ToCharArray().Aggregate((char char1, char char2) => (char)((int)char1 + (int)char2));
            }
        }
        Random.InitState(_currentSeed);
    }

    void InstantiateLevel(in Dictionary<Vector2Int, RoomData> currentFloor) {
        foreach (var room in currentFloor) {
            GameObject newRoom;
            switch (room.Value.Type) {
                default:
                case RoomType.NONE:
                case RoomType.RIDDLE:
                    newRoom = Instantiate(_riddleRoom.gameObject, new Vector3(room.Key.x * _roomSize.x, room.Key.y * _roomSize.y, 0), Quaternion.identity);
                    break;
                case RoomType.SECRET:
                    newRoom = Instantiate(_secretRoom.gameObject, new Vector3(room.Key.x * _roomSize.x, room.Key.y * _roomSize.y, 0), Quaternion.identity);
                    break;
                case RoomType.OBJECT:
                    newRoom = Instantiate(_objectRoom.gameObject, new Vector3(room.Key.x * _roomSize.x, room.Key.y * _roomSize.y, 0), Quaternion.identity);
                    break;
            }
            newRoom.transform.parent = _grid.transform;
            Vector3 adapt = new Vector3(-1, -1, 0);
            for (int i = 0; i < 4; i++) {
                RexDoors door;
                if (room.Value.Gates[Tools.ToGate(i)]) {
                    if (room.Value.BlockingGates[Tools.ToGate(i)]) {
                        door = Instantiate(_blockingDoorsPrefab, newRoom.transform.position + new Vector3(Tools.ToDirection(i).x * (_roomSize.x + adapt.x), Tools.ToDirection(i).y * (_roomSize.y + adapt.y), 0) / 2, Quaternion.identity, newRoom.transform.GetChild(0));
                    } else {
                        door = Instantiate(_doorsPrefab, newRoom.transform.position + new Vector3(Tools.ToDirection(i).x * (_roomSize.x + adapt.x), Tools.ToDirection(i).y * (_roomSize.y + adapt.y), 0) / 2, Quaternion.identity, newRoom.transform.GetChild(0));
                    }
                    if (room.Value.LeverGates[Tools.ToGate(i)]) {
                        RexLever lever = Instantiate(_leverPrefab, door.gameObject.transform.position + Vector3.RotateTowards(new Vector3(Tools.ToDirection(i).x, Tools.ToDirection(i).y, 0), Vector3.back, Mathf.PI / -2, 0) - new Vector3(Tools.ToDirection(i).x, Tools.ToDirection(i).y, 0), Quaternion.identity, newRoom.transform.GetChild(2));
                        lever.AddDoor(door);
                    }
                } else {
                    Instantiate(_wallPrefab, newRoom.transform.position + new Vector3(Tools.ToDirection(i).x * (_roomSize.x + adapt.x), Tools.ToDirection(i).y * (_roomSize.y + adapt.y), 0) / 2, Quaternion.identity, newRoom.transform.GetChild(1));
                }

            }
        }
    }
}


public class GenerationException : System.Exception {
    public GenerationException() : base() { }
    public GenerationException(string message) : base(message) { }
}
