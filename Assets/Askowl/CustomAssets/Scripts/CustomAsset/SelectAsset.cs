namespace CustomAsset {
  using Askowl;
  using UnityEngine;

  public class SelectAsset<T> : ScriptableObject, Pick<T> {
    [SerializeField] private T[]  list;
    [SerializeField] private bool cycle;
    [SerializeField] private bool exhaustive;

    private Selector<T> selector;

    private void OnEnable() { selector = new Selector<T>(list, exhaustive, !cycle); }

    public T Pick() { return selector.Pick(); }
  }
}