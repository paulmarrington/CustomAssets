namespace Askowl {
  using System.Collections.Generic;
  using JetBrains.Annotations;
  using UnityEngine;

  public sealed class Pool : MonoBehaviour {
    private void OnEnable() {
      DontDestroyOnLoad(target: gameObject);

      for (int i = 0; i < transform.childCount; ++i) {
        CreatePool(transform.GetChild(i).gameObject);
      }
    }

    private void CreatePool([NotNull] GameObject master) {
      var poolName = string.Format("{0} Pool", master.name);

      if (!Queues.ContainsKey(poolName)) {
        PoolMonitor poolMonitor = master.AddComponent<PoolMonitor>();
        poolMonitor.MasterName = master.name;
        Queues[poolName]       = new PoolQueue {Master = master};
      } else {
        Debug.LogFormat("Duplicate Pools for {0}", master.name);
      }
    }

    [CanBeNull]
    public static T Acquire<T>([CanBeNull] string    name          = null,
                               Vector3               position      = default(Vector3),
                               Quaternion            rotation      = default(Quaternion),
                               [CanBeNull] Transform parent        = default(Transform),
                               bool                  enable        = true,
                               bool                  poolOnDisable = true)
      where T : Component {
      name = name ?? typeof(T).Name;
      GameObject clone = Acquire(name, position, rotation, parent, enable, poolOnDisable);
      return (clone == null) ? null : clone.GetComponent<T>();
    }

    [CanBeNull]
    public static GameObject Acquire(string                name,
                                     Vector3               position      = default(Vector3),
                                     Quaternion            rotation      = default(Quaternion),
                                     [CanBeNull] Transform parent        = null,
                                     bool                  enable        = true,
                                     bool                  poolOnDisable = true) {
      PoolQueue pool = PoolFor(name);
      if (pool == null) return null;

      GameObject  clone;
      PoolMonitor poolMonitor;

      do {
        clone       = pool.Fetch();
        poolMonitor = clone.gameObject.GetComponent<PoolMonitor>();
      } while (!poolMonitor.OkToPool);

      clone.transform.SetParent(parent ?? pool.Master.transform.parent);
      clone.transform.position  = position;
      clone.transform.rotation  = rotation;
      poolMonitor.PoolOnDisable = poolOnDisable;
      clone.gameObject.SetActive(enable);
      poolMonitor.InPool   = false;
      poolMonitor.OkToPool = true;
      return clone;
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static void Return([NotNull] GameObject gameObject) {
      PoolMonitor poolMonitor = gameObject.GetComponent<PoolMonitor>();
      PoolQueue   pool        = PoolFor(poolMonitor.MasterName);

      if (pool != null) {
        poolMonitor.InPool = poolMonitor.OkToPool = true;
        poolMonitor.gameObject.SetActive(false);
        pool.Enqueue(gameObject);
      } else {
        Debug.LogErrorFormat("**** Error: {0} was not created in a pool", gameObject.name);
      }

      gameObject.SetActive(false);
    }

    [CanBeNull]
    public static PoolQueue PoolFor(string name) {
      string poolName = string.Format("{0} Pool", name);
      return Queues.ContainsKey(poolName) ? Queues[poolName] : null;
    }

    [CanBeNull, UsedImplicitly]
    private static PoolQueue PoolFor([NotNull] GameObject gameObject) {
      return PoolFor(gameObject.GetComponent<PoolMonitor>().MasterName);
    }

    private sealed class PoolDict : Dictionary<string, PoolQueue> { }

    private static readonly PoolDict Queues = new PoolDict();

    private sealed class PoolMonitor : MonoBehaviour {
      public bool   PoolOnDisable;
      public bool   InPool, OkToPool = true;
      public string MasterName;

//      private void Awake() { DontDestroyOnLoad(gameObject); }

      private void OnDisable() {
        OkToPool = true;
        if (PoolOnDisable && !InPool) Return(gameObject);
      }

      private void OnEnable() {
        if (InPool || !PoolOnDisable) return;

        OkToPool = false;
        Debug.LogErrorFormat("Pooled '{0}' should not be reused", gameObject.name);
      }
    }

    public sealed class PoolQueue : Queue<GameObject> {
      public GameObject Master;

      private int count = 0;

      [NotNull]
      public GameObject Fetch() {
        GameObject clone;

        if (Count > 0) {
          clone = Dequeue();
        } else {
          clone      = Instantiate(Master);
          clone.name = string.Format("{0} (clone {1})", Master.name, ++count);
        }

        return clone;
      }
    }
  }
}