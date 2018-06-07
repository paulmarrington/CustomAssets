// With thanks to Jason Weimann  -- jason@unity3d.college

namespace CustomAsset {
  using System.Collections;
  using System.Collections.Generic;
  using JetBrains.Annotations;
  using UnityEngine;

  /// <inheritdoc />
  /// <summary>
  /// Monobehaviour to provide pooling functionality for child GameObjects. Tob e eligible they must be created with `Acquire` and disabled but not destroyed.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#asset-pooling">More...</a></remarks>
  [HelpURL("http://customasset.marrington.net#asset-pooling")]
  public sealed class Pool : MonoBehaviour {
    private bool isPoolContainer;

    private void OnEnable() {
      if (gameObject.name.EndsWith("Pool") || (GetComponents<Component>().Length <= 2)) {
        isPoolContainer = true;
        // This is a list of predefined game objects to be pooled
        DontDestroyOnLoad(target: gameObject);
        Transform[] children = GetComponentsInChildren<Transform>();

        // skip first as it is the Pool itself
        for (int i = 1; i < children.Length; ++i) {
          CreatePoolQueue(children[i].gameObject);
        }

        StartCoroutine(SetParentOnReturn());
      } else if (!Queues.ContainsKey(gameObject.name)) {
        Destroy(this);
        PoolFor(gameObject); // we are inside a game object that wants to be pooled
      }
    }

    private PoolQueue CreatePoolQueue(GameObject master) {
      var poolName = string.Format("{0} Pool", master.name);

      if (!Queues.ContainsKey(poolName)) {
        PoolMonitor poolMonitor = master.AddComponent<PoolMonitor>();
        poolMonitor.MasterName = master.name;
        Queues[poolName]       = new PoolQueue {Master = master};
        GameObject poolRoot = new GameObject(poolName);
        poolRoot.transform.parent = transform;
        master.transform.parent   = poolMonitor.PoolRoot = poolRoot.transform;
        return Queues[poolName];
      }

      return null;
    }

    /// <summary>
    /// Use `Pool.Acquire&lt;T>()` to get a reference to a cloned gameObject instead of `Instantiate&lt;T>(). All parameters are optional.
    /// </summary>
    /// <param name="name">Name of cloned gameObject - defaults to name of GameObject Type</param>
    /// <param name="position">Position for clone in game space - defaults to (0, 0, 0)</param>
    /// <param name="rotation">Direction GameObject clone will be pointing - defaults to 0 degrees</param>
    /// <param name="parent">Parent GameObject - defaults to Pool</param>
    /// <param name="enable">Whether GameObject clone is enabled and visible - defaults to be true</param>
    /// <param name="poolOnDisable">Set to false if you want to be able to disable a clone without having it returned to the pool - defaults to true</param>
    /// <typeparam name="T">Type of component to pool. Must be in the pool to be used as a master to clone</typeparam>
    /// <returns>A cloned instance of T either reusing one returned to the pool or creating a new clone as needed</returns>
    /// <remarks><a href="http://customassets.marrington.net#acquire-gameobject-by-type">More...</a></remarks>
    public static T Acquire<T>(string     name,
                               Vector3    position      = default(Vector3),
                               Quaternion rotation      = default(Quaternion),
                               Transform  parent        = default(Transform),
                               bool       enable        = true,
                               bool       poolOnDisable = true)
      where T : Component {
      GameObject clone = Acquire(name, position, rotation, parent, enable, poolOnDisable);
      return (clone == null) ? null : clone.GetComponent<T>();
    }

    /// <summary>
    /// Use `Pool.Acquire()` to get a reference to a cloned gameObject by the name of the master gameObject held withing the Pool object.
    /// </summary>
    /// <param name="name">Name of cloned gameObject</param>
    /// <param name="position">Position for clone in game space - defaults to (0, 0, 0)</param>
    /// <param name="rotation">Direction GameObject clone will be pointing - defaults to 0 degrees</param>
    /// <param name="parent">Parent GameObject - defaults to Pool</param>
    /// <param name="enable">Whether GameObject clone is enabled and visible - defaults to be true</param>
    /// <param name="poolOnDisable">Set to false if you want to be able to disable a clone without having it returned to the pool - defaults to true</param>
    /// <returns>A cloned instance of T eitherr reusing one returned to the pool or creating a new clone as needed</returns>
    /// <remarks><a href="http://customassets.marrington.net#acquire-gameobject-by-name">More...</a></remarks>
    public static GameObject Acquire(string     name,
                                     Vector3    position      = default(Vector3),
                                     Quaternion rotation      = default(Quaternion),
                                     Transform  parent        = null,
                                     bool       enable        = true,
                                     bool       poolOnDisable = true) {
      PoolQueue pool = PoolFor(name);
      if (pool == null) return null;

      GameObject  clone;
      PoolMonitor poolMonitor;

      do {
        clone       = pool.Fetch();
        poolMonitor = clone.gameObject.GetComponent<PoolMonitor>();
      } while (!poolMonitor.OkToPool);

      clone.transform.SetParent(parent ? parent : poolMonitor.PoolRoot);
      clone.transform.position  = position;
      clone.transform.rotation  = rotation;
      poolMonitor.PoolOnDisable = poolOnDisable;
      clone.gameObject.SetActive(enable);
      poolMonitor.InPool   = false;
      poolMonitor.OkToPool = true;
      return clone;
    }

    // ReSharper disable once MemberCanBePrivate.Global
    /// <summary>
    /// Give an object created by `Acquire` in any pool, return it to the pool for
    /// reuse. Only needed if `poolOnDisable` is false
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#bool-poolondisable">More...</a></remarks>
    /// <param name="clone">Clone of a master GameObject in any pool within the application</param>
    public static void Return(GameObject clone) { Returns.Enqueue(clone); }

    private static readonly Queue<GameObject> Returns = new Queue<GameObject>();

    private IEnumerator SetParentOnReturn() {
      while (true) {
        while (Returns.Count > 0) {
          GameObject clone = Returns.Dequeue();
          if (clone == null) continue;

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

      // ReSharper disable once IteratorNeverReturns
    }

    /// <summary>
    /// Retrieve a reference to the pooling queue for the GameObject named.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#poolfor">More...</a></remarks>
    /// <param name="name">Name of GameObject under inspection</param>
    /// <returns>Reference to the `PoolQueue` or null if none exist for this name</returns>
    public static PoolQueue PoolFor(string name, Pool pool = null) {
      string poolName = string.Format("{0} Pool", name);
      if (Queues.ContainsKey(poolName)) return Queues[poolName];

      GameObject gameObject = Objects.FindGameObject(name);

      if (gameObject == null) {
        gameObject = Resources.Load<GameObject>(name);

        if (gameObject == null) {
          Debug.LogErrorFormat("Cannot find Prefab or loaded GameObject named '{0}'", name);
          return null;
        }

        gameObject = Instantiate(gameObject);
      }

      var monitor = gameObject.GetComponent<PoolMonitor>();
      if (monitor != null) return Queues[poolName] = Queues[monitor.MasterName];

      var queue = (pool ? pool : FindPool()).CreatePoolQueue(gameObject);
      // name may be a path to the prefab
      if (!Queues.ContainsKey(name)) Queues[name] = queue;
      return queue;
    }

    private static Pool FindPool() {
      Pool pool       = null;
      var  anyMonitor = FindObjectOfType<PoolMonitor>();

      if (anyMonitor != null) pool = anyMonitor.GetComponentInParent<Pool>();

      if (pool == null) {
        var pools = FindObjectsOfType<Pool>();

        for (int i = 0; (i < pools.Length) && (pool == null); i++) {
          if (pools[i].isPoolContainer) pool = pools[i];
        }
      }

      if (pool == null) pool = Components.Create<Pool>("Pool");
      return pool;
    }

    /// <summary>
    /// Create or retrieve the bool for a gameObject.
    /// </summary>
    /// <param name="gameObject">GameObject to add or inspect</param>
    /// <returns>PoolQueue that holds objects of this type</returns>
    [UsedImplicitly]
    public static PoolQueue PoolFor(GameObject gameObject) {
      var monitor = gameObject.GetComponent<PoolMonitor>();
      return (monitor != null) ? PoolFor(monitor.MasterName) : PoolFor(gameObject.name);
    }

    private sealed class PoolDict : Dictionary<string, PoolQueue> { }

    private static readonly PoolDict Queues = new PoolDict();

    private sealed class PoolMonitor : MonoBehaviour {
      public bool      PoolOnDisable;
      public bool      InPool, OkToPool = true;
      public string    MasterName;
      public Transform PoolRoot;

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

    /// <summary>
    /// Each GameObject enabled for pooling resides in a `PoolQueue`.
    /// </summary>
    public sealed class PoolQueue : Queue<GameObject> {
      /// <summary>
      /// Master GameObject from which copies are cloned
      /// </summary>
      public GameObject Master;

      private int count = 0;

      /// <summary>
      /// Fetch a cloned GameObject - either from a list of those returned or instantiating a new instance
      /// </summary>
      /// <returns>Reference to the cone of `Master` above</returns>
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