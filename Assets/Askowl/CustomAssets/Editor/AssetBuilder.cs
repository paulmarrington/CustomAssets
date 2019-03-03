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
    public void CreateAssets(string name) {
      if (builder != null) throw new Exception($"Please wait for asset builder {name} to complete");
      builder     = this;
      destination = Destination(name);
      ProcessSource(name, destination, "cs|txt");
      var assetPath = destination.Substring(destination.IndexOf("/Assets/", StringComparison.Ordinal) + 1);
      AssetDatabase.ImportAsset(
        Path.GetDirectoryName(assetPath)
      , ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ImportRecursive);
      Debug.Log($"Scripts for `{assetPath}` waiting on rebuild for basic assets...");
      // Will continue in `OnScriptsReloaded`
    }

    /// <a href=""></a> //#TBD#//
    protected abstract void OnScriptReload();

    [DidReloadScripts] private static void Phase2() => builder?.OnScriptReload();

    private static void ProcessSource(string name, string destination, string textAssetTypes = @"cs|txt") {
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

    private struct Asset {
      public ScriptableObject scriptableObject;
      public SerializedObject serializedObject;
    }
    private Dictionary<string, Asset> assets;

    /// <a href=""></a> //#TBD#//
    public void CreateAssetDictionary(string assetNameCsv) {
      foreach (var assetNameAndType in assetNameCsv.Split(',')) {
        string[] nameType         = assetNameAndType.Split('/');
        string   assetName        = nameType[0];
        string   assetType        = nameType[nameType.Length > 1 ? 1 : 0];
        var      scriptableObject = ScriptableObject.CreateInstance(assetType);
        var      serialisedObject = new SerializedObject(scriptableObject);
        assets[assetName] = new Asset {scriptableObject = scriptableObject, serializedObject = serialisedObject};
      }
    }

    /// <a href=""></a> //#TBD#//
    public void SetField(string assetName, string fieldName, Object fieldValue) =>
      assets[assetName].serializedObject.FindProperty(fieldName).objectReferenceValue = fieldValue;

    /// <a href=""></a> //#TBD#//
    public void InsertIntoArrayField(string assetName, string fieldName, Object fieldValue, int index = 0) {
      var asset              = assets[assetName].serializedObject;
      var serialisedProperty = asset.FindProperty(fieldName);
      serialisedProperty.InsertArrayElementAtIndex(index);
      serialisedProperty.GetArrayElementAtIndex(index).objectReferenceValue = fieldValue;
      asset.ApplyModifiedProperties();
    }

    /// <a href=""></a> //#TBD#//
    public void SaveAssetDictionary() {
      assets.
    }

    // *********************
    private static void TheRest() {
      var servicesManager = ScriptableObject.CreateInstance($"Decoupler.Services.{name}ServicesManager");
      var context         = ScriptableObject.CreateInstance($"Decoupler.Services.{name}Context");
      var serviceForMock  = ScriptableObject.CreateInstance($"Decoupler.Services.{name}ServiceForMock");

      var servicesManagerSerializedObject = new SerializedObject(servicesManager);
      var contextSerializedObject         = new SerializedObject(context);
      var serviceForMockSerializedObject  = new SerializedObject(serviceForMock);

      SetField(servicesManagerSerializedObject, "context", context);
      SetField(serviceForMockSerializedObject,  "context", context);
      InsertIntoArrayField(servicesManagerSerializedObject, "services", serviceForMock);

      AssetDatabase.CreateAsset(servicesManager, $"{destination}/{name}ServicesManager.asset");
      AssetDatabase.CreateAsset(context,         $"{destination}/{name}MockContext.asset");
      AssetDatabase.CreateAsset(serviceForMock,  $"{destination}/{name}ServiceForMock.asset");

      servicesManagerSerializedObject.ApplyModifiedProperties();
      contextSerializedObject.ApplyModifiedProperties();
      serviceForMockSerializedObject.ApplyModifiedProperties();
      AssetDatabase.SaveAssets();

      var managers = GameObject.Find("/Service Managers");
      if (managers == default) {
        var prefab = Resources.Load("Managers");
        managers      = (GameObject) Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        managers.name = "Service Managers";
      }
      var managersSerializedObject = new SerializedObject(managers.GetComponent<Managers>());
      InsertIntoArrayField(managersSerializedObject, "managers", servicesManager);
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