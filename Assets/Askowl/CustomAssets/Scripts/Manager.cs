// Copyright 2018,19 (C) paul@marrington.net http://www.askowl.net/unity-packages
using Askowl;
using UnityEditor;
using UnityEngine;

namespace CustomAsset {
  /// <a href=""></a> //#TBD#//
  public class Manager : Base {
    #if UNITY_EDITOR
    /// <a href="http://bit.ly/2RjdFF2">To Load managers during play-mode testing (without a scene)</a> //#TBD#//
    public static T Load<T>(string path) where T : Base {
      path = Objects.FindFile(path);
      if (path == null) return default;
      var customAsset = AssetDatabase.LoadAssetAtPath<T>(path);
      return customAsset;
    }
    #endif

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void InitialiseCustomAssetsFiber() =>
      Fiber.Start.OnFixedUpdates.Begin.Do(
        _ => {
          while (!AssetsWaitingInitialisation.Empty) AssetsWaitingInitialisation.Pop().Initialiser();
        }, "CustomAsset Initialiser").WaitFor(InitialiseAssetEmitter).Again.Finish();
  }
}