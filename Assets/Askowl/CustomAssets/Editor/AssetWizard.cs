// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using CustomAsset;
using UnityEditor;
using UnityEngine;
using String = CustomAsset.Constant.String;

namespace Askowl {
  public abstract class AssetWizard : Manager {
    protected static string assetType, destination, destinationName;

    [SerializeField, Tooltip("Optional: Overrides default project directory")]
    protected string destinationPath;
    protected               Jit<string>        selectedPathInProjectView = Jit<string>.Instance(getProjectFolder);
    private static readonly Func<bool, string> getProjectFolder          = _ => AssetDb.ProjectFolder();

    protected void CreateAssets(string newAssetType, string key, string destinationDirectory = null) {
      assetType = newAssetType;
      SetDestination(destinationDirectory);
      bool hasSource = ProcessAllFiles(@"cs|txt");
      AssetDatabase.ImportAsset(
        Path.GetDirectoryName(destination)
      , ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ImportRecursive);

      if (hasSource) {
        PlayerPrefs.SetString($"{key}.AssetEditor.destination", $"{destination}/{destinationName} ");
        Debug.Log($"      Scripts for `{destination}/{destinationName}` waiting on rebuild for basic assets...");
      }
    }

    protected virtual bool ProcessAllFiles(string textAssetTypes) {
      string[] sources                                    = AssetDatabase.FindAssets("", new[] {TemplatePath()});
      for (int i = 0; i < sources.Length; i++) sources[i] = AssetDatabase.GUIDToAssetPath(sources[i]);
      return ProcessFiles(textAssetTypes, sources);
    }

    protected bool ProcessFiles(string textAssetTypes, params string[] files) {
      bool  hasSource           = false;
      Regex textAssetTypesRegex = new Regex($@"\.({textAssetTypes})$");
      using (var template = Template.Instance) {
        for (int i = 0; i < files.Length; i++) {
          var fileName = Path.GetFileName(files[i]) ?? "";
          if (File.Exists(files[i])) {
            var filePath = $"{destination}/{fileName}";
            if (textAssetTypesRegex.IsMatch(files[i])) {
              if (files[i].EndsWith(".cs")) hasSource             = true;
              if (!fileName.StartsWith(destinationName)) filePath = $"{destination}/{destinationName}{fileName}";
              if (File.Exists(filePath) && filePath != files[i])
                Fatal($"{filePath} already exists. Select a different name or directory");
              File.WriteAllText(filePath, FillTemplate(template, File.ReadAllText(files[i])));
            } else {
              if (File.Exists(filePath)) Fatal($"{filePath} already exists. Select a different name or directory");
              File.Copy(files[i], filePath);
            }
          }
        }
      }
      return hasSource;
    }

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

    protected virtual string GetDestinationPath(string basePath) => basePath;

    protected string AskForDestinationPath(string basePath) =>
      EditorUtility.SaveFilePanel($"Location for your new {assetType}", basePath, "", "");

    private string SetDestination(string dest) {
      if (string.IsNullOrWhiteSpace(destinationPath))
        destinationPath = dest ?? (string) selectedPathInProjectView ?? "/Assets";
      dest = GetDestinationPath(destinationPath);
      if (dest != null) {
        if (string.IsNullOrWhiteSpace(destination = dest)) Fatal("Enter name for the new asset");
        destination = destination.Substring(destination.IndexOf("/Assets/", StringComparison.Ordinal) + 1);
        Directory.CreateDirectory(destination);
      }
      destinationName = Path.GetFileNameWithoutExtension(destination);
      return destination;
    }

    protected void Fatal(string msg) => throw new Exception(msg);

    protected string[] ToDefinitions(string text) => csRegex.Split(text);

    protected string ToTuple(string fields) {
      var pairs = ToDefinitions(fields);
      if (pairs.Length == 0) return null;
      if (pairs.Length == 1) return pairs[0];

      if ((pairs.Length < 4) || ((pairs.Length % 2) != 0))
        throw new Exception($"'{fields}' must be single type or pairs of (type, name)");

      builder.Clear().Append("(");
      builder.Append($"{pairs[0]} {pairs[1]}");
      for (int i = 2; i < pairs.Length; i += 2) builder.Append($",{pairs[i]} {pairs[i + 1]}");
      return builder.Append(")").ToString();
    }
    private readonly        StringBuilder builder = new StringBuilder();
    private static readonly Regex         csRegex = new Regex(@"\s*;\s*|\s*,\s*|\s+", RegexOptions.Singleline);

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
//        GUIUtility.ExitGUI();
      }
    }
    public virtual void Create()                => throw new NotImplementedException();
    public virtual void Clear(string dest = "") => throw new NotImplementedException();
  }
}