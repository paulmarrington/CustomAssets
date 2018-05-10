/*
 * With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomAsset {
  using JetBrains.Annotations;

  /// <inheritdoc />
  /// <summary>
  /// CustomAsset Dictionary with generic keys and values
  /// </summary>
  /// <remarks><a href="http://customasset.marrington.net#custom-asset-dictionaries">More...</a></remarks>
  public class Dict<TK, TV> : OfType<KVP<TK, TV>[]> {
    [SerializeField] private TK          tk;
    [SerializeField] private KVP<TK, TV> kvp;

    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// List of keys for the dictionary
    /// </summary>
    protected List<TK> keys;

    private readonly Dictionary<TK, TV> dictionary = new Dictionary<TK, TV>();

    /// <inheritdoc />
    protected override void OnEnable() {
      base.OnEnable();
      if (keys != null) return; // loaded persistent data

      for (int i = 0; i < Value.Length; i++) dictionary[Value[i].Key] = Value[i].Value;
      keys = dictionary.Keys.ToList();
    }

    /// <summary>
    /// Accessor for dictionary entries that will raise changed event on entry changed
    /// </summary>
    /// <param name="key">Generic dictionary key</param>
    [UsedImplicitly]
    public TV this[TK key] {
      get { return Contains(key) ? dictionary[key] : default(TV); }
      set {
        if (!Contains(key)) keys.Add(key);
        dictionary[key] = value;
        Changed();
      }
    }

    /// <summary>Remove an entry if it exists - and trigger a change event</summary>
    /// <remarks><a href="http://customasset.marrington.net#remove-dictionary-entry">More...</a></remarks>
    /// <param name="key">key to value to remove if it is in the list</param>
    [UsedImplicitly]
    public virtual void Remove(TK key) {
      if (!Contains(key)) return;

      dictionary.Remove(key);
      keys.Remove(key);
      Changed();
    }

    /// <summary>Empty the dictionary</summary>
    [UsedImplicitly]
    public virtual void Clear() {
      dictionary.Clear();
      keys.Clear();
      Changed();
    }

    /// <summary>See if a set contains a specific element.</summary>
    /// <remarks><a href="http://customasset.marrington.net#dictionary-contains-entry">More...</a></remarks>
    /// <param name="key">Key to value that may or may not be in the set</param>
    /// <returns>True if the element supplied is in this set</returns>
    [UsedImplicitly]
    public bool Contains(TK key) { return dictionary.ContainsKey(key); }

    /// <summary>Return the number of entries in the dictionary</summary>
    /// <remarks><a href="http://customasset.marrington.net#dictionary-count">More...</a></remarks>
    [UsedImplicitly]
    public int Count { get { return keys.Count; } }

    /// <summary>
    /// Reference to an array that holds all the keys to the dictionary
    /// </summary>
    /// <remarks><a href="http://customasset.marrington.net#dictionary-keys">More...</a></remarks>
    [UsedImplicitly]
    public TK[] Keys { get { return keys.ToArray(); } }

    /// <summary>Call an action on every entry in the dictionary.</summary>
    /// <param name="action">Action called with one entry from the dictionary as (key, value). Return true to continue, false to abort.</param>
    /// <remarks><a href="http://customasset.marrington.net#forall-dictionary-entries">More...</a></remarks>
    [UsedImplicitly]
    public void ForEach(Func<TK, TV, bool> action) {
      for (int i = keys.Count - 1; i >= 0; i--) {
        if (!action(keys[i], dictionary[keys[i]])) break;
      }
    }

    [Serializable]
    // ReSharper disable once InconsistentNaming
    private class KVPLists {
      public TK[] Keys;
      public TV[] Values;
    }

    /// <inheritdoc />
    public override void Load() {
      KVPLists kvpLists = Loader<KVPLists>();

      if (kvpLists != null) {
        keys = kvpLists.Keys.ToList();
        for (int i = 0; i < keys.Count; i++) dictionary[kvpLists.Keys[i]] = kvpLists.Values[i];
      } else {
        keys = new List<TK>();
      }
    }

    /// <inheritdoc />
    public override void Save() {
      KVPLists kvpLists = new KVPLists() {Keys = keys.ToArray()};
      kvpLists.Values = new TV[keys.Count];
      for (int i = 0; i < keys.Count; i++) kvpLists.Values[i] = dictionary[keys[i]];
      Saver(kvpLists);
    }
  }
}