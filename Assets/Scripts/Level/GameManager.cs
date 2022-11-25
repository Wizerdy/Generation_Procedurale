using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [SerializeField] float _time = 120f;

    [Header("Reverse World")]
    [SerializeField] GameObject _volume;
    [SerializeField] Vector2 _normalSpawnPoint;
    [SerializeField] Vector2 _reverseSpawnPoint;
    [SerializeField] Timer timer;

    float _timer = 10f;
    bool _reverseWorld = false;

    private void Start() {
        _timer = _time;
    }

    public void ChangeWorld(PlayerController player) {
        _reverseWorld = !_reverseWorld;

        _volume.SetActive(_reverseWorld);

        player.transform.position = _reverseWorld ? _reverseSpawnPoint : _normalSpawnPoint;
        timer.StartTimer(_reverseWorld);
    }

    private void Update() {
        //if (_reverseWorld) {
        //    _timer -= Time.deltaTime;
        //    if (_timer <= 0f) {
        //        YouLose();
        //    }
        //}
    }

    public void YouLose() {
        SceneManager.LoadScene(1);
    }
}
