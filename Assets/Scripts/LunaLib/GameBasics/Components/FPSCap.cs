﻿using UnityEngine;

namespace LunaLib {
    public class FPSCap : MonoBehaviour {

        public int target = 30;

        void Awake() {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = target;
        }

        void Update() {
            if (Application.targetFrameRate != target) {
                Application.targetFrameRate = target;
            }
            Debug.Log(Application.targetFrameRate);
        }

    }

}
