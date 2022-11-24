using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boxe : MonoBehaviour
{
    private BoxesManager manager;
    public bool isWinningBoxe = false;


    public void SetManager(BoxesManager _manager) {
        manager = _manager;
    }

    private void OnDestroy() {
        if (isWinningBoxe) {
            manager.OnWinningBoxeDestroy();
        }
    }
}
