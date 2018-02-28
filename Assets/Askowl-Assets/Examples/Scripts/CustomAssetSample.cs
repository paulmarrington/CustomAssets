using UnityEngine;

[CreateAssetMenu(menuName = "Examples/Custom Asset", fileName = "CustomAssetSample", order = 1)]
public sealed class CustomAssetSample : CustomAsset<CustomAssetSample> {
  public int AnInteger;
}