using Askowl;

namespace CustomAsset {
  using System;
  using System.Collections.Generic;
  using JetBrains.Annotations;

  /// <summary>
  /// Pick one item from a list.
  /// </summary>
  /// <typeparam name="T">Type of item. It can be a primative, object or even a Unity Asset</typeparam>
  // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
  public class Selector<T> : Pick<T> {
    private          T[]     choices = { };
    private readonly Func<T> picker;

    /// <summary>
    /// Constructor to create selection list.
    /// </summary>
    /// <param name="choices">The list to choose an item from</param>
    /// <param name="isRandom">Defaults to random. Set false to cycle through entries sequentially</param>
    /// <param name="exhaustiveBelow">If the list is shorter then select items randomly, but never choose one a second time until all have been picked. This is useful for short lists to reduce repeats.</param>
    public Selector(T[] choices = null, bool isRandom = true, int exhaustiveBelow = 0) {
      if (choices != null) this.choices = choices;
      choices = this.choices;

      if (!isRandom) { // cycle through list
        picker = () =>
          (choices.Length > 0) ? choices[cycleIndex++ % choices.Length] : default(T);
      } else if (choices.Length >= exhaustiveBelow) { // randoms election
        picker = () => choices[random.Next(minValue: 0, maxValue: choices.Length)];
      } else {
        picker = () => { // different random choice until list exhausted, then repeat
          if (remainingSelections.Count == 0) {
            remainingSelections = new List<T>(collection: choices);
          }

          cycleIndex = random.Next(minValue: 0, maxValue: remainingSelections.Count);
          T result = remainingSelections[index: cycleIndex];
          remainingSelections.RemoveAt(index: cycleIndex);
          return result;
        };
      }

      Init();
    }

    private readonly Random random = new Random();

    private void Init() {
      remainingSelections = new List<T>(collection: choices);
      cycleIndex          = 0;
    }

    /// <summary>
    /// Used to update the choices to a new set using the same picker.
    /// </summary>
    public T[] Choices {
      get { return choices; }
      [UsedImplicitly]
      set {
        choices = value;
        Init();
      }
    }

    private int cycleIndex;

    /// <summary>
    /// The location of the next choice in the sequence.
    /// </summary>
    [UsedImplicitly]
    public int CycleIndex { get { return cycleIndex % choices.Length; } }

    private List<T> remainingSelections;

    public virtual T Pick() { return picker(); }
  }
}