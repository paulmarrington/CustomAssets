namespace Askowl {
  using System.Collections.Generic;
  using JetBrains.Annotations;
  using UnityEngine;

  public class Pool<T> where T : Component {
    public Pool<T> Instance { get { } }

    public int Depth { get; private set; }

    private readonly Pool
    private readonly Queue<T> clones = new Queue<T>();

    public T Get(Vector3               position = default(Vector3),
                 Quaternion            rotation = default(Quaternion),
                 [CanBeNull] Transform parent   = default(Transform),
                 bool                  enable   = true) {
      if (clones.Count == 0) DeepenPool();
      T clone = clones.Dequeue();
      clone.gameObject.SetActive(enable);
      clone.transform.SetParent(parent);
      clone.transform.position = position;
      clone.transform.rotation = rotation;
      return clone;
    }

    public void Return(T clone) {
      clone.transform.SetParent(default(Transform));
      clone.gameObject.SetActive(false);
      clones.Enqueue(clone);
    }

    private void DeepenPool() {
      T clone = Object.Instantiate<T>(default(T));
      clone.gameObject.name += " " + ++Depth;
      Return(clone);
    }
  }
}