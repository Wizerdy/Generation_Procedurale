using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitDebugger : MonoBehaviour {
    [SerializeField] Gate _first;
    [SerializeField] Gate _second;

    void OnValidate() {
        Debug.Log("Processing");
        int4 first = new int4(_first);
        int4 second = new int4(_second);
        int4 result = first - second;
        Debug.Log(first + " - " + second + " = " + result);
    }
}
