﻿#if UNITY_EDITOR && CustomAssets
using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using CustomAsset.Mutable;

/// <inheritdoc />
/// <summary>
/// Show and update custom asset values to provide examples for their use.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public sealed class CustomAssetsExample : MonoBehaviour {
  [SerializeField] private CustomAsset.Constant.Float   maxFloat;
  [SerializeField] private Float                        currentFloat;
  [SerializeField] private Integer                      integer;
  [SerializeField] private CustomAsset.Constant.String  str;
  [SerializeField] private CustomAsset.Constant.Boolean boolean;
  [SerializeField] private Text                         textComponent;
  [SerializeField] private LargerAssetSample            largerSample;
  [SerializeField] private Integer                      persistent;
  [SerializeField] private CustomAsset.Constant.Quotes  quotes;
  [SerializeField] private Slider                       integerSlider;

  [SerializeField, UsedImplicitly] private AudioClips audioClips;
  [SerializeField]                 private UnityEvent audioClipsEvent;

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
  public void ShowCustomAssetValues() {
    textComponent.text =
      string.Format("currentFloat asset is {0}\n" +
                    "integer asset is {1}\n"      +
                    "str asset is {2}\n"          +
                    "boolean asset is {3}\n"      +
                    "larger asset is {4} / {5} / {6}",
                    (float) currentFloat, (int) integer, (string) str, (bool) boolean,
                    largerSample.AnInteger, largerSample.AFloat, largerSample.AString);
  }

  /// <summary>
  /// Make sure that a custom asset saved to persistent storage can be retrieved later
  /// </summary>
  public void CheckPersistence() {
    persistent.Set(12);
    persistent.Save();
    persistent.Set(33);
    persistent.Load();
    textComponent.text = "Persistent value - expecting 12, got " + persistent;
  }

  /// <summary>
  /// Since the component to monitor is a float we need a conversion to CustomAsset.Integer
  /// </summary>
  public void UpdateIntegerAsset() { integer.Set((int) (integerSlider.value)); }

  /// <summary>
  /// Button action to test the undating of a CustomAsset
  /// </summary>
  public void UpdateCustomFloat() { StartCoroutine(UpdateCustomFloatCoroutine()); }

  // Update is called once per frame
  private IEnumerator UpdateCustomFloatCoroutine() {
    textComponent.text = "";
    count              = 0;

    do {
      yield return new WaitForSeconds(0.1f);

      currentFloat.Set(currentFloat + 1);

      if (currentFloat >= maxFloat) {
        textComponent.text +=
          "Float " + ((float) maxFloat) + " reached " + (++count) + " of 5 times\n";

        currentFloat.Set(0);
      }
    } while (count < 5);

    currentFloat.Set(0.499f);
  }

  /// <summary>
  /// Show a random quote in the results text panel
  /// </summary>
  public void ShowQuote() { textComponent.text = quotes.Pick(); }

  /// <summary>
  /// Press a button and a random sound fires
  /// </summary>
  public void PlayAudioEvent() { audioClipsEvent.Invoke(); }

  /// <summary>
  /// Check Base.Instance for accuracy
  /// </summary>
  public void CheckInstance() {
    Float floatRef = Float.Instance("SampleFloatVariable");
    textComponent.text = "Find existing " + floatRef.name + " as " + currentFloat.name + "\n";

    Float newFloat = Float.Instance("NewFloat");
    newFloat.Set(1234);
    Float secondRef = Float.Instance("NewFloat");

    textComponent.text += "Created " + ((float) newFloat) + " same as " + ((float) secondRef);
  }
}
#endif