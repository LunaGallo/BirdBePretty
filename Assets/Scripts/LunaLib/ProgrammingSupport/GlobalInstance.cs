using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public class GlobalInstance : MonoBehaviour {

        public static Dictionary<string, List<GlobalInstance>> dictionary = new();

        public string type;

        public virtual void OnEnable() {
            if(dictionary.TryGetValue(type, out List<GlobalInstance> instanceList)) {
                instanceList.Add(this);
            } else {
                instanceList = new List<GlobalInstance> { this };
                dictionary.Add(type, instanceList);
            }
        }
        public virtual void OnDisable() {
            if(dictionary.TryGetValue(type, out List<GlobalInstance> instanceList)) {
                instanceList.Remove(this);
                if(instanceList.Count == 0) {
                    dictionary.Remove(type);
                }
            }
        }

        public static List<GlobalInstance> GetInstanceListOfType(string type) {
            dictionary.TryGetValue(type, out List<GlobalInstance> instanceList);
            return instanceList;
        }
        public static List<GameObject> GetGameObjectListOfType(string type) => GetInstanceListOfType(type).ConvertAll(i => i.gameObject);
        public List<GlobalInstance> GetInstanceListOfType() => GetInstanceListOfType(type);
        public List<GameObject> GetGameObjectListOfType() => GetInstanceListOfType().ConvertAll(i => i.gameObject);

        public static GlobalInstance GetInstanceOfType(string type) {
            dictionary.TryGetValue(type, out List<GlobalInstance> instanceList);
            if(instanceList != null || instanceList.Count == 0) {
                return null;
            }
            return instanceList[0];
        }
        public static GameObject GetGameObjectOfType(string type) => GetInstanceOfType(type).gameObject;
        public GlobalInstance GetInstanceOfType() => GetInstanceOfType(type);
        public GameObject GetGameObjectOfType() => GetInstanceOfType().gameObject;

    }
}
