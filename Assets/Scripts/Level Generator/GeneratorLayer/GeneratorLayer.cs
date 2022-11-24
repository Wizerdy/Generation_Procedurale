using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GeneratorLayer : ScriptableObject {
    public abstract Dictionary<Vector2Int, RoomData> Generate(in Dictionary<Vector2Int, RoomData> currentFloor);
}
