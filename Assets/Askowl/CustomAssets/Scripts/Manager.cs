// Copyright 2018,19 (C) paul@marrington.net http://www.askowl.net/unity-packages
using Askowl;
using UnityEditor;
using UnityEngine;

namespace CustomAsset {
  /// <a href=""></a> //#TBD#//
  public class Manager : Base {
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void InitialiseCustomAssetsFiber() =>
      // Permanently running fiber that waits until an asset requires initialising before looping back on itself.
      Fiber.Start().OnFixedUpdates.Begin.Do(
        _ => {
          while (!AssetsWaitingInitialisation.Empty) AssetsWaitingInitialisation.Pop().Initialiser();
        }, "CustomAsset Initialiser").WaitFor(InitialiseAssetEmitter).Again.Finish();
  }
}