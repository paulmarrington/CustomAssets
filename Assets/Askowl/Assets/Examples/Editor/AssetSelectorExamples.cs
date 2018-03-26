using System.Linq;
using NUnit.Framework;
using UnityEngine;

public sealed class AssetSelectorExamples {
  private readonly AssetSelectorSample assetList =
    Resources.Load<AssetSelectorSample>(path: "AssetSelectorSample");

  [Test]
  public void AssetSelectorRandomExample() {
    Assert.IsNotNull(anObject: assetList, message: "Did not load resource 'AssetSelectorSample'");

    assetList.Select.Random();

    int[] hits = new int[assetList.Assets.Length];

    for (int idx = 0; idx < 100; idx++) {
      int at = assetList.ToPlay();
      Assert.AreNotEqual(expected: at, actual: -1, message: "Asset returned does not exist");
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
  public void AssetSelectorCycleExample() {
    Assert.IsNotNull(anObject: assetList, message: "Did not load resource 'AssetSelectorSample'");

    assetList.Select.Cycle();

    for (int idx = 0; idx < 100; idx++) {
      int at = assetList.ToPlay();
      Assert.AreNotEqual(expected: at, actual: -1, message: "Asset returned does not exist");
      Assert.Less(arg1: at, arg2: assetList.Assets.Length);
      Assert.AreEqual(expected: at, actual: idx % assetList.Assets.Length);
    }
  }

  [Test]
  public void AssetSelectorExhaustiveExample() {
    Assert.IsNotNull(anObject: assetList, message: "Did not load resource 'AssetSelectorSample'");

    assetList.Select.Exhaustive();

    int[] hits = new int[assetList.Assets.Length];

    for (int idx = 0; idx < (hits.Length * 100); idx++) {
      int at = assetList.ToPlay();
      Assert.AreNotEqual(expected: at, actual: -1, message: "Asset returned does not exist");
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