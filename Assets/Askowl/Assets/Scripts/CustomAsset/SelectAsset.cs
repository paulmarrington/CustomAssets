namespace CustomAsset {
  using Askowl;
  using UnityEngine;

  public class SelectAsset<T> : ScriptableObject, IPick<T> {
    [SerializeField] private Selector<T> list;

    public T Pick() { return list.Pick(); }
  }
}