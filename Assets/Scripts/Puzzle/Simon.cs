using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simon : RexPuzzle
{
    [SerializeField] private int numberOfFlash = 4;
    public float flashDuration = 1;
    private List<SimonLight> flashOrder = new List<SimonLight>();
    private List<SimonLight> lights = new List<SimonLight>();
    private List<SimonLight> hitOrder = new List<SimonLight>();
    private int numberHit = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform) {
            lights.Add(child.GetComponent<SimonLight>());
        }
        for (int i = 0; i < numberOfFlash; i++) {
            flashOrder.Add(lights[Random.Range(0, lights.Count)]);
        }
        StartCoroutine("SimonStart");
    }

    public IEnumerator SimonStart() {
        foreach (var light in flashOrder) {
            light.isFlashing = true;
        }

        foreach (var light in flashOrder) {
            StartCoroutine(light.Flash());
            yield return new WaitForSeconds(flashDuration);
        }

        foreach (var light in flashOrder) {
            light.isFlashing = false;
        }

        yield return null;
    }

    public void AddToHitOrder(SimonLight light) {
        hitOrder.Add(light);
        if (CheckOrder()) {
            if (hitOrder.Count == flashOrder.Count) {
                foreach (var flash in lights) {
                    flash.canBeHit = false;
                    flash.SetToWinColor();
                }
                SwitchDoor();

                Debug.Log("SIMON PUZZLE WIN");
                return;
            }
            numberHit++;
            return;
        } else {
            Debug.Log("Wrong order");
            numberHit = 0;
            hitOrder.Clear();
            StartCoroutine("SimonStart");
        }
    }
    public bool CheckOrder() {
        if (flashOrder[numberHit] == hitOrder[numberHit]) {
            return true;
        }
        return false;
    }

    public bool CheckIfLightCanBeHit() {
        foreach (SimonLight light in lights) {
            if (!light.canBeHit)
                return false;
        }
        return true;
    }
}
