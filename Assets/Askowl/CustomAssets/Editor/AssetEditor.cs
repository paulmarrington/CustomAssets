using System;
using System.Collections.Generic;
using CustomAsset;
using CustomAsset.Mutable;
using UnityEditor;
using UnityEngine;
using GameObject = UnityEngine.GameObject;
using Object = UnityEngine.Object;
using String = CustomAsset.Mutable.String;

namespace Askowl {
  public class AssetEditor : IDisposable {
    private readonly Dictionary<string, SerializedObject> assets         = new Dictionary<string, SerializedObject>();
    private readonly Dictionary<string, SerializedObject> existingAssets = new Dictionary<string, SerializedObject>();

    public string destination;
    public string key;

    public static AssetEditor Instance(string key, string destination) {
      var instance = Cache<AssetEditor>.Instance;
      instance.destination = destination;
      instance.key         = key;
      return instance;
    }

    public static AssetEditor Instance(string key) {
      var pref        = $"{key}.AssetEditor.destination";
      var destination = PlayerPrefs.GetString(pref);
      PlayerPrefs.DeleteKey(pref);
      if (string.IsNullOrWhiteSpace(destination)) return null;
      var instance = Instance(key, destination);
      return instance;
    }

    public AssetEditor Add(params (string name, string asset)[] assetNameAndTypeList) {
      foreach (var entry in assetNameAndTypeList) Add((entry.name, ScriptableType(entry.asset)));
      return this;
    }

    public AssetEditor Add(params (string name, Type asset)[] assetNameAndTypeList) {
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

    public AssetEditor Load(params (string name, string asset)[] assetNameAndTypeList) {
      foreach (var entry in assetNameAndTypeList) {
        var name = entry.name.Contains(".") ? entry.name : $"{entry.name}.asset";
        var asset = AssetDb.Load(name, ScriptableType(entry.asset)) ??
                    AssetDatabase.LoadAssetAtPath($"{destination}/{entry.name}", ScriptableType(entry.asset));
        if (asset == null) throw new Exception($"No asset '{name}' found to load");
        existingAssets[entry.name] = assets[entry.name] = new SerializedObject(asset);
      }
      return this;
    }

    public SerializedObject SerialisedAsset(string assetName) {
      if (!assets.ContainsKey(assetName)) throw new Exception($"No asset '{assetName}' set in CreateAssetDictionary");
      return assets[assetName];
    }

    public Object Asset(string assetName) => SerialisedAsset(assetName).targetObject;

    protected void SetActiveObject(string assetName) => Selection.activeObject = Asset(assetName);

    private static Type ScriptableType(string name) => ScriptableObject.CreateInstance(name)?.GetType();

    public AssetEditor SetFieldToAssetEditorEntry(string assetName, string fieldName, string assetField) =>
      SetField(assetName, fieldName, Asset(assetField));

    public AssetEditor SetField(string assetName, string fieldName, SerializedObject fieldValue) =>
      SetField(assetName, fieldName, fieldValue.targetObject);

    public AssetEditor SetField(string assetName, string fieldName, string fieldValue) {
      Field(assetName, fieldName).stringValue = fieldValue;
      return this;
    }

    public AssetEditor SetField(string assetName, string fieldName, Object fieldValue) {
      Field(assetName, fieldName).objectReferenceValue = fieldValue;
      return this;
    }

    public SerializedProperty Field(string assetName, string fieldName = "value") {
      if (!assets.ContainsKey(assetName)) throw new Exception($"No asset '{assetName}' set in CreateAssetDictionary");
      fieldName = CamelCase(fieldName);
      var property = FindProperty(assetName, fieldName);
      if (property == default) FindProperty(assetName, char.ToUpper(fieldName[0]) + fieldName.Substring(1));
      if (property == default) throw new Exception($"No property '{fieldName}' in '{assetName}'");
      return property;
    }

    private string CamelCase(string name) {
      name = name.Replace(" ", "");
      if (name.Length == 0) return "";
      if (name.Length == 1) return $"{char.ToLower(name[0])}";
      return char.ToLower(name[0]) + name.Substring(1);
    }

    public AssetEditor InsertIntoArrayField(
      string assetName, string fieldName, string assetField, int index = 0) =>
      InsertIntoArrayField(assetName, fieldName, Asset(assetField), index);

    public AssetEditor InsertIntoArrayField(
      string assetName, string fieldName, SerializedObject fieldValue, int index = 0) =>
      InsertIntoArrayField(assetName, fieldName, fieldValue.targetObject, index);

    public AssetEditor InsertIntoArrayField(string assetName, string fieldName, Object fieldValue, int index = 0) {
      var serialisedProperty = FindProperty(assetName, fieldName);
      serialisedProperty.InsertArrayElementAtIndex(index);
      serialisedProperty.GetArrayElementAtIndex(index).objectReferenceValue = fieldValue;
      SerialisedAsset(assetName).ApplyModifiedPropertiesWithoutUndo();
      return this;
    }

    public void InsertIntoArrayField(SerializedObject asset, string fieldName, Object fieldValue, int index = 0) {
      var serialisedProperty = FindProperty(asset, fieldName);
      serialisedProperty.InsertArrayElementAtIndex(index);
      serialisedProperty.GetArrayElementAtIndex(index).objectReferenceValue = fieldValue;
      asset.ApplyModifiedPropertiesWithoutUndo();
    }

    public T FindProperty<T>(string assetName, string fieldName = "value") where T : class =>
      FindProperty(assetName, fieldName).exposedReferenceValue as T;

    private SerializedProperty FindProperty(SerializedObject asset, string fieldName) {
      var property = asset.FindProperty(fieldName);
      if (property == default) throw new Exception($"No property '{fieldName}'");
      return property;
    }

    private SerializedProperty FindProperty(string assetName, string fieldName) {
      var property = SerialisedAsset(assetName).FindProperty(fieldName);
      if (property == default) throw new Exception($"No property '{fieldName}' in '{assetName}'");
      return property;
    }

    public AssetEditor Save() {
      var    manager = GetCustomAssetManager();
      Object item    = null;
      foreach (var entry in assets) {
        if (entry.Value.targetObject.GetType().IsSubclassOf(typeof(ScriptableObject))) {
          if (!existingAssets.ContainsKey(entry.Key)) {
            string assetName = $"{destination}{entry.Key}.asset";
            AssetDatabase.CreateAsset(item = entry.Value.targetObject, assetName);
          }
          if (entry.Key.EndsWith("Manager")) InsertIntoArrayField(manager, "managers", entry.Value.targetObject);
        }
        entry.Value.ApplyModifiedPropertiesWithoutUndo();
      }
      AssetDatabase.SaveAssets();
      AssetDb.Instance.Select(item).Dispose();
      return this;
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
      Save();
      Cache<AssetEditor>.Dispose(this);
    }
  }
}