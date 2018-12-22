using Askowl;
using UnityEditor;
using UnityEngine;

namespace CustomAsset {
  /// <a href=""></a> //#TBD#//
  public class Manager : Base {
    public static T Load<T>(string path) where T : Base {
      path = Objects.FindFile(path);
      if (path == null) return default;
      var customAsset = AssetDatabase.LoadAssetAtPath<T>(path);
      return customAsset;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnAfterSceneLoadRuntimeMethod() {
      void initialiser(Fiber fiber) {
        while (!AssetsWaitingInitialisation.Empty) AssetsWaitingInitialisation.Pop().Initialiser();
      }
      Fiber.Start.OnFixedUpdates.Begin.Do(initialiser).WaitFor(InitialiseAssetEmitter).Again.Finish();
    }
    private static bool managersLoaded;

    [MenuItem("GameObject/Create Managers")]
    public static void CreateManagersGameObject() {
      var prefab     = Resources.Load("Managers");
      var gameObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
      gameObject.name = prefab.name;
    }
  }
}