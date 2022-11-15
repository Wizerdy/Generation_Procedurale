using System.Collections.Generic;
using UnityEngine;

public class RoomEnemiesManager : MonoBehaviour
{
    private readonly List<EnemyController> enemiesInRoom = new();

    public void SetAllEnemiesInRoomActive(bool isActive)
    {
        foreach(var enemy in enemiesInRoom)
        {
            enemy.enabled = isActive;
        }
    }

    public void AddEnemyToRoom(EnemyController enemyToAdd)
    {
        enemiesInRoom.Add(enemyToAdd);
    }

    public void RemoveEnemyFromRoom(EnemyController enemyToRemove)
    {
        enemiesInRoom.Remove(enemyToRemove);
    }
}
