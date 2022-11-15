using System.Linq;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public DoorController connectedDoor { get; private set; }

    protected virtual void Awake()
    {
        var grid = GetComponentInParent<Grid>();
        var allDoors = grid.GetComponentsInChildren<DoorController>();
        connectedDoor = allDoors.FirstOrDefault(OneTileApartDoor);

        bool OneTileApartDoor(DoorController door)
        {
            var distance = Vector2.Distance(door.transform.position, transform.position);
            return distance < grid.cellSize.magnitude && distance > Mathf.Epsilon;
        }
    }
}
