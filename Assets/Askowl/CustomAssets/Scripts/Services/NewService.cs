// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages

#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using Object = UnityEngine.Object;
using String = CustomAsset.Constant.String;

namespace CustomAsset.Services {
  /// <a href=""></a> //#TBD#//
  public class NewService : Base {
    [MenuItem("Assets/Create/Custom Assets/Services/New Service")]
    private static void NewServiceMenuItem() {
      var templatePath    = TemplatePath();
      var sources         = AssetDatabase.FindAssets("", new[] {templatePath});
      var destinationPath = EditorUtility.SaveFilePanel("Save Your New Service", GetSelectedPathOrFallback(), "", "");
      var serviceName     = Path.GetFileNameWithoutExtension(destinationPath);
      if (string.IsNullOrEmpty(destinationPath)) return;
      if (!Directory.Exists(destinationPath)) Directory.CreateDirectory(destinationPath);

      using (var template = Askowl.Template.Instance) {
        template.Substitute("Template", serviceName);
        for (int i = 0; i < sources.Length; i++) {
          var sourcePath = AssetDatabase.GUIDToAssetPath(sources[i]);
          if (!File.Exists(sourcePath)) continue;
          var text     = template.Process(File.ReadAllText(sourcePath));
          var fileName = Path.GetFileNameWithoutExtension(sourcePath);
          File.WriteAllText($"{destinationPath}/{fileName}.cs", text);
        }

        destinationPath = destinationPath.Substring(destinationPath.IndexOf("/Assets/", StringComparison.Ordinal) + 1);
        AssetDatabase.ImportAsset(
          Path.GetDirectoryName(destinationPath),
          ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ImportRecursive);
        PlayerPrefs.SetString("CustomAssetServiceBuildServiceName",     serviceName);
        PlayerPrefs.SetString("CustomAssetServiceBuildDestinationPath", destinationPath);

        Debug.Log($"Scripts for {destinationPath} waiting on rebuild for basic assets...");
      }
    }

    [DidReloadScripts] private static void OnScriptsReloaded() {
      if (!PlayerPrefs.HasKey("CustomAssetServiceBuildServiceName")) return;
      var serviceName = PlayerPrefs.GetString("CustomAssetServiceBuildServiceName");
      PlayerPrefs.DeleteKey("CustomAssetServiceBuildServiceName");
      var destinationPath = PlayerPrefs.GetString("CustomAssetServiceBuildDestinationPath");
      PlayerPrefs.DeleteKey("CustomAssetServiceBuildDestinationPath");

      Debug.Log($"... rebuild complete, creating basic assets for {destinationPath}");

      Environment mockEnvironment =
        AssetDatabase.LoadAssetAtPath<Environment>(
          "Assets/Askowl/CustomAsset/Scripts/Services/Environments/Mock.asset");

      var referent = CreateInstance(serviceName, "Referent");
      var context  = CreateInstance(serviceName, "Context");
      var service  = CreateInstance(serviceName, "Service");
      var elector  = CreateInstance(serviceName, "Elector");

      var referentSo = new SerializedObject(referent);
      var contextSo  = new SerializedObject(context);
      var serviceSo  = new SerializedObject(service);
      var electorSo  = new SerializedObject(elector);

      SetField(referentSo, "elector", elector);
      SetField(referentSo, "context", context);
      SetArrayField(electorSo, "services", service);
      SetField(contextSo, "environment", mockEnvironment);

      AssetDatabase.CreateAsset(referent, $"{destinationPath}/Referent.asset");
      AssetDatabase.CreateAsset(context,  $"{destinationPath}/MockContext.asset");
      AssetDatabase.CreateAsset(elector,  $"{destinationPath}/Elector.asset");
      AssetDatabase.CreateAsset(service,  $"{destinationPath}/MockService.asset");

      referentSo.ApplyModifiedProperties();
      electorSo.ApplyModifiedProperties();
      contextSo.ApplyModifiedProperties();
      serviceSo.ApplyModifiedProperties();
      AssetDatabase.SaveAssets();
    }

    private static void SetField(SerializedObject asset, string fieldName, Object fieldValue) {
      var serialisedProperty                                                  = FindProperty(asset, fieldName);
      if (serialisedProperty != null) serialisedProperty.objectReferenceValue = fieldValue;
    }

    private static void SetArrayField(SerializedObject asset, string fieldName, Object fieldValue) {
      var serialisedProperty = FindProperty(asset, fieldName);
      if (serialisedProperty != null) {
        serialisedProperty.InsertArrayElementAtIndex(0);
        var arrayElementSerialisedProperty = serialisedProperty.GetArrayElementAtIndex(0);
        arrayElementSerialisedProperty.objectReferenceValue = fieldValue;
      }
    }

    private static SerializedProperty FindProperty(SerializedObject asset, string name) {
      var serialisedProperty = asset.FindProperty(name);
      if (serialisedProperty == null) Debug.LogError($"No serialisable property {name} in {asset}");
      return serialisedProperty;
    }

    private static ScriptableObject CreateInstance(string serviceName, string scriptableObjectName) =>
      CreateInstance($"CustomAsset.Services.{serviceName}.{scriptableObjectName}");

    private static string TemplatePath() {
      var paths = AssetDatabase.FindAssets("CustomAssetServiceTemplatePath");
      for (int i = 0; i < paths.Length; i++) {
        var path = AssetDatabase.GUIDToAssetPath(paths[i]);
        if (path.IndexOf("Askowl", StringComparison.Ordinal) == -1) return path;
      }
      if (paths.Length == 0) return "";
      return AssetDatabase.LoadAssetAtPath<String>(AssetDatabase.GUIDToAssetPath(paths[0]));
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
  }
}
#endif