using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Examples/AssetSelector", fileName = "AssetSelectorExample", order = 1)]
public class AssetSelectorExample: AssetSelector<AudioClip> {

  public int ToPlay() {
    AudioClip pick = Pick();
    for (int idx = 0; idx < Assets.Length; idx++) {
      if (pick == Assets [idx]) {
        return idx;
      }
    }
    return -1;
  }
}