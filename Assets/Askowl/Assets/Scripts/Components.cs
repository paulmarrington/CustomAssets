namespace Askowl {
  using System.Linq;
  using JetBrains.Annotations;
  using UnityEngine;

  public static class Components {
    public static T Find<T>(string name = null) where T : Object {
      name = string.IsNullOrEmpty(value: name) ? typeof(T).Name : name;

      T[] objects = Object.FindObjectsOfType<T>();

      foreach (T obj in objects.Where(obj => obj.name.Equals(name))) return obj;

      T resource = Resources.Load<T>(path: name);
      return resource;
    }

    [NotNull]
    public static T Create<T>([CanBeNull] string name = null) where T : Component {
      GameObject gameObject = new GameObject();

      T instance = Create<T>(gameObject, name);
      instance.name   = name ?? typeof(T).ToString();
      gameObject.name = instance.name;
      return instance;
    }

    [NotNull]
    public static T Create<T>([NotNull] GameObject gameObject, [CanBeNull] string name = null)
      where T : Component {
      T instance = gameObject.AddComponent<T>();
      instance.name = name ?? typeof(T).ToString();
      return instance;
    }
  }
}