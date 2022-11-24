using UnityEngine;
using UnityEngine.Events;

public class RoomController : MonoBehaviour
{
    private RoomController[] neighbours;

    private RoomEnemiesManager enemiesManager;
    private LevelManager levelManager;

    private void Awake()
    {
        enemiesManager = GetComponentInChildren<RoomEnemiesManager>();
        levelManager = GetComponentInParent<LevelManager>();

        levelManager.OnLevelGenerated += Init;
    }

    private void Init()
    {
        neighbours = levelManager.GetNeighbours(this);
    }

    public void SetRoomActive(bool isActive)
    {
        enemiesManager.SetAllEnemiesInRoomActive(isActive);
    }
}
