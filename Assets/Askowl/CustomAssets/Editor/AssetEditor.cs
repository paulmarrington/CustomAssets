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
    private readonly Dictionary<string, (SerializedObject serializedAsset, string path)> assets =
      new Dictionary<string, (SerializedObject, string)>();
    private readonly Dictionary<string, bool> existingAssets = new Dictionary<string, bool>();

    public static AssetEditor Instance => Cache<AssetEditor>.Instance;

    public AssetEditor Add(string assetName, string nameSpace, string path) {
      var  qualifiedName = string.IsNullOrWhiteSpace(nameSpace) ? assetName : $"{nameSpace}.{assetName}";
      Type assetType     = ScriptableType(qualifiedName);
      if (assetType == default) throw new Exception($"No asset '{qualifiedName}' found to add");
      return Add(assetName, assetType, path);
    }

    public AssetEditor Add(string assetName, Type assetType, string path) {
      if (assetType == default) throw new Exception($"No asset '{assetName}' found to add");
      if (assetType.IsSubclassOf(typeof(ScriptableObject))) {
        var scriptableObject = ScriptableObject.CreateInstance(assetType);
        assets[assetName] = (new SerializedObject(scriptableObject), path);
      } else if (assetType.IsSubclassOf(typeof(MonoBehaviour))) {
        GetPrefabMonoBehaviour(assetName, assetType, path);
      } else if (assetType.IsSubclassOf(typeof(Object))) {
        var asset = Resources.Load(assetName, assetType);
        if (asset == null) throw new Exception($"No resource '{assetName}' of type {assetType.Name}");
        var serialisedObject = new SerializedObject(asset);
        assets[assetName] = (serialisedObject, path);
      } else {
        throw new Exception($"{assetType.Name} is not a MonoBehaviour, ScriptableObject or Resource");
      }
      return this;
    }

    public AssetEditor Load(string assetName, string nameSpace, string path) {
      if (Exists(assetName)) return this;
      var name      = assetName.Contains(".") ? assetName : $"{assetName}.asset";
      var assetType = ScriptableType($"{nameSpace}.{assetName}");
      var asset     = AssetDb.Load(name, assetType) ?? AssetDatabase.LoadAssetAtPath($"{path}/{assetName}", assetType);
      if (asset == null) throw new Exception($"No asset '{name}' found to load");
      existingAssets[assetName] = true;
      assets[assetName]         = (new SerializedObject(asset), path);
      return this;
    }

    public bool Exists(string assetName) => assets.ContainsKey(assetName);

    public SerializedObject SerialisedAsset(string assetName) {
      if (!Exists(assetName)) throw new Exception($"No asset '{assetName}' set in CreateAssetDictionary");
      return assets[assetName].serializedAsset;
    }

    public Object Asset(string assetName) => SerialisedAsset(assetName).targetObject;

    public string AssetPath(string assetName) => assets[assetName].path;

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
      if (!assets.ContainsKey(assetName)) throw new Exception($"No asset '{assetName}' set in CreateAssetDictionary - Have you run 'Assets // Create // Decoupler // Build Assets`?");
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
        if (entry.Value.serializedAsset.targetObject.GetType().IsSubclassOf(typeof(ScriptableObject))) {
          if (!existingAssets.ContainsKey(entry.Key))
            AssetDatabase.CreateAsset(item = entry.Value.serializedAsset.targetObject, entry.Value.path);
          if (entry.Key.EndsWith("Manager"))
            InsertIntoArrayField(manager, "managers", entry.Value.serializedAsset.targetObject);
        }
        entry.Value.serializedAsset.ApplyModifiedPropertiesWithoutUndo();
      }
      AssetDatabase.SaveAssets();
      AssetDb.Instance.Select(item).Dispose();
      return this;
    }

    private SerializedObject GetCustomAssetManager() =>
      GetPrefabMonoBehaviour("Custom Asset Managers", typeof(Managers), "");

    private SerializedObject GetPrefabMonoBehaviour(string prefabName, Type monoBehaviour, string path) {
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
      assets[prefabName] = (serialisedObject, path);
      return serialisedObject;
    }
    public void Dispose() {
      Save();
      Cache<AssetEditor>.Dispose(this);
    }
  }
}