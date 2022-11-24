using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool isFlashing;
    public bool isActive;
    [SerializeField] private MagicBalls magicBalls;
    [SerializeField] private SpriteRenderer spriteRendere;

    public void ChangeColor() {
        if (!isFlashing)
            spriteRendere.color = magicBalls.flashColor;
        else
            spriteRendere.color = magicBalls.baseColor;

    }

    public void OnHit() {
        if (isActive) {
            isActive = false;
        } else {
            isActive = true;
        }
    }
}
