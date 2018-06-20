/*
 * With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017
 */

using Askowl;

namespace CustomAsset.Mutable {
  /// <inheritdoc cref="Pick" />
  /// <summary>
  /// Set of any serialised type as a custom asset.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#custom-asset-sets">More...</a></remarks>
  public class Set<T> : OfType<Support.Set<T>>, Pick<T> {
    /// <summary>Add an entry if one does not exist already - and trigger a change event.</summary>
    /// <remarks><a href="http://customassets.marrington.net#addentry">More...</a></remarks>
    /// <param name="entry">Element to add if it isn't in the list</param>
    // ReSharper disable once UnusedMember.Global
    public void Add(T entry) {
      if (Value.Contains(entry)) return;

      Value.Elements.Add(entry);
      Changed();
    }

    /// <summary>Remove an entry if it exists - and trigger a change event.</summary>
    /// <remarks><a href="http://customassets.marrington.net#removeentry">More...</a></remarks>
    /// <param name="entry">Element to remove if it is in the list</param>
    // ReSharper disable once UnusedMember.Global
    public void Remove(T entry) {
      if (!Value.Contains(entry)) return;

      Value.Elements.Remove(entry);
      Changed();
    }

    private void Changed() {
      Value.Reset();
      Emitter.Fire();
    }

    /// <inheritdoc />
    public T Pick() { return Value.Pick(); }
  }
}