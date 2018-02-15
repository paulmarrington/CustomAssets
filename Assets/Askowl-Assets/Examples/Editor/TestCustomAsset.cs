using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

[CreateAssetMenu(menuName = "Examples/Custom Asset", fileName = "CustomAssetExample", order = 1)]
public class TestCustomAsset: CustomAsset<TestCustomAsset> {
  public int anInteger;
}

public class CustomAssetTest {

  [Test]
  public void UnityCustomAssetTest() {
    // Used Unity Asset menu to create a custom asset named TestCustomAsset
    TestCustomAsset customAsset = TestCustomAsset.Asset();
    Assert.AreEqual(1234, customAsset.anInteger);
  }

  [Test]
  public void UnityNamedCustomAssetTest() {
    // If we give the asset a name -- that is the asset looked for in Resources
    TestCustomAsset customAsset = TestCustomAsset.Asset(name: "SecondTestCustomAsset");
    Assert.AreEqual(5678, customAsset.anInteger);
  }

  [Test]
  public void UnityDefaultCustomAssetTest() {
    // If an asset doesn't exist we can load a default instead
    TestCustomAsset customAsset = TestCustomAsset.Asset(name: "ThirdTestCustomAsset");
    Assert.AreEqual(9000, customAsset.anInteger);
  }
}
