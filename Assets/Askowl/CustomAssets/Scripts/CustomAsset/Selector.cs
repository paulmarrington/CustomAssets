namespace CustomAsset {
  using Askowl;
  using UnityEngine;

  /// <summary>
  /// Custom Asset to store a list and allow code to pick one randomly with constraints.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class Selector<T> : ScriptableObject, Pick<T> {
    [SerializeField, Tooltip("List of items to choose from")]
    private T[] list;

    [SerializeField, Tooltip(
       "If set, items will be retrieved sequentially. Leave unckecked for a random selection")]
    private bool cycle;

    [SerializeField, Tooltip(
       "If the list is shorter then select items randomly, but never choose one a second time until all have been picked. This is useful for short lists to reduce repeats.")]
    private int exhaustiveBelow;

    private Askowl.Selector<T> selector;

    private void OnEnable() { selector = new Askowl.Selector<T>(list, !cycle, exhaustiveBelow); }

    /// <summary>
    /// Choose one item from the list based on the restraints set.
    /// </summary>
    /// <returns>One instance from the list</returns>
    public T Pick() { return selector.Pick(); }
  }
}