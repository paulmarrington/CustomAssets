// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace CustomAsset {
  public abstract partial class OfType<T> {
    [SerializeField, Header("Value")] private T            seed;
    [SerializeField]                  private List<string> members;

    private T                     seedSaver;
    private Dictionary<string, T> dictionary;

    /// <summary>
    /// Accessor for dictionary entries that will raise changed event on entry changed
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#members">More...</a></remarks>
    /// <param name="memberName">Generic dictionary key</param>
    [UsedImplicitly]
    public T this[string memberName] {
      get { return Contains(memberName) ? dictionary[memberName] : seedSaver; }
      set {
        if (!Contains(memberName)) members.Add(memberName);
        dictionary[memberName] = value;
        Changed(memberName);
      }
    }

    /// <inheritdoc />
    /// <remarks><a href="http://customassets.marrington.net#members">More...</a></remarks>
    [UsedImplicitly]
    public override string ToStringForMember(string memberName) {
      return this[memberName].ToString();
    }

    /// <summary>See if a set contains a specific element.</summary>
    /// <remarks><a href="http://customassets.marrington.net#members">More...</a></remarks>
    /// <param name="memberName">Key to value that may or may not be in the set</param>
    /// <returns>True if the element supplied is in this set</returns>
    [UsedImplicitly]
    public bool Contains(string memberName) { return dictionary.ContainsKey(memberName); }

    /// <summary>Remove an entry if it exists - and trigger a change event.</summary>
    /// <remarks><a href="http://customassets.marrington.net#members">More...</a></remarks>
    /// <param name="memberName">Member to remove if it is in the list</param>
    [UsedImplicitly]
    internal void Remove(string memberName) {
      if (!Contains(memberName)) return;

      dictionary.Remove(memberName);
      members.Remove(memberName);
      Changed(memberName);
    }

    /// <summary>
    /// Clear members list for a clean slate
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#members">More...</a></remarks>
    [UsedImplicitly]
    internal void Clear() {
      dictionary.Clear();
      members.Clear();
      Changed();
    }

    /// <summary>
    /// Used to get all member names
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#members">More...</a></remarks>
    /// <returns>withMembers</returns>
    [UsedImplicitly]
    public string[] MemberNames { get { return members.ToArray(); } }

    [Serializable, UsedImplicitly]
    // ReSharper disable MissingXmlDoc
    public class ToPersist {
      public T        CurrentValue;
      public string[] Members;
      public T[]      Values;
    }
    // ReSharper restore MissingXmlDoc

    private readonly ToPersist toPersist = new ToPersist();

    /// <summary>
    /// Load the last previously saved value from persistent storage. Called
    /// implicitly when persistent flag is set and custom asset is enabled.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#custom-asset-persistence">More...</a></remarks>
    internal void Load() {
      dictionary = new Dictionary<string, T>();
      seedSaver  = seed;
      ToPersist data = Loader<ToPersist>();

      if (members == null) members = new List<string>();

      if (data != default(ToPersist)) {
        seed = data.CurrentValue;

        if (data.Members != null) {
          members = data.Members.ToList();
          for (int i = 0; i < members.Count; i++) dictionary[members[i]] = data.Values[i];
        }
      } else {
        for (int i = 0; i < members.Count; i++) dictionary[members[i]] = seed;
      }
    }

    /// <summary>
    /// Save current value to persistent storage. Called emplicitly when  when persistent flag is set
    /// and custom asset is disabled or on every change if it is marked critical.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#custom-asset-persistence">More...</a></remarks>
    [UsedImplicitly]
    internal void Save() {
      toPersist.CurrentValue = seed;
      toPersist.Members      = MemberNames;
      toPersist.Values       = new T[members.Count];

      for (int i = 0; i < members.Count; i++) toPersist.Values[i] = dictionary[members[i]];

      Saver(toPersist);
    }
  }
}