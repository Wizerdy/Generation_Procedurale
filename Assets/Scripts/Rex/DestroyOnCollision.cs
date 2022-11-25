using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour {
    private void OnCollisionEnter2D(Collision2D collision) {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        gameObject.SetActive(false);
    }
}
