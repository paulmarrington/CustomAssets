/*
 * With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017
 */

using System;
using System.Collections.Generic;
using Askowl;
using UnityEngine;

namespace CustomAsset {
  using JetBrains.Annotations;

  /// <inheritdoc cref="Pick" />
  /// <summary>
  /// Set of any serialised type as a custom asset.
  /// </summary>
  public class Set<T> : OfType<List<T>>, Pick<T> {
    [SerializeField, Tooltip("For Pick: true for sequential, false for random")]
    private bool cycle;

    [SerializeField, Tooltip(
       "For Pick: If the list is shorter then select items randomly, but never choose one a second time until all have been picked. This is useful for short lists to reduce repeats.")]
    private int exhaustiveBelow;

    /// <summary>
    /// Add an entry if one does not exist already - and trigger a change event.
    /// </summary>
    [UsedImplicitly]
    public void Add(T entry) {
      if (Value.Contains(entry)) return;

      Value.Add(entry);
      selector = null;
      Changed();
    }

    /// <summary>
    /// Remove an entry if it exists - and trigger a change event.
    /// </summary>
    [UsedImplicitly]
    public void Remove(T entry) {
      if (!Value.Contains(entry)) return;

      Value.Remove(entry);
      selector = null;
      Changed();
    }

    /// <summary>
    /// Call an action on every entry in the set. Order is from last to first so that items can be removed safely.
    /// </summary>
    /// <param name="action">Action called with one entry from the set</param>
    [UsedImplicitly]
    public void ForAll(Action<T> action) {
      // Loop backwards since the list may change when disabling
      for (int i = Value.Count - 1; i >= 0; i--) action(Value[i]);
    }

    private Selector<T> selector;

    public T Pick() {
      if (selector == null) {
        selector = new Selector<T>(Value.ToArray(), !cycle, exhaustiveBelow);
      }

      return selector.Pick();
    }
  }
}