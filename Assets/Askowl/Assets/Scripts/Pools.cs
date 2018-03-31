namespace Askowl {
  using System;
  using System.Collections.Generic;
  using JetBrains.Annotations;
  using UnityEngine;
  using Object = UnityEngine.Object;

  public sealed class Pool : Queue<GameObject> {
    public GameObject Master;

    private int count = 0;

    public GameObject Fetch() {
      if (Count > 0) return Dequeue();

      GameObject clone = Object.Instantiate(Master);
      clone.name = string.Format("{0} (clone {1})", Master.name, ++count);
      return clone;
    }
  }

  public sealed class Pools : MonoBehaviour {
    private void OnEnable() {
      DontDestroyOnLoad(target: this);

      for (int i = 0; i < transform.childCount; ++i) {
        CreatePool(transform.GetChild(i).gameObject);
      }
    }

    private void CreatePool([NotNull] GameObject master) {
      var poolName = string.Format("{0} Pool", master.name);

      if (!Queues.ContainsKey(poolName)) {
        PoolMonitor poolMonitor = master.AddComponent<PoolMonitor>();
        poolMonitor.Master = master;
        Queues[poolName]   = new Pool {Master = master};
      } else {
        Debug.LogFormat("Duplicate Pools for {0}", master.name);
      }
    }

    [CanBeNull]
    public static T Get<T>([CanBeNull] string    name     = null,
                           Vector3               position = default(Vector3),
                           Quaternion            rotation = default(Quaternion),
                           [CanBeNull] Transform parent   = default(Transform),
                           bool                  enable   = true) where T : Component {
      name = name ?? typeof(T).Name;
      GameObject clone = Get(name, position, rotation, parent, enable);
      return (clone == null) ? null : clone.GetComponent<T>();
    }

    [CanBeNull]
    public static GameObject Get(string                name,
                                 Vector3               position = default(Vector3),
                                 Quaternion            rotation = default(Quaternion),
                                 [CanBeNull] Transform parent   = null,
                                 bool                  enable   = true) {
      Pool pool = PoolFor(name);
      if (pool == null) return null;

      GameObject clone = pool.Fetch();
      clone.gameObject.SetActive(enable);
      clone.transform.SetParent(parent ?? pool.Master.transform.parent);
      clone.transform.position = position;
      clone.transform.rotation = rotation;
      return clone;
    }

    private static void Return(GameObject gameObject) {
      try {
        Pool pool = PoolFor(gameObject);
        pool.Enqueue(gameObject);
      } catch (Exception e) {
        Debug.LogFormat("**** Error: {0} was not created in a pool ({1})", gameObject.name, e);
      }
    }

    public static Pool PoolFor(GameObject gameObject) {
      return PoolFor(gameObject.GetComponent<PoolMonitor>().Master.name);
    }

    public static Pool PoolFor(string name) {
      string poolName = string.Format("{0} Pool", name);

      return Queues.ContainsKey(poolName) ? Queues[poolName] : null;
    }

    private sealed class PoolDict : Dictionary<string, Pool> { }

    private static readonly PoolDict Queues = new PoolDict();

    private sealed class PoolMonitor : MonoBehaviour {
      public  GameObject Master;
      private void       OnDisable() { Return(gameObject); }
    }
  }
}