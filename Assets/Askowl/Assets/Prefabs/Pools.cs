namespace Askowl {
  using System.Collections.Generic;
  using JetBrains.Annotations;
  using UnityEngine;

  public sealed class Pools : MonoBehaviour {
    private void Awake() {
      DontDestroyOnLoad(target: this);

      for (int i = 0; i < transform.childCount; ++i) {
        CreatePool(transform.GetChild(i).gameObject);
      }
    }

    private void CreatePool([NotNull] GameObject master) {
      var poolName = string.Format("{0} Pool", master.name);

      if (!Queues.ContainsKey(poolName)) {
        PoolMonitor poolMonitor = master.AddComponent<PoolMonitor>();
        Queues[poolName] = new PoolQueue {master = master};
        poolMonitor.name = poolName;
      } else {
        Debug.LogFormat("Duplicate Pools for {0}", master.name);
      }
    }

    private static void Return(GameObject gameObject) { PoolFor(gameObject).Enqueue(gameObject); }

    private static PoolQueue PoolFor(GameObject gameObject) {
      return Queues[gameObject.GetComponent<PoolMonitor>().name];
    }

    private static readonly PoolDict Queues = new PoolDict();

    private sealed class PoolQueue : Queue<GameObject> {
      public GameObject master;
    }

    private sealed class PoolDict : Dictionary<string, PoolQueue> { }

    private sealed class PoolMonitor : MonoBehaviour {
      private void OnDisable() { Return(gameObject); }
    }
  }
}