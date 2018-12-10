#if UNITY_EDITOR && CustomAssets
using System;
using System.Collections;
using Askowl;
using CustomAsset.Constant;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Boolean = CustomAsset.Mutable.Boolean;
using Integer = CustomAsset.Mutable.Integer;
using String = CustomAsset.Mutable.String;

/// <a href="">Show and update custom asset values to provide examples for their use</a> //#TBD#// <inheritdoc />
[RequireComponent(typeof(AudioSource))]
public sealed class CustomAssetsExample : MonoBehaviour {
  [SerializeField] private Float                     maxFloat;
  [SerializeField] private CustomAsset.Mutable.Float currentFloat;
  [SerializeField] private Integer                   integer;
  [SerializeField] private String                    str;
  [SerializeField] private Boolean                   boolean;
  [SerializeField] private Text                      textComponent;
  [SerializeField] private LargerAssetSample         largerSample;
  [SerializeField] private Integer                   persistent;
  [SerializeField] private Quotes                    quotes;
  [SerializeField] private Slider                    integerSlider;

  [SerializeField, UsedImplicitly] private AudioClips audioClips;
  [SerializeField]                 private UnityEvent audioClipsEvent;

  /// <a href="">A CustomAsset need not be just a primitive - as long as it is serializable</a> //#TBD#//
  [Serializable] public class LargerAssetContents {
    /// <a href=""></a> //#TBD#//
    public int I;

    /// <a href=""></a> //#TBD#//
    public float F;

    /// <a href=""></a> //#TBD#//
    public string S;

    /// <a href=""></a> //#TBD#//
    public override string ToString() => $"I={I}, F={F}, S={S}";

    /// <a href=""></a> //#TBD#//
    public override bool Equals(object other) =>
      (other is LargerAssetContents one) && ((one.I == I) && Compare.AlmostEqual(one.F, F) && (one.S == S));

    /// <a href=""></a> //#TBD#//
    public override int GetHashCode() => ToString().GetHashCode();
  }

  private int count;

  /// <a href="">Button action to display one of each CustomAsset under test</a> //#TBD#//
  public void ShowCustomAssetValues() {
    textComponent.text =
      $"currentFloat asset is {currentFloat}\ninteger asset is {integer}\n" +
      $"str asset is {str}\nboolean asset is {boolean}\n"                   +
      $"larger asset is {largerSample.AnInteger} / {largerSample.AFloat} / {largerSample.AString}";
  }

  /// <a href="">Make sure that a custom asset saved to persistent storage can be retrieved later</a> //#TBD#//
  public void CheckPersistence() {
    persistent.Set(12);
    persistent.Save();
    persistent.Set(33);
    persistent.Load();
    textComponent.text = "Persistent value - expecting 12, got " + persistent;
  }

  /// <a href="">Since the component to monitor is a float we need a conversion to CustomAsset.Integer</a> //#TBD#//
  public void UpdateIntegerAsset() => integer.Set((int) (integerSlider.value));

  /// <a href="">Button action to test the updating of a CustomAsset</a> //#TBD#//
  public void UpdateCustomFloat() => StartCoroutine(UpdateCustomFloatCoroutine());

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

  /// <a href="">Show a random quote in the results text panel</a> //#TBD#//
  public void ShowQuote() => textComponent.text = quotes.Pick();

  /// <a href=""></a> //#TBD#//
  public void PlayAudioClip() => audioClips.Picker.Play(gameObject);

  /// <a href="">Press a button and a random sound fires</a> //#TBD#//
  public void PlayAudioEvent() => audioClipsEvent.Invoke();

  /// <a href="">Check Base.Instance for accuracy</a> //#TBD#//
  public void CheckInstance() {
    CustomAsset.Mutable.Float floatRef = CustomAsset.Mutable.Float.Instance("SampleFloatVariable");
    textComponent.text = "Find existing " + floatRef.name + " as " + currentFloat.name + "\n";

    CustomAsset.Mutable.Float newFloat = CustomAsset.Mutable.Float.New("NewFloat");
    newFloat.Set(1234);
    CustomAsset.Mutable.Float secondRef = CustomAsset.Mutable.Float.Instance("NewFloat");

    textComponent.text += "Created " + ((float) newFloat) + " same as " + ((float) secondRef);
  }
}
#endif