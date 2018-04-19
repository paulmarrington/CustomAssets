namespace CustomAsset {
  using Askowl;
  using UnityEngine;

  public class SelectAsset<T> : ScriptableObject, Pick<T> {
    [SerializeField] private Selector<T> list;

    public T Pick() { return list.Pick(); }
  }
}