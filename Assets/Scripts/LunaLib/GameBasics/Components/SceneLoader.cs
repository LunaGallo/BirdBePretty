using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LunaLib {
    public class SceneLoader : MonoBehaviour {
    
        public UnityEvent onSceneLoaded;
        public IntegerEvent onSceneIDLoaded;
        public StringEvent onSceneNameLoaded;
        [ValueDropdown("EDITOR_GetBuildSceneNames", AppendNextDrawer = true)] public static string sceneToLoad;

        public void SelectScene (string sceneName) {
            sceneToLoad = sceneName;
        }

        public void LoadScene () {
            SceneManager.LoadScene (sceneToLoad);
        }
        public void LoadScene(string sceneName) {
            SceneManager.LoadScene(sceneName);
        }
        public void LoadScene(int sceneID) {
            SceneManager.LoadScene(sceneID);
        }
        public void LoadSceneRelative(int offset) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + offset);
        }

        public void QuitApplication() {
            Application.Quit();
        }

        public static SceneLoader instance;
        private void Awake() {
            instance = this;
            onSceneLoaded.Invoke();
            onSceneIDLoaded.Invoke(SceneManager.GetActiveScene().buildIndex);
            onSceneNameLoaded.Invoke(SceneManager.GetActiveScene().name);
        }

#if UNITY_EDITOR
        private IEnumerable<string> EDITOR_GetBuildSceneNames() {
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
                yield return scene.path;
            }
        }
        private IEnumerable<int> EDITOR_GetBuildSceneIndices() {
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++) {
                yield return i;
            }
        }
#endif

    }

}

