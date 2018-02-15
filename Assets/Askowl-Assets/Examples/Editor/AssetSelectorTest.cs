using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class AssetSelectorTest {

  AssetSelectorExample assetList = Resources.Load<AssetSelectorExample>("AssetSelectorExample");

  [Test]
  public void AssetSelectorTestRandom() {
    Assert.IsNotNull(assetList, "Did not load resource 'AssetSelectorExample'");

    assetList.Select.Random();

    int[] hits = new int[assetList.Assets.Length];
    for (int idx = 0; idx < 100; idx++) {
      int at = assetList.ToPlay();
      Assert.AreNotEqual(at, -1, "Asset returned does not exist");
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
  public void AssetSelectorTestCycle() {
    Assert.IsNotNull(assetList, "Did not load resource 'AssetSelectorExample'");

    assetList.Select.Cycle();

    for (int idx = 0; idx < 100; idx++) {
      int at = assetList.ToPlay();
      Assert.AreNotEqual(at, -1, "Asset returned does not exist");
      Assert.Less(at, assetList.Assets.Length);
      Assert.AreEqual(at, idx % assetList.Assets.Length);
    }
  }

  [Test]
  public void AssetSelectorTestExhaustive() {
    Assert.IsNotNull(assetList, "Did not load resource 'AssetSelectorExample'");

    assetList.Select.Exhaustive();

    int[] hits = new int[assetList.Assets.Length];
    for (int idx = 0; idx < hits.Length * 100; idx++) {
      int at = assetList.ToPlay();
      Assert.AreNotEqual(at, -1, "Asset returned does not exist");
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
