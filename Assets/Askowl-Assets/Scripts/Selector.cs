using System.Collections;
using System.Collections.Generic;
using System;

public class Selector<T>: Pick<T> {
  T[] choices = { };
  Func<T> picker;

  public Selector() {
    Random();
    init();
  }

  public Selector(T[] choices) {
    this.choices = choices;
    Random();
    init();
  }

  private Random random = new Random ();

  void init() {
    remaining = new List<T> (choices);
    idx = 0;
  }

  public T[] Choices {
    get { return choices; }
    set {
      choices = value;
      init();
    }
  }

  public void Random() {
    picker = () => choices [random.Next(0, choices.Length)];
    init();
  }

  int idx;

  public void Cycle() {
    picker = () => choices [idx++ % choices.Length];
  }

  public int CycleIndex { get { return idx % choices.Length; } }

  List<T> remaining;

  public void Exhaustive() {
    picker = () => {
      if (remaining.Count == 0) {
        remaining = new List<T> (choices);
      }
      idx = random.Next(0, remaining.Count);
      T result = remaining [idx];
      remaining.RemoveAt(idx);
      return result;
    };
  }

  public virtual T Pick() {
    return picker();
  }
}