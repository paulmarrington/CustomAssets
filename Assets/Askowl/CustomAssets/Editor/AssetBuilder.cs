// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using CustomAsset;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using GameObject = UnityEngine.GameObject;
using Object = UnityEngine.Object;
using String = CustomAsset.Constant.String;

namespace Askowl {
  /// <a href=""></a> //#TBD#//
  public abstract class AssetBuilder {
    private static string       name, destination, destinationName, destinationRelative;
    private static AssetBuilder builder;
    // [MenuItem("Assets/Create/NAME")]
    /// <a href=""></a> //#TBD#//
    public void CreateAssets(string assetName) {
      name = assetName;
      if (builder != null) throw new Exception($"Please wait for asset builder {name} to complete");
      builder     = this;
      destination = Destination();
      bool hasSource = ProcessSource(@"cs|txt");
      destinationRelative = destination.Substring(destination.IndexOf("/Assets/", StringComparison.Ordinal) + 1);
      AssetDatabase.ImportAsset(
        Path.GetDirectoryName(destinationRelative)
      , ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ImportRecursive);
      if (hasSource) {
        Debug.Log($"Scripts for `{destinationRelative}` waiting on rebuild for basic assets...");
        // Will continue in `OnScriptsReloaded` if there is source to recompile
      } else {
        Phase2();
      }
    }

    /// <a href=""></a> //#TBD#//
    protected abstract void OnScriptReload();

    [DidReloadScripts] private static void Phase2() {
      if (builder == null) return;
      builder.OnScriptReload();
      builder.SaveAssetDictionary();
    }

    private static bool ProcessSource(string textAssetTypes) {
      bool     hasSource           = false;
      Regex    textAssetTypesRegex = new Regex($"\\.({textAssetTypes})$");
      string[] sources             = AssetDatabase.FindAssets("", new[] {TemplatePath()});
      using (var template = Template.Instance) {
        template.Substitute("Template", name);
        for (int i = 0; i < sources.Length; i++) {
          var sourcePath                            = AssetDatabase.GUIDToAssetPath(sources[i]);
          if (sourcePath.EndsWith(".cs")) hasSource = true;
          var fileName                              = Path.GetFileName(sourcePath);
          if (File.Exists(sourcePath)) {
            if (textAssetTypesRegex.IsMatch(sourcePath)) {
              var text = template.Process(File.ReadAllText(sourcePath));
              File.WriteAllText($"{destination}/{name}{fileName}", text);
            } else {
              File.Copy(sourcePath, $"{destination}/{fileName}");
            }
          }
        }
      }
      return hasSource;
    }

    private static string TemplatePath() {
      var paths = AssetDatabase.FindAssets($"{name}TemplatePath");
      for (int i = 0; i < paths.Length; i++) {
        var path = AssetDatabase.GUIDToAssetPath(paths[i]);
        if (path.IndexOf("Askowl", StringComparison.Ordinal) == -1) return path;
      }
      if (paths.Length != 0) return AssetDatabase.LoadAssetAtPath<String>(AssetDatabase.GUIDToAssetPath(paths[0]));

      string exists(string path) {
        path = $"Assets/Askowl/{name}/{path}";
        return Directory.Exists(path) ? path : null;
      }
      return exists("Editor/Template") ?? exists("Template") ?? exists("scripts/Template") ?? "";
    }

    private static string GetSelectedPathOrFallback() {
      string path = "Assets";
      foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets)) {
        path = AssetDatabase.GetAssetPath(obj);
        if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
          path = Path.GetDirectoryName(path);
          break;
        }
      }
      return path;
    }

    private readonly Dictionary<string, SerializedObject> assets = new Dictionary<string, SerializedObject>();

    /// <a href=""></a> //#TBD#//
    protected SerializedObject Asset(string assetName) {
      if (!assets.ContainsKey(assetName)) throw new Exception($"No asset '{assetName}' set in CreateAssetDictionary");
      return assets[assetName];
    }

    /// <a href=""></a> //#TBD#//
    protected void CreateAssetDictionary(params (string name, Type asset)[] assetNameAndTypeList) {
      foreach (var entry in assetNameAndTypeList) {
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
          throw new Exception($"{entry.asset.Name} is not a MonoBehaviour or ScriptableObject");
        }
      }
    }

    /// <a href=""></a> //#TBD#//
    protected void SetField(string assetName, string fieldName, SerializedObject fieldValue) =>
      SetField(assetName, fieldName, fieldValue.targetObject);

    /// <a href=""></a> //#TBD#//
    protected void SetField(string assetName, string fieldName, Object fieldValue) =>
      Field(assetName, fieldName).objectReferenceValue = fieldValue;

    /// <a href=""></a> //#TBD#//
    protected SerializedProperty Field(string assetName, string fieldName) {
      if (!assets.ContainsKey(assetName)) throw new Exception($"No asset '{assetName}' set in CreateAssetDictionary");
      var property = FindProperty(assetName, fieldName);
      if (property == default) throw new Exception($"No property '{fieldName}' in '{assetName}'");
      return property;
    }

    /// <a href=""></a> //#TBD#//
    protected void InsertIntoArrayField(
      string assetName, string fieldName, SerializedObject fieldValue, int index = 0) =>
      InsertIntoArrayField(assetName, fieldName, fieldValue.targetObject, index);

    /// <a href=""></a> //#TBD#//
    protected void InsertIntoArrayField(string assetName, string fieldName, Object fieldValue, int index = 0) {
      var serialisedProperty = FindProperty(assetName, fieldName);
      serialisedProperty.InsertArrayElementAtIndex(index);
      serialisedProperty.GetArrayElementAtIndex(index).objectReferenceValue = fieldValue;
      Asset(assetName).ApplyModifiedProperties();
    }

    /// <a href=""></a> //#TBD#//
    protected void InsertIntoArrayField(SerializedObject asset, string fieldName, Object fieldValue, int index = 0) {
      var serialisedProperty = FindProperty(asset, fieldName);
      serialisedProperty.InsertArrayElementAtIndex(index);
      serialisedProperty.GetArrayElementAtIndex(index).objectReferenceValue = fieldValue;
      asset.ApplyModifiedProperties();
    }

    /// <a href=""></a> //#TBD#//
    protected T FindProperty<T>(string assetName, string fieldName) where T : class =>
      FindProperty(assetName, fieldName) as T;

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
    private void SaveAssetDictionary() {
      var manager = GetCustomAssetManager();
      foreach (var entry in assets) {
        if (entry.Value != default) {
          string assetName = $"{destinationRelative}/{destinationName} {name} {entry.Key}.asset";
          AssetDatabase.CreateAsset(entry.Value.targetObject, assetName);
          if (entry.Key.EndsWith("Manager")) InsertIntoArrayField(manager, "managers", entry.Value.targetObject);
          entry.Value.ApplyModifiedProperties();
        }
      }
      AssetDatabase.SaveAssets();
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

    private static string Destination() {
      var destinationPath = EditorUtility.SaveFilePanel(
        $"Location for your new {name}", GetSelectedPathOrFallback(), "", "");
      destinationName = Path.GetFileNameWithoutExtension(destinationPath);
      if (string.IsNullOrEmpty(destinationPath)) return null;
      if (Directory.Exists(destinationPath)) {
        Debug.LogError($"{destinationPath} already exists. Please select a different name or project directory");
        return null;
      }
      Directory.CreateDirectory(destinationPath);
      return destinationPath;
    }
  }
}