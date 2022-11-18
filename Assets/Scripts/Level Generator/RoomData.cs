using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour {
    public Gates gates;

    public int DoorCount => gates.Count;
}