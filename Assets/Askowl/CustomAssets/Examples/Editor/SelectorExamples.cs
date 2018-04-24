using Askowl;
using UnityEngine;
using NUnit.Framework;
using System.Linq;

internal sealed class SelectorExamples {
  [Test]
  public void SelectorTestRandom() {
    Selector<int> selector = new Selector<int>(choices: new[] {0, 1, 2, 3, 4});

    int[] hits = new int[selector.Choices.Length];

    for (int idx = 0; idx < 100; idx++) {
      int at = selector.Pick();
      Assert.AreNotEqual(expected: at, actual: -1, message: " returned does not exist");
      Assert.Less(arg1: at, arg2: hits.Length);
      hits[at]++;
    }

    string results = string.Join(separator: ", ",
                                 value: hits.ToList().Select(selector: x => x.ToString())
                                            .ToArray());

    foreach (int hit in hits) {
      Assert.AreNotEqual(expected: hit, actual: 0, message: results);
    }

    Debug.Log(message: "Random: " + results);
  }

  [Test]
  public void SelectorTestCycle() {
    Selector<int> selector = new Selector<int>(choices: new[] {0, 1, 2, 3, 4}, isRandom: false);

    for (int idx = 0; idx < 100; idx++) {
      int at = selector.Pick();
      Assert.AreEqual(expected: at, actual: idx % selector.Choices.Length);
    }
  }

  [Test]
  public void SelectorTestExhaustive() {
    Selector<int> selector = new Selector<int>(choices: new[] {0, 1, 2, 3, 4}, exhaustiveBelow: 10);

    int[] hits = new int[selector.Choices.Length];

    for (int idx = 0; idx < (hits.Length * 100); idx++) {
      int at = selector.Pick();
      Assert.AreNotEqual(expected: at, actual: -1, message: " returned does not exist");
      Assert.Less(arg1: at, arg2: hits.Length);
      hits[at]++;
    }

    string results = string.Join(separator: ", ",
                                 value: hits.ToList().Select(selector: x => x.ToString())
                                            .ToArray());

    int first = hits[0];

    foreach (int hit in hits) {
      Assert.AreEqual(expected: hit, actual: first, message: results);
    }

    Debug.Log(message: "Exhaustive: " + results);
  }
}