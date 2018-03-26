using Askowl;
using UnityEngine;

[CreateAssetMenu(menuName = "Examples/AssetSelector", fileName = "AssetSelectorSample", order = 1)]
public sealed class AssetSelectorSample : AssetSelector<AudioClip> {
  public int ToPlay() {
    AudioClip pick = Pick();

    for (int idx = 0; idx < Assets.Length; idx++) {
      if (pick == Assets[idx]) {
        return idx;
      }
    }

    return -1;
  }
}