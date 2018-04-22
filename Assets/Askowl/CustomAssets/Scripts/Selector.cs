namespace Askowl {
  using System;
  using System.Collections.Generic;
  using JetBrains.Annotations;

  public sealed class Selector<T> : Pick<T> {
    private          T[]     choices = { };
    private readonly Func<T> picker;

    public Selector(T[] choices = null, bool isExhaustive = false, bool isRandom = true) {
      if (choices != null) this.choices = choices;

      if (!isRandom) { // cycle through list
        picker = () => this.choices[cycleIndex++ % this.choices.Length];
      } else if (!isExhaustive) { // randoms election
        picker = () => this.choices[random.Next(minValue: 0, maxValue: this.choices.Length)];
      } else {
        picker = () => { // different random choice until list exhausted, then repeat
          if (remainingSelections.Count == 0) {
            remainingSelections = new List<T>(collection: this.choices);
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

    public T[] Choices {
      get { return choices; }
      [UsedImplicitly]
      set {
        choices = value;
        Init();
      }
    }

    private int cycleIndex;

    [UsedImplicitly]
    public int CycleIndex { get { return cycleIndex % choices.Length; } }

    private List<T> remainingSelections;

    public T Pick() { return picker(); }
  }
}