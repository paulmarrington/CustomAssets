using UnityEngine;
using NUnit.Framework;
using System.Linq;

public class AssetSelectorExamples {

  AssetSelectorSample assetList = Resources.Load<AssetSelectorSample>("AssetSelectorSample");

  [Test]
  public void AssetSelectorRandomExample() {
    Assert.IsNotNull(assetList, "Did not load resource 'AssetSelectorSample'");

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
  public void AssetSelectorCycleExample() {
    Assert.IsNotNull(assetList, "Did not load resource 'AssetSelectorSample'");

    assetList.Select.Cycle();

    for (int idx = 0; idx < 100; idx++) {
      int at = assetList.ToPlay();
      Assert.AreNotEqual(at, -1, "Asset returned does not exist");
      Assert.Less(at, assetList.Assets.Length);
      Assert.AreEqual(at, idx % assetList.Assets.Length);
    }
  }

  [Test]
  public void AssetSelectorExhaustiveExample() {
    Assert.IsNotNull(assetList, "Did not load resource 'AssetSelectorSample'");

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
