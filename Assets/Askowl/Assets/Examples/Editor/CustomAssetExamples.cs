using NUnit.Framework;

public sealed class CustomAssetExamples {
  [Test]
  public void UnityCustomAssetExample() {
    // Used Unity Asset menu to create a custom asset named TestCustomAsset
    CustomAssetSample customAsset = CustomAssetSample.Asset();
    Assert.AreEqual(expected: 1234, actual: customAsset.AnInteger);
  }

  [Test]
  public void UnityNamedCustomAssetExample() {
    // If we give the asset a name -- that is the asset looked for in Resources
    CustomAssetSample customAsset = CustomAssetSample.Asset(name: "SecondCustomAssetSample");
    Assert.AreEqual(expected: 5678, actual: customAsset.AnInteger);
  }

  [Test]
  public void UnityDefaultCustomAssetExample() {
    // If an asset doesn't exist we can load a default instead
    CustomAssetSample customAsset = CustomAssetSample.Asset(name: "ThirdCustomAssetSample");
    Assert.AreEqual(expected: 9000, actual: customAsset.AnInteger);
  }
}