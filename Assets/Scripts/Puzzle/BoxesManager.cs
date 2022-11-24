using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxesManager : RexPuzzle
{
    private List<Boxe> boxes = new List<Boxe>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform) {
            boxes.Add(child.GetComponent<Boxe>());
        }

        foreach (var boxe in boxes) {
            boxe.SetManager(this);
        }

        SetWinningBoxe();
    }

    private void SetWinningBoxe() {
        boxes[Random.Range(0, boxes.Count)].isWinningBoxe = true;
    }
    public void OnWinningBoxeDestroy() {
        SwitchDoor();

        Debug.Log("BOX PUZZLE WIN");
    }
}
