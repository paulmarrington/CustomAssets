// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages
using Askowl;
using UnityEditor;
using UnityEngine;

namespace CustomAsset {
  /// <a href="http://bit.ly/2RjdJog">Logic-only manager custom asset superclass</a> //#TBD#//
  public class Manager : Base {
    /// <a href="http://bit.ly/2RjdFF2">To Load managers during play-mode testing (without a scene)</a> //#TBD#//
    public static T Load<T>(string path) where T : Base {
      path = Objects.FindFile(path);
      if (path == null) return default;
      var customAsset = AssetDatabase.LoadAssetAtPath<T>(path);
      return customAsset;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void InitialiseCustomAssetsFiber() {
      void initialiser(Fiber fiber) {
        while (!AssetsWaitingInitialisation.Empty) AssetsWaitingInitialisation.Pop().Initialiser();
      }
      Fiber.Start.OnFixedUpdates.Begin.Do(initialiser).WaitFor(InitialiseAssetEmitter).Again.Finish();
    }
    private static bool managersLoaded;
  }
}