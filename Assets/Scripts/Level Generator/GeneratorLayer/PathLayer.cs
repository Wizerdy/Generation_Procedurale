using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathLayer : GeneratorLayer {
    [SerializeField] Vector2Int _startPosition = Vector2Int.zero;
    [SerializeField] int _floorSize = 20;
    [SerializeField, Range(0f, 1f)] float[] _doors = new float[4];

    Gates[] _allGates;

    public override Dictionary<Vector2Int, RoomData> Generate(in Dictionary<Vector2Int, RoomData> currentFloor) {
        return Generate(currentFloor, _startPosition, _floorSize, _doors);
    }

    private void Init() {
        if (_allGates != null && _allGates.Length > 0) { return; }
        _allGates = new Gates[16];
        for (int i = 0; i < _allGates.Length; i++) {
            _allGates[i] = new Gates((uint)i);
        }
    }

    private Dictionary<Vector2Int, RoomData> Generate(in Dictionary<Vector2Int, RoomData> currentFloor, Vector2Int startPosition, float size, float[] doors) {
        Init();

        List<(Vector2Int, Gate)> toSpawn = new List<(Vector2Int, Gate)>();
        Vector2Int currentPosition = startPosition;
        int open = 0;

        Dictionary<Vector2Int, RoomData> floor = new Dictionary<Vector2Int, RoomData>(currentFloor);

        if (floor == null) {
            floor = new Dictionary<Vector2Int, RoomData>();
        }

        RoomData startRoom = null;
        if (!floor.ContainsKey(startPosition)) {
            // Start Room
            //startRoom = new RoomData(_startingRooms[Random.Range(0, _startingRooms.Count)]);
        } else {
            startRoom = new RoomData(floor[startPosition]);
            floor.Remove(startPosition);
            Gate newGate = FreePlace(floor, startPosition).Random(true);
            startRoom.Gates[newGate] = true;
            startRoom.BlockingGates[newGate] = true;
        }

        open += AddRoom(ref floor, ref toSpawn, startPosition, startRoom);

        int debugCount = 0;

        while (toSpawn.Count > 0 && (size > 0 ? floor.Count < size : true)) {
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

        return floor;
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
}
