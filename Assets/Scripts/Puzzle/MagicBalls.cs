using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBalls : MonoBehaviour
{
    public Color baseColor;
    public Color flashColor;
    public int flashDuration;
    [SerializeField] List<RexDoors> _doors = new List<RexDoors>();
    private List<Ball> balls = new List<Ball>();
    private bool winGame = false;

    void Start()
    {
        foreach (Transform child in transform) {
            Ball ball = child.GetComponent<Ball>();
            ball.isFlashing = Random.value < 0.5f;
            ball.isActive = true;
            balls.Add(ball);
        }

        while (CheckAllBallState(true) || CheckAllBallState(false)) {
            foreach (Ball ball in balls) {
                ball.isFlashing = Random.value < 0.5f;
            }
        }

        StartCoroutine(FlashActiveBalls());
    }

    public IEnumerator FlashActiveBalls() {
        while (!winGame) {
            foreach (Ball ball in balls) {
                if (ball.isActive) {
                    if (ball.isFlashing)
                        ball.isFlashing = false;
                    else
                        ball.isFlashing = true;
                }
            }
            ChangeActiveBallsColor();

            if (CheckAllBallState(true) || CheckAllBallState(false)) {
                Debug.Log("WIN MAGIC BALL");
                winGame = true;

                SwitchDoor();
                StopAllCoroutines();
            }
            yield return new WaitForSeconds(flashDuration);
        }
    }

    public void ChangeActiveBallsColor() {
        foreach (Ball ball in balls) {
            if (ball.isActive) {
                ball.ChangeColor();
            }
        }
    }

    public bool CheckAllBallState(bool stateToCheck) {
        for (int i = 0; i < balls.Count; i++) {
            if (balls[i].isFlashing != stateToCheck) {
                return false;
            }
        }
        return true;
    }

    public void SwitchDoor() {
        for (int i = 0; i < _doors.Count; i++) {
            _doors[i].ChangeState();
        }
    }

    public void AddDoor(RexDoors door) {
        if (!_doors.Contains(door)) {
            _doors.Add(door);
        }
    }

    public void RemoveDoor(RexDoors door) {
        if (_doors.Contains(door)) {
            _doors.Remove(door);
        }
    }
}
