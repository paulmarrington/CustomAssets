// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
  public abstract class AssetWizard : Manager {
    /// <a href=""></a> //#TBD#//
    protected static string assetType, destination, destinationName, selectedPathInProjectView;
    /// <a href=""></a> //#TBD#//

    protected override void OnEnable() {
      base.OnEnable();
      selectedPathInProjectView = GetSelectedPathInProjectView();
    }

    /// <a href=""></a> //#TBD#//
    protected void CreateAssets(string newAssetType) {
      Selection.activeObject = null; // in case we had a creation form inspector up
        assetType = newAssetType;
        SetDestination();
        bool hasSource = ProcessAllFiles(@"cs|txt");
        AssetDatabase.ImportAsset(
          Path.GetDirectoryName(destination)
        , ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ImportRecursive);

        if (hasSource) {
          Debug.Log($"Scripts for `{destination}` waiting on rebuild for basic assets...");
          // Will continue in `OnScriptsReloaded` if there is source to recompile
        } else {
          Phase2();
        }
    }

    [DidReloadScripts] private static void Phase2() {
      var wizard = OnScriptReload();
      wizard.SaveAssetDictionary();
      Debug.Log("...All Done");
    }

    /// <a href=""></a> //#TBD#//
    protected virtual bool ProcessAllFiles(string textAssetTypes) {
      string[] sources                                    = AssetDatabase.FindAssets("", new[] {TemplatePath()});
      for (int i = 0; i < sources.Length; i++) sources[i] = AssetDatabase.GUIDToAssetPath(sources[i]);
      return ProcessFiles(textAssetTypes, sources);
    }

    /// <a href=""></a> //#TBD#//
    protected bool ProcessFiles(string textAssetTypes, params string[] files) {
      bool  hasSource           = false;
      Regex textAssetTypesRegex = new Regex($@"\.({textAssetTypes})$");
      using (var template = Template.Instance) {
        for (int i = 0; i < files.Length; i++) {
          var fileName = Path.GetFileName(files[i]) ?? "";
          if (File.Exists(files[i])) {
            if (textAssetTypesRegex.IsMatch(files[i])) {
              if (files[i].EndsWith(".cs")) hasSource = true;
              var text                                = FillTemplate(template, File.ReadAllText(files[i]));
              File.WriteAllText(
                fileName.StartsWith(destinationName)
                  ? $"{destination}/{fileName}"
                  : $"{destination}/{destinationName}{fileName}", text);
            } else {
              File.Copy(files[i], $"{destination}/{fileName}");
            }
          }
        }
      }
      return hasSource;
    }

    /// <a href=""></a> //#TBD#//
    protected virtual string FillTemplate(Template template, string text) => template.From(text).Result();

    private static string TemplatePath() {
      var paths = AssetDatabase.FindAssets($"{assetType}TemplatePath");
      for (int i = 0; i < paths.Length; i++) {
        var path = AssetDatabase.GUIDToAssetPath(paths[i]);
        if (path.IndexOf("Askowl", StringComparison.Ordinal) == -1) return path;
      }
      if (paths.Length != 0) return AssetDatabase.LoadAssetAtPath<String>(AssetDatabase.GUIDToAssetPath(paths[0]));

      string exists(string path) {
        path = $"Assets/Askowl/{assetType}/{path}";
        return Directory.Exists(path) ? path : null;
      }
      return exists("Editor/Template") ?? exists("Template") ?? exists("scripts/Template") ?? "";
    }

    private readonly Dictionary<string, SerializedObject> assets = new Dictionary<string, SerializedObject>();

    /// <a href=""></a> //#TBD#//
    protected SerializedObject Asset(string assetName) {
      if (!assets.ContainsKey(assetName)) throw new Exception($"No asset '{assetName}' set in CreateAssetDictionary");
      return assets[assetName];
    }

    /// <a href=""></a> //#TBD#//
    protected void SetActiveObject(string assetName) => Selection.activeObject = Asset(assetName).targetObject;

    /// <a href=""></a> //#TBD#//
    protected void CreateAssetDictionary(params (string name, Type asset)[] assetNameAndTypeList) {
      foreach (var entry in assetNameAndTypeList) {
        if (entry.asset.IsSubclassOf(typeof(ScriptableObject))) {
          var scriptableObject = CreateInstance(entry.asset);
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
    }

    /// <a href=""></a> //#TBD#//
    protected Type ScriptableType(string name) => CreateInstance(name)?.GetType();

    /// <a href=""></a> //#TBD#//
    protected void SetField(string assetName, string fieldName, SerializedObject fieldValue) =>
      SetField(assetName, fieldName, fieldValue.targetObject);

    /// <a href=""></a> //#TBD#//
    protected void SetField(string assetName, string fieldName, Object fieldValue) =>
      Field(assetName, fieldName).objectReferenceValue = fieldValue;

    /// <a href=""></a> //#TBD#//
    protected SerializedProperty Field(string assetName, string fieldName = "value") {
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
    protected T FindProperty<T>(string assetName, string fieldName = "value") where T : class =>
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
    protected void SaveAssetDictionary() {
      var manager = GetCustomAssetManager();
      foreach (var entry in assets) {
        if (entry.Value.targetObject.GetType().IsSubclassOf(typeof(ScriptableObject))) {
          string assetName = $"{destination}/{destinationName} {assetType} {entry.Key}.asset";
          AssetDatabase.CreateAsset(entry.Value.targetObject, assetName);
          if (entry.Key.EndsWith("Manager")) InsertIntoArrayField(manager, "managers", entry.Value.targetObject);
        }
        entry.Value.ApplyModifiedProperties();
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
        gameObject      = (GameObject) Instantiate(prefab, Vector3.zero, Quaternion.identity);
        gameObject.name = prefabName;
      }
      var component = gameObject.GetComponentInChildren(monoBehaviour);
      if (component == default) throw new Exception($"No component '{monoBehaviour.Name}' in '{gameObject.name}'1");
      var serialisedObject = new SerializedObject(component);
      assets[prefabName] = serialisedObject;
      return serialisedObject;
    }

    /// <a href=""></a> //#TBD#//
    protected virtual string GetDestinationPath() => EditorUtility.SaveFilePanel(
      $"Location for your new {assetType}", selectedPathInProjectView, "", "");

    private void SetDestination() {
      var dest = GetDestinationPath();
      if (dest != null) {
        if (string.IsNullOrWhiteSpace(destination = dest)) Fatal("Enter name for the new asset");
        if (Directory.Exists(destination)) {
          Fatal($"{destination} already exists. Please select a different name or project directory");
        }
        destination = destination.Substring(destination.IndexOf("/Assets/", StringComparison.Ordinal) + 1);
        Directory.CreateDirectory(destination);
      }
      destinationName = Path.GetFileNameWithoutExtension(destination);
    }

    /// <a href=""></a> //#TBD#//
    protected void Fatal(string msg) => throw new Exception(msg);

    /// <a href=""></a> //#TBD#//
    protected string[] ToDefinitions(string text) => csRegex.Split(text);

    /// <a href=""></a> //#TBD#//
    protected string ToTuple(string fields) {
      var pairs = ToDefinitions(fields);
      if (pairs.Length == 0) return null;
      if (pairs.Length == 1) return pairs[0];

      if ((pairs.Length < 4) || ((pairs.Length % 2) != 0))
        throw new Exception($"'{fields}' must be single type or pairs of (type, name)");

      builder.Clear().Append("(");
      builder.Append($"{pairs[0]} {pairs[1]}");
      for (int i = 2; i < pairs.Length; i += 2) builder.Append($",{pairs[i]} {pairs[i + 1]}");
      return builder.Append($")").ToString();
    }
    private readonly        StringBuilder builder = new StringBuilder();
    private static readonly Regex         csRegex = new Regex(@"\s*;\s*|\s*,\s*|\s+", RegexOptions.Singleline);

    /// <a href=""></a> //#TBD#//
    private static string GetSelectedPathInProjectView() {
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

    /// <a href=""></a> //#TBD#//
    [CustomEditor(typeof(AssetWizard), true)] public class AssetWizardEditor : Editor {
      public override void OnInspectorGUI() {
        GUI.SetNextControlName("FirstWizardField");
        if (GUILayout.Button("Clear")) {
          ((AssetWizard) target).Clear();
          GUI.FocusControl(null);
        }
        serializedObject.Update();
        DrawDefaultInspector();
        if (GUILayout.Button("Create")) ((AssetWizard) target).Create();
        serializedObject.ApplyModifiedProperties();
        GUIUtility.ExitGUI();
      }
    }
    /// <a href=""></a> //#TBD#//
    protected virtual void Create() => throw new NotImplementedException();
    /// <a href=""></a> //#TBD#//
    protected virtual void Clear() => throw new NotImplementedException();
  }
}