namespace Askowl {
  using System.Collections.Generic;
  using JetBrains.Annotations;
  using UnityEngine;

  public class Pool<T> : Singleton<Pool<T>> where T : Component {
    [SerializeField] private T original;

    private readonly Queue<T> clones = new Queue<T>();

    public T Get([CanBeNull] Transform parent = default(Transform),
                 bool                  enable = true) {
      if (clones.Count == 0) DeepenPool();
      T clone = clones.Dequeue();
      clone.gameObject.SetActive(enable);
      clone.transform.SetParent(parent);
      return clone;
    }

    public T Get(Vector3               position = default(Vector3),
                 Quaternion            rotation = default(Quaternion),
                 [CanBeNull] Transform parent   = default(Transform),
                 bool                  enable   = true) {
      T clone = Get(parent, enable);
      clone.transform.position = position;
      clone.transform.rotation = rotation;
      return clone;
    }

    public void Return(T clone) {
      clone.transform.SetParent(transform);
      clone.gameObject.SetActive(false);
      clones.Enqueue(clone);
    }

    private void DeepenPool() {
      for (int i = 0; i < 10; i++) {
        T clone = Instantiate<T>(original);
        clone.gameObject.name += " " + i;
        Return(clone);
      }
    }
  }
}