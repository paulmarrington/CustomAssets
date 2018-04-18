using CustomAsset;
using UnityEngine;

public sealed class CustomAssetsExample : MonoBehaviour {
  [SerializeField] private FloatAsset maxFloat;
  [SerializeField] private FloatAsset currentFloat;

  private int count;

  // Update is called once per frame
  void Update() {
    currentFloat.Value += 1;

    if (currentFloat >= maxFloat) {
      currentFloat.Value = 0;
      Debug.Log("Float " + maxFloat + " reached " + (++count) + " of 5 times");
    }
  }
}