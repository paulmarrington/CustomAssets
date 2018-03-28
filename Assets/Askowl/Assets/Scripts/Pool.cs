namespace Askowl {
  using System.Collections.Generic;
  using UnityEngine;

  public class Pool<T> : MonoBehaviour where T : Component {
    [SerializeField] private T original;

    public static Pool<T> Instance { get; private set; }

    private static readonly Queue<T> clones = new Queue<T>();

    private void Awake() { Instance = this; }

    public T Get() {
      if (clones.Count == 0) DeepenPool();
      T clone = clones.Dequeue();
      clone.gameObject.SetActive(true);
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