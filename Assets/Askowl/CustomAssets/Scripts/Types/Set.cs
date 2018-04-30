﻿/*
 * With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017
 */

using System;
using System.Collections.Generic;
using CustomAsset;
using UnityEngine;

namespace CustomAsset {
  using JetBrains.Annotations;

  /// <inheritdoc cref="Pick" />
  /// <summary>
  /// Set of any serialised type as a custom asset.
  /// </summary>
  /// <remarks><a href="http://customasset.marrington.net#custom-asset-sets">More...</a></remarks>
  public class Set<T> : OfType<List<T>>, Pick<T> {
    [Header("Set Picker"), SerializeField,
     Tooltip("true for sequential, false for random")]
    private bool cycle;

    [SerializeField, Tooltip(
       "If the list is shorter then select items randomly, but never choose one a second time until all have been picked. This is useful for short lists to reduce unexpected repeats.")]
    private int exhaustiveBelow;

    /// <summary>Add an entry if one does not exist already - and trigger a change event.</summary>
    /// <remarks><a href="http://customasset.marrington.net#addentry">More...</a></remarks>
    /// <param name="entry">Element to add if it isn't in the list</param>
    [UsedImplicitly]
    public void Add(T entry) {
      if (Contains(entry)) return;

      Value.Add(entry);
      selector = null;
      Changed();
    }

    /// <summary>Remove an entry if it exists - and trigger a change event.</summary>
    /// <remarks><a href="http://customasset.marrington.net#removeentry">More...</a></remarks>
    /// <param name="entry">Element to remove if it is in the list</param>
    [UsedImplicitly]
    public void Remove(T entry) {
      if (!Contains(entry)) return;

      Value.Remove(entry);
      selector = null;
      Changed();
    }

    /// <summary>See if a set contains a specific element.</summary>
    /// <remarks><a href="http://customasset.marrington.net#containsentry">More...</a></remarks>
    /// <param name="entry">Element that may or may not be in the set</param>
    /// <returns>True if the element supplied is in this set</returns>
    [UsedImplicitly]
    public bool Contains(T entry) { return Value.Contains(entry); }

    /// <summary>Return the number of entries in the Set</summary>
    /// <remarks><a href="http://customasset.marrington.net#count">More...</a></remarks>
    [UsedImplicitly]
    public int Count { get { return Value.Count; } }

    /// <summary>Call an action on every entry in the set. Order is from last to first so that items can be removed safely.</summary>
    /// <param name="action">Action called with one entry from the set</param>
    /// <remarks><a href="http://customasset.marrington.net#forall">More...</a></remarks>
    [UsedImplicitly]
    public void ForAll(Action<T> action) {
      // Loop backwards since the list may change when disabling
      for (int i = Value.Count - 1; i >= 0; i--) action(Value[i]);
    }

    private Selector<T> selector;

    /// <summary>
    /// Pick one element from the set based on parameters <see cref="cycle"/> and <see cref="exhaustiveBelow"/>
    /// </summary>
    /// <remarks><a href="http://customasset.marrington.net#pick">More...</a></remarks>
    /// <returns>Element chosen</returns>
    public T Pick() {
      if (selector == null) {
        selector = new Selector<T>(Value.ToArray(), !cycle, exhaustiveBelow);
      }

      return selector.Pick();
    }
  }
}