using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRoom : MonoBehaviour {
    [System.Serializable]
    public struct Weapons {
        public WeaponType type;
        public GameObject weapon;
    }

    private static WeaponType globalType;

    [SerializeField] List<Weapons> _weapons;
    public bool empty = false;

    private void Start() {
        if (empty) { return; }
        int current = (int)globalType;
        ++current;
        current %= 4;
        globalType = (WeaponType)current;

        if (globalType == WeaponType.None) {
            globalType = WeaponType.ThunderBall;
        }

        for (int i = 0; i < _weapons.Count; i++) {
            if (_weapons[i].type == globalType) {
                GameObject insta = Instantiate(_weapons[i].weapon, transform);
                insta.transform.localPosition = Vector2.one * 0.5f;
            }
        }
    }
}
