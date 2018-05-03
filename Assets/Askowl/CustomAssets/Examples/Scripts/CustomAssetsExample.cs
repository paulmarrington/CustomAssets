#if UNITY_EDITOR && CustomAssets
using System;
using System.Collections;
using CustomAsset;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Boolean = CustomAsset.Boolean;
using String = CustomAsset.String;

/// <inheritdoc />
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
  [SerializeField] private Integer           persistent;
  [SerializeField] private Float             critical;
  [SerializeField] private Quotes            quotes;

  [SerializeField] private Slider integerSlider;

//  public float UpdateIntegerValue {
//    get { return integer; }
//    set { integer.Value = (int) (value * 10); }
//  }

  /// <summary>
  /// A CustomAsset need not be just a primative - as long as it is serializable.
  /// </summary>
  [Serializable]
  public class LargerAssetContents {
    // ReSharper disable MissingXmlDoc
    // ReSharper disable UnassignedField.Global

    public int    I;
    public float  F;
    public string S;

    // ReSharper restore UnassignedField.Global
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
  /// Make sure that a custom asset saved to persistent storage can be retrieved later
  /// </summary>
  [UsedImplicitly]
  public void CheckPersistence() {
    persistent.Value = 12;
    persistent.Save();
    persistent.Value = 33;
    persistent.Load();
    textComponent.text = "Persistent value - expecting 12, got " + persistent.Value;
  }

  /// <summary>
  /// Make sure that persistent custom assets marked as critical are saved on change
  /// </summary>
  [UsedImplicitly]
  public void CheckCriticalPersistence() {
    critical.Value = 44;
    critical.Load();
    textComponent.text = "Persistent value - expecting 44, got " + critical.Value;
  }

  /// <summary>
  /// Since the component to monitor is a float we need a conversion to CustomAsset.Integer
  /// </summary>
  [UsedImplicitly]
  public void UpdateIntegerAsset() { integer.Value = (int) (integerSlider.value * 100); }

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

      if (!(currentFloat >= maxFloat)) continue;

      currentFloat.Value = 0;

      textComponent.text +=
        "Float " + ((float) maxFloat) + " reached " + (++count) + " of 5 times\n";
    } while (count < 5);
  }

  /// <summary>
  /// Show a random quote in the results text panel
  /// </summary>
  [UsedImplicitly]
  public void ShowQuote() { textComponent.text = quotes.Pick(); }
}
#endif