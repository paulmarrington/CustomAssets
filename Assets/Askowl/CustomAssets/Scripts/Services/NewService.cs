// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages

#if UNITY_EDITOR
using System;
using System.IO;
using Askowl;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using Object = UnityEngine.Object;
using String = CustomAsset.Constant.String;

namespace CustomAsset.Services {
  /// <a href=""></a> //#TBD#//
  public class NewService : Base {
    [MenuItem("Assets/Create/Custom Assets/Services/Create Service")]
    private static void Phase1() {
      var templatePath    = TemplatePath();
      var sources         = AssetDatabase.FindAssets("", new[] {templatePath});
      var destinationPath = EditorUtility.SaveFilePanel("Save Your New Service", GetSelectedPathOrFallback(), "", "");
      var serviceName     = Path.GetFileNameWithoutExtension(destinationPath);
      if (string.IsNullOrEmpty(destinationPath)) return;
      if (Directory.Exists(destinationPath)) {
        Debug.LogError($"{destinationPath} already exists. Please select a different name or project directory");
        return;
      }
      Directory.CreateDirectory(destinationPath);

      using (var template = Template.Instance) {
        template.Substitute("Template", serviceName);
        for (int i = 0; i < sources.Length; i++) {
          var sourcePath = AssetDatabase.GUIDToAssetPath(sources[i]);
          if (!File.Exists(sourcePath)) continue;
          var text     = template.Process(File.ReadAllText(sourcePath));
          var fileName = Path.GetFileNameWithoutExtension(sourcePath);
          File.WriteAllText($"{destinationPath}/{serviceName}{fileName}.cs", text);
        }

        destinationPath = destinationPath.Substring(destinationPath.IndexOf("/Assets/", StringComparison.Ordinal) + 1);
        AssetDatabase.ImportAsset(
          Path.GetDirectoryName(destinationPath),
          ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ImportRecursive);
        PlayerPrefs.SetString("CustomAssetServiceBuildServiceName",     serviceName);
        PlayerPrefs.SetString("CustomAssetServiceBuildDestinationPath", destinationPath);

        Debug.Log($"Scripts for `{destinationPath}` waiting on rebuild for basic assets...");
        // Will continue in `OnScriptsReloaded`
      }
    }

    [DidReloadScripts] private static void Phase2() {
      if (!PlayerPrefs.HasKey("CustomAssetServiceBuildServiceName")) return;
      var serviceName = PlayerPrefs.GetString("CustomAssetServiceBuildServiceName");
      PlayerPrefs.DeleteKey("CustomAssetServiceBuildServiceName");
      var destinationPath = PlayerPrefs.GetString("CustomAssetServiceBuildDestinationPath");
      PlayerPrefs.DeleteKey("CustomAssetServiceBuildDestinationPath");

      Debug.Log($"... rebuild complete, creating basic assets for `{destinationPath}`");
      Debug.Log($"  1. Update `{serviceName}Context.cs` with context data for your service. Don't forget `Equals`");
      Debug.Log($"  2. Fill `{serviceName}Service.cs` with supporting code and abstract or default interface methods");
      Debug.Log($"  3. Fill `{serviceName}ServiceForMock.cs` interface methods for mocking");
      Debug.Log("  4. Write some tests to prove that the mock interface works as expected");
      Debug.Log($"  5. Write `{serviceName}ServiceForSource.cs` where `Source` is a service source");
      Debug.Log("  6. Create an asset for each source and update relevant data accordingly");
      Debug.Log("  7. Repeat [5] for each source");
      Debug.Log($"  8. Add source service assets to `{serviceName}Elector");
      Debug.Log($"  9. Create Context assets and update `{serviceName}Services` in GameObject `Managers` as needed");

      Environment mockEnvironment =
        AssetDatabase.LoadAssetAtPath<Environment>(
          "Assets/Askowl/CustomAssets/Scripts/Services/Environments/Mock.asset");

      var servicesManager  = CreateInstance(serviceName, "ServicesManager");
      var context          = CreateInstance(serviceName, "Context");
      var serviceConnector = CreateInstance(serviceName, "ServiceConnector");
      var serviceForMock   = CreateInstance(serviceName, "ServiceConnectorForMock");

      var servicesManagerSerializedObject = new SerializedObject(servicesManager);
      var contextSerializedObject         = new SerializedObject(context);
      var serviceSerializedObject         = new SerializedObject(serviceConnector);
      var serviceForMockSerializedObject  = new SerializedObject(serviceForMock);

      SetField(servicesManagerSerializedObject, "context", context);
      InsertIntoArrayField(servicesManagerSerializedObject, "services", serviceForMock);
      SetField(contextSerializedObject, "environment", mockEnvironment);

      AssetDatabase.CreateAsset(servicesManager, $"{destinationPath}/{serviceName}ServicesManager.asset");
      AssetDatabase.CreateAsset(context,         $"{destinationPath}/{serviceName}MockContext.asset");
      AssetDatabase.CreateAsset(serviceForMock,  $"{destinationPath}/{serviceName}ServiceForMock.asset");

      servicesManagerSerializedObject.ApplyModifiedProperties();
      contextSerializedObject.ApplyModifiedProperties();
      serviceSerializedObject.ApplyModifiedProperties();
      serviceForMockSerializedObject.ApplyModifiedProperties();
      AssetDatabase.SaveAssets();

      var managers = UnityEngine.GameObject.Find("/Service Managers");
      if (managers == default) {
        var prefab = Resources.Load("Managers");
        managers      = (UnityEngine.GameObject) Instantiate(prefab, Vector3.zero, Quaternion.identity);
        managers.name = "Service Managers";
      }
      var managersSerializedObject = new SerializedObject(managers.GetComponent<Managers>());
      InsertIntoArrayField(managersSerializedObject, "managers", servicesManager);
    }

    private static void SetField(SerializedObject asset, string fieldName, Object fieldValue) {
      var serialisedProperty                                                  = FindProperty(asset, fieldName);
      if (serialisedProperty != null) serialisedProperty.objectReferenceValue = fieldValue;
    }

    private static void InsertIntoArrayField(SerializedObject asset, string fieldName, Object fieldValue) {
      var serialisedProperty = FindProperty(asset, fieldName);
      if (serialisedProperty != null) {
        serialisedProperty.InsertArrayElementAtIndex(0);
        var arrayElementSerialisedProperty = serialisedProperty.GetArrayElementAtIndex(0);
        arrayElementSerialisedProperty.objectReferenceValue = fieldValue;
        asset.ApplyModifiedProperties();
      }
    }

    private static SerializedProperty FindProperty(SerializedObject asset, string name) {
      var serialisedProperty = asset.FindProperty(name);
      if (serialisedProperty == null) Debug.LogError($"No serialisable property {name} in {asset}");
      return serialisedProperty;
    }

    private static ScriptableObject CreateInstance(string serviceName, string scriptableObjectName) =>
      CreateInstance($"CustomAsset.Services.{serviceName}{scriptableObjectName}");

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