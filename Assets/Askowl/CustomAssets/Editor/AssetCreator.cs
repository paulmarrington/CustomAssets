using System;
using System.Collections.Generic;
using System.IO;
using CustomAsset;
using UnityEditor;
using UnityEngine;
using GameObject = UnityEngine.GameObject;
using Object = UnityEngine.Object;

namespace Askowl {
  /// <a href=""></a> //#TBD#//
  public class AssetCreator : IDisposable {
    private readonly Dictionary<string, SerializedObject> assets         = new Dictionary<string, SerializedObject>();
    private readonly Dictionary<string, SerializedObject> existingAssets = new Dictionary<string, SerializedObject>();

    /// <a href=""></a> //#TBD#//
    public string destination;
    /// <a href=""></a> //#TBD#//
    public string destinationName;

    /// <a href=""></a> //#TBD#//
    public static AssetCreator Instance(string key) {
      key = $"{key}AssetWizard.CreateAssets.destination";
      try {
        var destination = PlayerPrefs.GetString(key);
        if (string.IsNullOrWhiteSpace(destination)) return null;
        var instance = Cache<AssetCreator>.Instance;
        instance.destination     = destination;
        instance.destinationName = Path.GetFileNameWithoutExtension(destination);
        return instance;
      } finally { PlayerPrefs.DeleteKey(key); }
    }

    /// <a href=""></a> //#TBD#//
    public AssetCreator Add(params (string name, string asset)[] assetNameAndTypeList) {
      foreach (var entry in assetNameAndTypeList) Add((entry.name, ScriptableType(entry.asset)));
      return this;
    }

    /// <a href=""></a> //#TBD#//
    public AssetCreator Add(params (string name, Type asset)[] assetNameAndTypeList) {
      foreach (var entry in assetNameAndTypeList) {
        if (entry.asset == default) throw new Exception($"No asset '{entry.name}' found to add");
        if (entry.asset.IsSubclassOf(typeof(ScriptableObject))) {
          var scriptableObject = ScriptableObject.CreateInstance(entry.asset);
          var serialisedObject = new SerializedObject(scriptableObject);
          assets[entry.name] = serialisedObject;
        } else if (entry.asset.IsSubclassOf(typeof(MonoBehaviour))) {
          GetPrefabMonoBehaviour(entry.name, entry.asset);
        } else if (entry.asset.IsSubclassOf(typeof(Object))) {
          var asset = Resources.Load(entry.name, entry.asset);
          if (asset == null) throw new Exception($"No resource '{entry.name}' of type {entry.asset.Name}");
          var serialisedObject = new SerializedObject(asset);
          assets[entry.name] = serialisedObject;
        } else {
          throw new Exception($"{entry.asset.Name} is not a MonoBehaviour, ScriptableObject or Resource");
        }
      }
      return this;
    }

    public AssetCreator Load(params (string name, string asset)[] assetNameAndTypeList) {
      foreach (var entry in assetNameAndTypeList) {
        var path  = $"{destination}/{destinationName} {entry.name}.asset";
        var asset = AssetDatabase.LoadAssetAtPath(path, ScriptableType(entry.asset));
        if (asset == null) throw new Exception($"No asset '{path}' found to load");
        existingAssets[entry.name] = assets[entry.name] = new SerializedObject(asset);
      }
      return this;
    }

    /// <a href=""></a> //#TBD#//
    protected SerializedObject Asset(string assetName) {
      if (!assets.ContainsKey(assetName)) throw new Exception($"No asset '{assetName}' set in CreateAssetDictionary");
      return assets[assetName];
    }

    /// <a href=""></a> //#TBD#//
    protected void SetActiveObject(string assetName) => Selection.activeObject = Asset(assetName).targetObject;

    /// <a href=""></a> //#TBD#//
    private static Type ScriptableType(string name) => ScriptableObject.CreateInstance(name).GetType();

    /// <a href=""></a> //#TBD#//
    public AssetCreator SetField(string assetName, string fieldName, string assetField) =>
      SetField(assetName, fieldName, Asset(assetField)?.targetObject);

    /// <a href=""></a> //#TBD#//
    public AssetCreator SetField(string assetName, string fieldName, SerializedObject fieldValue) =>
      SetField(assetName, fieldName, fieldValue.targetObject);

    /// <a href=""></a> //#TBD#//
    public AssetCreator SetField(string assetName, string fieldName, Object fieldValue) {
      Field(assetName, fieldName).objectReferenceValue = fieldValue;
      return this;
    }

    /// <a href=""></a> //#TBD#//
    public SerializedProperty Field(string assetName, string fieldName = "value") {
      if (!assets.ContainsKey(assetName)) throw new Exception($"No asset '{assetName}' set in CreateAssetDictionary");
      var property = FindProperty(assetName, fieldName);
      if (property == default) throw new Exception($"No property '{fieldName}' in '{assetName}'");
      return property;
    }

    /// <a href=""></a> //#TBD#//
    public AssetCreator InsertIntoArrayField(
      string assetName, string fieldName, string assetField, int index = 0) =>
      InsertIntoArrayField(assetName, fieldName, Asset(assetField)?.targetObject, index);

    /// <a href=""></a> //#TBD#//
    public AssetCreator InsertIntoArrayField(
      string assetName, string fieldName, SerializedObject fieldValue, int index = 0) =>
      InsertIntoArrayField(assetName, fieldName, fieldValue.targetObject, index);

    /// <a href=""></a> //#TBD#//
    public AssetCreator InsertIntoArrayField(string assetName, string fieldName, Object fieldValue, int index = 0) {
      var serialisedProperty = FindProperty(assetName, fieldName);
      serialisedProperty.InsertArrayElementAtIndex(index);
      serialisedProperty.GetArrayElementAtIndex(index).objectReferenceValue = fieldValue;
      Asset(assetName).ApplyModifiedProperties();
      return this;
    }

    /// <a href=""></a> //#TBD#//
    public void InsertIntoArrayField(SerializedObject asset, string fieldName, Object fieldValue, int index = 0) {
      var serialisedProperty = FindProperty(asset, fieldName);
      serialisedProperty.InsertArrayElementAtIndex(index);
      serialisedProperty.GetArrayElementAtIndex(index).objectReferenceValue = fieldValue;
      asset.ApplyModifiedProperties();
    }

    /// <a href=""></a> //#TBD#//
    public T FindProperty<T>(string assetName, string fieldName = "value") where T : class =>
      FindProperty(assetName, fieldName).exposedReferenceValue as T;

    private SerializedProperty FindProperty(SerializedObject asset, string fieldName) {
      var property = asset.FindProperty(fieldName);
      if (property == default) throw new Exception($"No property '{fieldName}'");
      return property;
    }

    private SerializedProperty FindProperty(string assetName, string fieldName) {
      var property = Asset(assetName).FindProperty(fieldName);
      if (property == default) throw new Exception($"No property '{fieldName}' in '{assetName}'");
      return property;
    }

    /// <a href=""></a> //#TBD#//
    public void SaveAssetDictionary() {
      var    manager = GetCustomAssetManager();
      Object item    = null;
      foreach (var entry in assets) {
        if (entry.Value.targetObject.GetType().IsSubclassOf(typeof(ScriptableObject))) {
          if (!existingAssets.ContainsKey(entry.Key)) {
            string assetName = $"{destination}/{destinationName} {entry.Key}.asset";
            AssetDatabase.CreateAsset(item = entry.Value.targetObject, assetName);
          }
          if (entry.Key.EndsWith("Manager")) InsertIntoArrayField(manager, "managers", entry.Value.targetObject);
        }
        entry.Value.ApplyModifiedProperties();
      }
      AssetDatabase.SaveAssets();
      ShowInProject(item);
    }

    private void ShowInProject(Object obj = null) {
      if (obj == null) obj = AssetDatabase.LoadAssetAtPath(destination, typeof(Object));
      EditorUtility.FocusProjectWindow();
      Selection.activeObject = obj;
      EditorGUIUtility.PingObject(obj);
    }

    private SerializedObject GetCustomAssetManager() =>
      GetPrefabMonoBehaviour("Custom Asset Managers", typeof(Managers));

    private SerializedObject GetPrefabMonoBehaviour(string prefabName, Type monoBehaviour) {
      var gameObject = GameObject.Find(prefabName);
      if (gameObject == default) {
        var prefab = Resources.Load(prefabName);
        if (prefab == null) throw new Exception($"No prefab named {prefabName}");
        gameObject      = (GameObject) Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        gameObject.name = prefabName;
      }
      var component = gameObject.GetComponentInChildren(monoBehaviour);
      if (component == default) throw new Exception($"No component '{monoBehaviour.Name}' in '{gameObject.name}'1");
      var serialisedObject = new SerializedObject(component);
      assets[prefabName] = serialisedObject;
      return serialisedObject;
    }
    public void Dispose() {
      SaveAssetDictionary();
      Cache<AssetCreator>.Dispose(this);
    }
  }
}