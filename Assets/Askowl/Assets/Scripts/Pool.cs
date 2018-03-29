namespace Askowl {
  using System;
  using System.Collections.Generic;
  using JetBrains.Annotations;
  using UnityEngine;
  using Object = UnityEngine.Object;

  public interface IPool {
    void Prepare(string name, Component master);
  }

  public sealed class PooledMonobehaviour : MonoBehaviour {
    [SerializeField] public Component Original;
  }

  public class Pool<T> : IPool where T : Component {
    public int Depth { get; private set; }

    // ReSharper disable once MemberCanBePrivate.Global
    public string Name { [UsedImplicitly] get; private set; }

    private readonly Queue<T> clones = new Queue<T>();
    private          T        original;

    private static Pool<T> instance;

    public static void CreatePool(T ofComponent) {
      if (instance != null) return;

      Type                poolType  = ofComponent.GetType();
      GameObject          singleton = new GameObject();
      PooledMonobehaviour component = singleton.AddComponent<PooledMonobehaviour>();

      component.Original = ofComponent;
      singleton.name     = component.name = "Pooled " + poolType.Name;

      Type genericType = typeof(Pool<>).MakeGenericType(poolType);
      instance = Activator.CreateInstance(type: genericType) as Pool<T>;

      if (instance != null) instance.Prepare(name: ofComponent.name, master: ofComponent);

      Object.DontDestroyOnLoad(target: singleton);
    }

    [NotNull]
    public static Pool<T> Instance { get { return instance ?? (instance = new Pool<T>()); } }

    public void Prepare([CanBeNull] string name, Component master) {
      Instance.Name     = string.IsNullOrEmpty(value: name) ? typeof(T).Name : name;
      Instance.original = master as T;
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
}