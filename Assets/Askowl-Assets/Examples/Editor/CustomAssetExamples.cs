using NUnit.Framework;

public class CustomAssetExamples {

  [Test]
  public void UnityCustomAssetExample() {
    // Used Unity Asset menu to create a custom asset named TestCustomAsset
    CustomAssetSample customAsset = CustomAssetSample.Asset();
    Assert.AreEqual(1234, customAsset.anInteger);
  }

  [Test]
  public void UnityNamedCustomAssetExample() {
    // If we give the asset a name -- that is the asset looked for in Resources
    CustomAssetSample customAsset = CustomAssetSample.Asset(name: "SecondCustomAssetSample");
    Assert.AreEqual(5678, customAsset.anInteger);
  }

  [Test]
  public void UnityDefaultCustomAssetExample() {
    // If an asset doesn't exist we can load a default instead
    CustomAssetSample customAsset = CustomAssetSample.Asset(name: "ThirdCustomAssetSample");
    Assert.AreEqual(9000, customAsset.anInteger);
  }
}
