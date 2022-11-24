using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiddleRoom : RexRoom {
    [SerializeField] List<RexPuzzle> EZ = new List<RexPuzzle>();
    [SerializeField] List<RexPuzzle> LESSEZ = new List<RexPuzzle>();
    [SerializeField] List<RexPuzzle> NOTEZ = new List<RexPuzzle>();
    public int CULTY = 0;

    private void Start() {
        RexPuzzle THECHOSENONE;
        switch (CULTY) {
            case 0:
                THECHOSENONE = EZ[Random.Range(0, EZ.Count)];
                break;
            case 1:
                THECHOSENONE = LESSEZ[Random.Range(0, LESSEZ.Count)];
                break;
            default:
            case 2:
                THECHOSENONE = NOTEZ[Random.Range(0, NOTEZ.Count)];
                break;
        }
        THECHOSENONE.gameObject.SetActive(true);
        for (int i = 0; i < DOORS.Count; i++) {
            THECHOSENONE.AddDoor(DOORS[i]);
        }
    }
}
