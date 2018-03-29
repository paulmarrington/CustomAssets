using UnityEngine;

namespace Askowl {
  using System;
  using System.Collections.Generic;
  using JetBrains.Annotations;
  using UnityEngine;
  using Object = UnityEngine.Object;

  public class Pool<T> where T : Component {
    public int    Depth { get; private set; }
    public string Name  { get; private set; }

    private Queue<T> clones = new Queue<T>();
    private T        original;

    private static Pool<T> instance;

    [NotNull]
    public static Pool<T> Instance { get { return instance ?? (instance = new Pool<T>()); } }

    protected static void Prepare([CanBeNull] string name = null, T master = null) {
      Instance.Name     = string.IsNullOrEmpty(value: name) ? typeof(T).Name : name;
      Instance.original = master;
    }

    [NotNull]
    public T Get(Vector3               position = default(Vector3),
                 Quaternion            rotation = default(Quaternion),
                 [CanBeNull] Transform parent   = default(Transform),
                 bool                  enable   = true) {
      T clone = (clones.Count == 0) ? CreateComponent() : clones.Dequeue();
      clone.gameObject.SetActive(enable);
      clone.transform.SetParent(parent);
      clone.transform.position = position;
      clone.transform.rotation = rotation;
      return clone;
    }

    public void Return([NotNull] T clone) {
      clone.transform.SetParent(default(Transform));
      clone.gameObject.SetActive(false);
      clones.Enqueue(clone);
    }

    [NotNull]
    private T CreateComponent() {
      if (original == null) {
        if ((original = Object.FindObjectOfType<T>()) == null) {
          original = Resources.Load<T>(path: typeof(T).Name);
        }
      }

      T clone = Object.Instantiate(original);
      clone.gameObject.name += " " + ++Depth;
      return clone;
    }
  }

public class PooledMonobehaviour : MonoBehaviour {
  [SerializeField] private Component original;

  private void Awake() {

    typeof(Pool<>).MakeGenericType(original.GetType());

    Type d1 = typeof(Pool<>);
    Type[] typeArgs = {original.GetType()};
  }
}
}
