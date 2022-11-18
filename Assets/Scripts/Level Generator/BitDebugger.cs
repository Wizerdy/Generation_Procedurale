using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitDebugger : MonoBehaviour {
    [SerializeField] Gate _first;
    [SerializeField] Gate _second;

    void OnValidate() {
        Debug.Log("Processing");
        bint4 first = new bint4(_first);
        bint4 second = new bint4(_second);
        bint4 result = first - second;
        Debug.Log(first + " - " + second + " = " + result);
        // 1000 - 0100 => 0100
        // 1000 - 0100 => 1000 // 1000 ^ 0100 => 
        // 1000 - 1100 => 0000
        // 1000 + 1100 => 1100

        // >> 0011 (3) => 0001
        // >> 1100 (12) => 0110 (6)
        // << 0011 => 0110

        // int => -1 000 0000 0000 0000 0000 0000 0000 0000 -3600
        // float => 0 000 0000 0000 0000 0000 0000 0000 0000 0.657 657x10^-3
    }
}
