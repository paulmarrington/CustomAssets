namespace Askowl {
  using System;
  using System.Collections.Generic;
  using JetBrains.Annotations;

  public sealed class Selector<T> : Pick<T> {
    private T[]     choices = { };
    private Func<T> picker;

    internal Selector() {
      Random();
      Init();
    }

    public Selector(T[] choices) {
      this.choices = choices;
      Random();
      Init();
    }

    private readonly Random random = new Random();

    private void Init() {
      remaining = new List<T>(collection: choices);
      idx       = 0;
    }

    public T[] Choices {
      get { return choices; }
      set {
        choices = value;
        Init();
      }
    }

    public void Random() {
      picker = () => choices[random.Next(minValue: 0, maxValue: choices.Length)];
      Init();
    }

    private int idx;

    public void Cycle() { picker = () => choices[idx++ % choices.Length]; }

    [UsedImplicitly]
    public int CycleIndex { get { return idx % choices.Length; } }

    private List<T> remaining;

    public void Exhaustive() {
      picker = () => {
        if (remaining.Count == 0) {
          remaining = new List<T>(collection: choices);
        }

        idx = random.Next(minValue: 0, maxValue: remaining.Count);
        T result = remaining[index: idx];
        remaining.RemoveAt(index: idx);
        return result;
      };
    }

    public T Pick() { return picker(); }
  }
}