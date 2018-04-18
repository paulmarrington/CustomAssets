namespace CustomAsset {
  using Askowl;
  using UnityEngine;

  public class Select<T> : ScriptableObject, IPick<T> {
    [SerializeField] private Selector<T> list;

    public T Pick() { return list.Pick(); }
  }
}