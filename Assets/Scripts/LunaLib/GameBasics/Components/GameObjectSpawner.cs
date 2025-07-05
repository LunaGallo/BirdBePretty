using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public class GameObjectSpawner : MonoBehaviour {

        public List<GameObject> prefabs;
        public List<Transform> spawnPoints;
        public bool spawnWithRotation;

        public void Spawn() {
            GameObject prefab = prefabs.RandomElement();
            Transform spawnPoint = spawnPoints.RandomElement();
            if (spawnWithRotation) {
                Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            }
            else {
                Instantiate(prefab, spawnPoint.position, Quaternion.identity);
            }
        }

    }
}
