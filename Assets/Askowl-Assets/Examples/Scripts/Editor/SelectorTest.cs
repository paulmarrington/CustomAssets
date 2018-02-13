using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class SelectorTest {

  Selector<int> selector = new Selector<int> (new int[] { 0, 1, 2, 3, 4 });

  [Test]
  public void SelectorTestRandom() {

    selector.Random(); // is also the default

    int[] hits = new int[selector.Choices.Length];
    for (int idx = 0; idx < 100; idx++) {
      int at = selector.Pick();
      Assert.AreNotEqual(at, -1, " returned does not exist");
      Assert.Less(at, hits.Length);
      hits [at]++;
    }
    string results = string.Join(", ", hits.ToList().Select(x => x.ToString()).ToArray());
    foreach (int hit in hits) {
      Assert.AreNotEqual(hit, 0, results);
    }
    Debug.Log("Random: " + results);
  }

  [Test]
  public void SelectorTestCycle() {

    selector.Cycle();

    for (int idx = 0; idx < 100; idx++) {
      int at = selector.Pick();
      Assert.AreEqual(at, idx % selector.Choices.Length);
    }
  }

  [Test]
  public void SelectorTestExhaustive() {
    
    selector.Exhaustive();

    int[] hits = new int[selector.Choices.Length];
    for (int idx = 0; idx < hits.Length * 100; idx++) {
      int at = selector.Pick();
      Assert.AreNotEqual(at, -1, " returned does not exist");
      Assert.Less(at, hits.Length);
      hits [at]++;
    }
    string results = string.Join(", ", hits.ToList().Select(x => x.ToString()).ToArray());
    int first = hits [0];
    foreach (int hit in hits) {
      Assert.AreEqual(hit, first, results);
    }
    Debug.Log("Exhaustive: " + results);
  }
}
