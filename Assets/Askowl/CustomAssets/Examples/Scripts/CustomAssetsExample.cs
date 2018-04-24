#if UNITY_EDITOR
using System;
using System.Collections;
using CustomAsset;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Boolean = CustomAsset.Boolean;
using String = CustomAsset.String;

/// <summary>
/// Show and update custom asset values to provide examples for their use.
/// </summary>
public sealed class CustomAssetsExample : MonoBehaviour {
  [SerializeField] private Float             maxFloat;
  [SerializeField] private Float             currentFloat;
  [SerializeField] private Integer           integer;
  [SerializeField] private String            str;
  [SerializeField] private Boolean           boolean;
  [SerializeField] private Text              textComponent;
  [SerializeField] private LargerAssetSample largerSample;

  public float UpdateIntegerValue {
    get { return integer; }
    set { integer.Value = (int) (value * 10); }
  }

  /// <summary>
  /// A CustomAsset need not be just a primative - as long as it is serializable.
  /// </summary>
  [Serializable]
  public class LargerAssetContents {
    // ReSharper disable MissingXmlDoc

    public int    I;
    public float  F;
    public string S;

    // ReSharper restore MissingXmlDoc
  }

  private int count;

  /// <summary>
  /// Button action to display one of each CustomAsset under test.
  /// </summary>
  [UsedImplicitly]
  public void ShowCustomAssetValues() {
    textComponent.text =
      string.Format("currentFloat asset is {0}\n" +
                    "integer asset is {1}\n"      +
                    "str asset is {2}\n"          +
                    "boolean asset is {3}\n"      +
                    "larger asset is {4} / {5} / {6}",
                    (float) currentFloat, (int) integer, (string) str, (bool) boolean,
                    largerSample.Value.I, largerSample.Value.F,
                    largerSample.Value.S);
  }

  /// <summary>
  /// Button action to test the undating of a CustomAsset
  /// </summary>
  [UsedImplicitly]
  public void UpdateCustomFloat() { StartCoroutine(UpdateCustomFloatCoroutine()); }

  // Update is called once per frame
  private IEnumerator UpdateCustomFloatCoroutine() {
    textComponent.text = "";
    count              = 0;

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