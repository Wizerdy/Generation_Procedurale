using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public IReadOnlyList<RoomController> allRooms => Array.AsReadOnly(rooms);
    private RoomController activeRoom;
    private RoomController[] rooms;
    private CinemachineBrain cinemachineBrain;

    private void Awake()
    {
        cinemachineBrain = FindObjectOfType<CinemachineBrain>();
        rooms = GetComponentsInChildren<RoomController>();
    }

    public void OnCameraBlend(ICinemachineCamera newCam, ICinemachineCamera oldCam)
    {
        StartCoroutine(WaitForBlend());

        IEnumerator WaitForBlend()
        {
            if (cinemachineBrain.IsBlending)
            {
                yield return new WaitForSeconds(cinemachineBrain.ActiveBlend.Duration);
            }

            SetActiveRoom(newCam.VirtualCameraGameObject.GetComponentInParent<RoomController>());
        }
    }

    public RoomController[] GetNeighbours(RoomController roomToCheck)
    {
        var doors = roomToCheck.GetComponentsInChildren<DoorController>();
        var neighbours = new RoomController[doors.Length];
        for (var i = 0; i < doors.Length; i++)
        {
            neighbours[i] = doors[i].connectedDoor.GetComponentInParent<RoomController>();
        }
        return neighbours;
    }


    private void SetActiveRoom(RoomController newActiveRoom)
    {
        if (activeRoom != null)
            activeRoom.SetRoomActive(false);
        newActiveRoom.SetRoomActive(true);
        activeRoom = newActiveRoom;
    }
}
