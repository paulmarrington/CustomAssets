/*
 * With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017
 */

using System;
using System.Collections.Generic;
using Askowl;
using UnityEngine;

namespace CustomAsset.Support {
  /// <inheritdoc cref="Pick" />
  /// <summary>
  /// Set of any serialised type as a custom asset.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#custom-asset-sets">More...</a></remarks>
  [Serializable]
  public class Set<T> : Pick<T> {
    [SerializeField] private List<T> elements;

    [SerializeField, Tooltip("true for sequential, false for random")]
    internal bool Cycle;

    [SerializeField, Tooltip(
       "If the list is shorter then select items randomly, but never choose one a second time until all have been picked. This is useful for short lists to reduce unexpected repeats.")]
    internal int ExhaustiveBelow;

    /// <summary>
    /// Elements that make up the set
    /// </summary>
    public List<T> Elements { get { return elements; } set { elements = value; } }

    /// <summary>See if a set contains a specific element.</summary>
    /// <remarks><a href="http://customassets.marrington.net#containsentry">More...</a></remarks>
    /// <param name="entry">Element that may or may not be in the set</param>
    /// <returns>True if the element supplied is in this set</returns>
    public bool Contains(T entry) { return Elements.Contains(entry); }

    /// <summary>Return the number of entries in the Set</summary>
    /// <remarks><a href="http://customassets.marrington.net#count">More...</a></remarks>
    public int Count { get { return Elements.Count; } }

    /// <summary>Call an action on every entry in the set. Order is from last to first so that items can be removed safely.</summary>
    /// <param name="action">Action called with one entry from the set</param>
    /// <remarks><a href="http://customasset.marrington.net#forall">More...</a></remarks>
    public void ForEach(Func<T, bool> action) {
      // Loop backwards since the list may change when disabling
      for (int i = Elements.Count - 1; i >= 0; i--) {
        if (!action(Elements[i])) break;
      }
    }

    /// <summary>
    /// If the contents have changed we will need to rebuild the selector. This is normally
    /// at the next Pick() call. It can be overridden by more complex Set types.
    /// </summary>
    protected virtual void BuildSelector() {
      Selector = new Selector<T>(Elements.ToArray(), !Cycle, ExhaustiveBelow);
    }

    /// <inheritdoc />
    public T Pick() {
      if (Selector == null) BuildSelector();

      return Selector.Pick();
    }

    /// <summary>
    /// If something has changed the underlying data we need to tell the Selector
    /// that it is now out of date.
    /// </summary>
    public void Reset() { Selector = null; }

    /// <summary>
    /// Delector used to pick an element from the set
    /// </summary>
    protected Selector<T> Selector;
  }
}

namespace CustomAsset.Constant {
  /// <inheritdoc cref="Pick" />
  /// <summary>
  /// Set of any serialised type as a custom asset.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#custom-asset-sets">More...</a></remarks>
  public class Set<T> : OfType<Support.Set<T>>, Pick<T> {
    /// <inheritdoc />
    public T Pick() { return Value.Pick(); }
  }
}