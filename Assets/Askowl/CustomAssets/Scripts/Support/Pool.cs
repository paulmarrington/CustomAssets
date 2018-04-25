namespace Askowl {
  using System.Collections;
  using System.Collections.Generic;
  using JetBrains.Annotations;
  using UnityEngine;

  public sealed class Pool : MonoBehaviour {
    private void OnEnable() {
      DontDestroyOnLoad(target: gameObject);
      Transform[] children = GetComponentsInChildren<Transform>();

      // skip first as it is the Pool itself
      for (int i = 1; i < children.Length; ++i) {
        CreatePool(children[i].gameObject);
      }

      StartCoroutine(SetParentOnReturn());
    }

    private void CreatePool([NotNull] GameObject master) {
      var poolName = string.Format("{0} Pool", master.name);

      if (!Queues.ContainsKey(poolName)) {
        PoolMonitor poolMonitor = master.AddComponent<PoolMonitor>();
        poolMonitor.MasterName = master.name;
        Queues[poolName]       = new PoolQueue {Master = master};
        GameObject poolRoot = new GameObject(poolName);
        poolRoot.transform.parent = transform;
        poolMonitor.PoolRoot      = poolRoot.transform;
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

      clone.transform.SetParent(parent ?? poolMonitor.PoolRoot);
      clone.transform.position  = position;
      clone.transform.rotation  = rotation;
      poolMonitor.PoolOnDisable = poolOnDisable;
      clone.gameObject.SetActive(enable);
      poolMonitor.InPool   = false;
      poolMonitor.OkToPool = true;
      return clone;
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static void Return([NotNull] GameObject clone) { returns.Enqueue(clone); }

    private static readonly Queue<GameObject> returns = new Queue<GameObject>();

    private IEnumerator SetParentOnReturn() {
      while (true) {
        while (returns.Count > 0) {
          GameObject  clone       = returns.Dequeue();
          PoolMonitor poolMonitor = clone.GetComponent<PoolMonitor>();
          PoolQueue   pool        = PoolFor(poolMonitor.MasterName);

          if (pool != null) {
            poolMonitor.InPool = poolMonitor.OkToPool = true;
            pool.Enqueue(clone);
            clone.transform.SetParent(poolMonitor.PoolRoot);
          } else {
            Debug.LogErrorFormat("**** Error: {0} was not created in a pool", clone.name);
          }

          clone.SetActive(false);
        }

        yield return null;
      }
    }

    [CanBeNull]
    public static PoolQueue PoolFor(string name) {
      string poolName = string.Format("{0} Pool", name);
      return Queues.ContainsKey(poolName) ? Queues[poolName] : null;
    }

    [CanBeNull, UsedImplicitly]
    private static PoolQueue PoolFor([NotNull] GameObject clone) {
      return PoolFor(clone.GetComponent<PoolMonitor>().MasterName);
    }

    private sealed class PoolDict : Dictionary<string, PoolQueue> { }

    private static readonly PoolDict Queues = new PoolDict();

    private sealed class PoolMonitor : MonoBehaviour {
      public bool      PoolOnDisable;
      public bool      InPool, OkToPool = true;
      public string    MasterName;
      public Transform PoolRoot;

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