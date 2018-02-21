using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Examples/Custom Asset", fileName = "CustomAssetSample", order = 1)]
public class CustomAssetSample: CustomAsset<CustomAssetSample> {
  public int anInteger;
}
