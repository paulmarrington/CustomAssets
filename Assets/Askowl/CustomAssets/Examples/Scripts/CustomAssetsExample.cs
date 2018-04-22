#if UNITY_EDITOR
using System;
using System.Collections;
using CustomAsset;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Boolean = CustomAsset.Boolean;
using String = CustomAsset.String;

public sealed class CustomAssetsExample : MonoBehaviour {
  [SerializeField] private Float             maxFloat;
  [SerializeField] private Float             currentFloat;
  [SerializeField] private Integer           integer;
  [SerializeField] private String            str;
  [SerializeField] private Boolean           boolean;
  [SerializeField] private Text              textComponent;
  [SerializeField] private LargerAssetSample largerSample;

  [Serializable]
  public class LargerAssetContents {
    public int    I;
    public float  F;
    public string S;
  }

  private int count;

  [UsedImplicitly]
  public void ShowCustomAssetValues() {
    textComponent.text =
      string.Format("currentFloat asset is {0}\n" +
                    "integer asset is {1}\n"      +
                    "str asset is {2}\n"          +
                    "boolean asset is {3}\n"      +
                    "larger asset is {4} / {5} / {6}",
                    (float) currentFloat, (int) integer, str, (bool) boolean,
                    largerSample.Value.I, largerSample.Value.F,
                    largerSample.Value.S);
  }

  [UsedImplicitly]
  public void UpdateCustomFloat() { StartCoroutine(UpdateCustomFloatCoroutine()); }

  // Update is called once per frame
  private IEnumerator UpdateCustomFloatCoroutine() {
    textComponent.text = "";

    do {
      yield return new WaitForSeconds(0.1f);

      currentFloat.Value += 1;

      if (currentFloat >= maxFloat) {
        currentFloat.Value = 0;

        textComponent.text +=
          "Float " + ((float) maxFloat) + " reached " + (++count) + " of 5 times\n";
      }
    } while (count < 5);
  }
}
#endif