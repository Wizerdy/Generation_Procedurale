using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class SimonLight : MonoBehaviour
{
    [SerializeField] private Simon simon;
    [SerializeField] private Color baseColor;
    [SerializeField] private Color flashColor;
    private SpriteRenderer spriteRenderer;
    [HideInInspector] public bool isFlashing = false;
    [HideInInspector] public bool canBeHit = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = baseColor;
    }

    public IEnumerator Flash() {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(simon.flashDuration);
        spriteRenderer.color = baseColor;
        yield return null;
    }

    private void OnMouseDown() {
        LightTrigger();
    }

    private void OnMouseUp() {
        if (canBeHit)
            spriteRenderer.color = baseColor;
    }

    public void SetToWinColor() {
        StopCoroutine("Flash");
        spriteRenderer.color = flashColor;
    }

    public void LightTrigger() {
        if (!isFlashing && canBeHit && simon.CheckIfLightCanBeHit()) {
            StartCoroutine("Flash");
            simon.AddToHitOrder(this);
        }
    }
}
