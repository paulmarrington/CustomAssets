// Copyright 2018,19 (C) paul@marrington.net http://www.askowl.net/unity-packages
using Askowl;
using UnityEditor;
using UnityEngine;

namespace CustomAsset {
  /// <a href=""></a> //#TBD#//
  public class Manager : Base {
    #region Load or Create with Script
    #if UNITY_EDITOR
    /// <a href="http://bit.ly/2RjdFF2">To Load managers during play-mode testing (without a scene)</a> //#TBD#//
    public static T Load<T>(string path) where T : Base {
      path = Objects.FindFile(path);
      if (path == null) return default;
      var customAsset = AssetDatabase.LoadAssetAtPath<T>(path);
      return customAsset;
    }

    /// <a href=""></a> //#TBD#//
    /// Warning: Changes path in project window. Get old path first with
    ///   var activeObject = Selection.activeObject;
    ///   var selectedPathInProjectView = AssetDatabase.GetAssetPath(Selection.activeObject);
    /// afterwards you can return with
    ///   EditorGUIUtility.PingObject(activeObject);
    /// but it is not perfect. Left project pane returns correctly, but right pane still points elsewhere
    public static T LoadOrCreate<T>(string path) where T : Base {
      var customAsset = Load<T>(path);
      if (customAsset != null) return customAsset;
      AssetDatabase.CreateAsset(CreateInstance<T>(), $"Assets/{path}");
      AssetDatabase.SaveAssets();
      AssetDatabase.Refresh();
      return customAsset;
    }
    #endif
    #endregion

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void InitialiseCustomAssetsFiber() =>
      // Permanently running fiber that waits until an asset requires initialising before looping back on itself.
      Fiber.Start.OnFixedUpdates.Begin.Do(
        _ => {
          while (!AssetsWaitingInitialisation.Empty) AssetsWaitingInitialisation.Pop().Initialiser();
        }, "CustomAsset Initialiser").WaitFor(InitialiseAssetEmitter).Again.Finish();
  }
}