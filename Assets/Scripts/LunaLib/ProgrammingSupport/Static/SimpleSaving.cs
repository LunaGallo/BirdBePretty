using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

namespace LunaLib {
    public static class SaveManager {

        public static SortedDictionary<string, List<int>> intValues = new SortedDictionary<string, List<int>>();
        public static SortedDictionary<string, List<float>> floatValues = new SortedDictionary<string, List<float>>();
        public static SortedDictionary<string, List<double>> doubleValues = new SortedDictionary<string, List<double>>();
        public static SortedDictionary<string, List<bool>> boolValues = new SortedDictionary<string, List<bool>>();
        public static SortedDictionary<string, List<string>> stringValues = new SortedDictionary<string, List<string>>();

        public static string fileName = "save";
        public static string fileType = "gd";
        public static string FilePath {
            get {
                return Path.Combine(Application.persistentDataPath, fileName + "." + fileType);
            }
        }

        public static void SaveValues() {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(FilePath);
            bf.Serialize(file, ValuesToObject());
            file.Close();
        }
        public static void LoadValues() {
            if (File.Exists(FilePath)) {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(FilePath, FileMode.Open);
                GetValuesFromObject((SavingObject)bf.Deserialize(file));
                file.Close();
            }
        }
        [Serializable] private class SavingObject {
            public SortedDictionary<string, List<int>> intValues = new SortedDictionary<string, List<int>>();
            public SortedDictionary<string, List<float>> floatValues = new SortedDictionary<string, List<float>>();
            public SortedDictionary<string, List<double>> doubleValues = new SortedDictionary<string, List<double>>();
            public SortedDictionary<string, List<bool>> boolValues = new SortedDictionary<string, List<bool>>();
            public SortedDictionary<string, List<string>> stringValues = new SortedDictionary<string, List<string>>();
        }
        private static SavingObject ValuesToObject() {
            return new SavingObject() {
                intValues = intValues,
                floatValues = floatValues,
                doubleValues = doubleValues,
                boolValues = boolValues,
                stringValues = stringValues
            };
        }
        private static void GetValuesFromObject(SavingObject savingObject) {
            intValues = savingObject.intValues;
            floatValues = savingObject.floatValues;
            doubleValues = savingObject.doubleValues;
            boolValues = savingObject.boolValues;
            stringValues = savingObject.stringValues;
        }

    }

}
