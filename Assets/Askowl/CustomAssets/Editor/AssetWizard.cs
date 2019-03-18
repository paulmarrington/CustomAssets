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
  /// <a href=""></a> //#TBD#//
  public struct Jit<T> {
    private T             value;
    private bool          initialised;
    private Func<bool, T> factory;
    /// <a href=""></a> //#TBD#//
    public static Jit<T> Instance(Func<bool, T> factory) => new Jit<T> {factory = factory};
    /// <a href=""></a> //#TBD#//
    public T Value => (initialised) ? value : value = factory(initialised = true);
    /// <a href=""></a> //#TBD#//
    public static implicit operator T(Jit<T> jit) => jit.value;
  }
  /// <a href=""></a> //#TBD#//
  public abstract class AssetWizard : Manager {
    /// <a href=""></a> //#TBD#//
    protected static string assetType, destination, destinationName;
    /// <a href=""></a> //#TBD#//
    protected Jit<string> selectedPathInProjectView = Jit<string>.Instance(getProjectFolder);
    private static readonly Func<bool, string> getProjectFolder = _ => AssetDb.ProjectFolder();

    /// <a href=""></a> //#TBD#//
    protected void CreateAssets(string newAssetType, string key) {
      assetType = newAssetType;
      SetDestination();
      bool hasSource = ProcessAllFiles(@"cs|txt");
      AssetDatabase.ImportAsset(
        Path.GetDirectoryName(destination)
      , ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ImportRecursive);

      if (hasSource) {
        PlayerPrefs.SetString($"{key}AssetEditor.destination", $"{destination}/{destinationName} ");
        Debug.Log($"Scripts for `{destination}` waiting on rebuild for basic assets...");
        // Will continue in `OnScriptsReloaded` if there is source to recompile
      }
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
      return builder.Append(")").ToString();
    }
    private readonly        StringBuilder builder = new StringBuilder();
    private static readonly Regex         csRegex = new Regex(@"\s*;\s*|\s*,\s*|\s+", RegexOptions.Singleline);

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
//        GUIUtility.ExitGUI();
      }
    }
    /// <a href=""></a> //#TBD#//
    public virtual void Create() => throw new NotImplementedException();
    /// <a href=""></a> //#TBD#//
    protected virtual void Clear() => throw new NotImplementedException();
  }
}