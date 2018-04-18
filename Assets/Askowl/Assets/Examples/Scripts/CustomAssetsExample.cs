using CustomAsset;
using UnityEngine;

public sealed class CustomAssetsExample : MonoBehaviour {
  [SerializeField] private Float   maxFloat;
  [SerializeField] private Float   currentFloat;
  [SerializeField] private Integer integer;
  [SerializeField] private String  str;
  [SerializeField] private Boolean boolean;

  private int count;

  private void Awake() {
    Debug.Log("maxFloat asset is " + (float) maxFloat);
    Debug.Log("integer asset is "  + (int) integer);
    Debug.Log("str asset is "      + str);
    Debug.Log("boolean asset is "  + (bool) boolean);
  }

  // Update is called once per frame
  void Update() {
    currentFloat.Value += 1;

    if (currentFloat >= maxFloat) {
      currentFloat.Value = 0;
      Debug.Log("Float " + maxFloat + " reached " + (++count) + " of 5 times");
      if (count >= 5) gameObject.SetActive(false);
    }
  }
}