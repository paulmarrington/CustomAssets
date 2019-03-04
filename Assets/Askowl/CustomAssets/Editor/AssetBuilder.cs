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
    private static string       name, destination;
    private static AssetBuilder builder;
    // [MenuItem("Assets/Create/NAME")]
    /// <a href=""></a> //#TBD#//
    public void CreateAssets(string assetName) {
      name = assetName;
      if (builder != null) throw new Exception($"Please wait for asset builder {name} to complete");
      builder     = this;
      destination = Destination(name);
      ProcessSource(@"cs|txt");
      var assetPath = destination.Substring(destination.IndexOf("/Assets/", StringComparison.Ordinal) + 1);
      AssetDatabase.ImportAsset(
        Path.GetDirectoryName(assetPath)
      , ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ImportRecursive);
      Debug.Log($"Scripts for `{assetPath}` waiting on rebuild for basic assets...");
      // Will continue in `OnScriptsReloaded`
    }

    /// <a href=""></a> //#TBD#//
    protected abstract void OnScriptReload();

    [DidReloadScripts] private static void Phase2() {
      if (builder == null) return;
      builder.OnScriptReload();
      builder.SaveAssetDictionary();
    }

    private static void ProcessSource(string textAssetTypes) {
      Regex    textAssetTypesRegex = new Regex($"\\.({textAssetTypes})$");
      string[] sources             = AssetDatabase.FindAssets("", new[] {TemplatePath(name)});
      using (var template = Template.Instance) {
        template.Substitute("Template", name);
        for (int i = 0; i < sources.Length; i++) {
          var sourcePath = AssetDatabase.GUIDToAssetPath(sources[i]);
          var fileName   = Path.GetFileName(sourcePath);
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
    }

    private static string TemplatePath(string name) {
      var paths = AssetDatabase.FindAssets(name);
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

    private readonly Dictionary<string, (ScriptableObject scriptableObject, SerializedObject serializedObject)> assets
      = new Dictionary<string, (ScriptableObject scriptableObject, SerializedObject serializedObject)>();

    /// <a href=""></a> //#TBD#//
    protected ScriptableObject Asset(string name) => assets[name].scriptableObject;

    /// <a href=""></a> //#TBD#//
    protected void CreateAssetDictionary(params (string name, Type customAsset)[] assetNameAndTypeList) {
      foreach (var entry in assetNameAndTypeList) {
        var scriptableObject = ScriptableObject.CreateInstance(entry.customAsset);
        var serialisedObject = new SerializedObject(scriptableObject);
        assets[entry.name] = (scriptableObject, serialisedObject);
      }
    }

    /// <a href=""></a> //#TBD#//
    protected void SetField(string assetName, string fieldName, Object fieldValue) =>
      assets[assetName].serializedObject.FindProperty(fieldName).objectReferenceValue = fieldValue;

    /// <a href=""></a> //#TBD#//
    protected void InsertIntoArrayField(string assetName, string fieldName, Object fieldValue, int index = 0) =>
      InsertIntoArrayField(assets[assetName].serializedObject, fieldName, fieldValue, index);

    /// <a href=""></a> //#TBD#//
    protected void InsertIntoArrayField(SerializedObject asset, string fieldName, Object fieldValue, int index = 0) {
      var serialisedProperty = asset.FindProperty(fieldName);
      serialisedProperty.InsertArrayElementAtIndex(index);
      serialisedProperty.GetArrayElementAtIndex(index).objectReferenceValue = fieldValue;
      asset.ApplyModifiedProperties();
    }

    /// <a href=""></a> //#TBD#//
    private void SaveAssetDictionary() {
      var manager = GetCustomAssetManager();
      foreach (var entry in assets) {
        AssetDatabase.CreateAsset(entry.Value.scriptableObject, $"{destination}/{name}{entry.Key}.asset");
        entry.Value.serializedObject.ApplyModifiedProperties();
        if (entry.Key.EndsWith("Manager")) InsertIntoArrayField(manager, "managers", entry.Value.scriptableObject);
      }
      AssetDatabase.SaveAssets();
    }

    private SerializedObject GetCustomAssetManager() =>
      GetMonoBehaviour("Custom Asset Managers", typeof(Managers));

    private SerializedObject GetMonoBehaviour(string gameObjectName, Type monoBehaviourType) {
      var managers = GameObject.Find(gameObjectName);
      if (managers == default) {
        var prefab = Resources.Load(monoBehaviourType.Name);
        managers      = (GameObject) Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        managers.name = gameObjectName;
      }
      var serialisedObject = new SerializedObject(managers.GetComponent(monoBehaviourType));
      assets[name] = (null, serialisedObject);
      return serialisedObject;
    }

    private static string Destination(string name) {
      var destinationPath = EditorUtility.SaveFilePanel(
        $"Location for your new {name}", GetSelectedPathOrFallback(), "", "");
      var serviceName = Path.GetFileNameWithoutExtension(destinationPath);
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