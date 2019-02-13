#if AskowlTests
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

// ReSharper disable MissingXmlDoc
namespace Askowl.CustomAssets.Examples {
  [RequireComponent(typeof(AudioSource))]
  public sealed class CustomAssetsExample : MonoBehaviour {
    [SerializeField] private Float                     maxFloat      = default;
    [SerializeField] private CustomAsset.Mutable.Float currentFloat  = default;
    [SerializeField] private Integer                   integer       = default;
    [SerializeField] private String                    str           = default;
    [SerializeField] private Boolean                   boolean       = default;
    [SerializeField] private Text                      textComponent = default;
    [SerializeField] private LargerAssetSample         largerSample  = default;
    [SerializeField] private Integer                   persistent    = default;
    [SerializeField] private Quotes                    quotes        = default;
    [SerializeField] private Slider                    integerSlider = default;

    [SerializeField, UsedImplicitly] private AudioClips audioClips      = default;
    [SerializeField]                 private UnityEvent audioClipsEvent = default;

    [Serializable] public class LargerAssetContents {
      public int I;

      public float f;

      public string s;

      public override string ToString() => $"I={I}, F={f}, S={s}";

      public override bool Equals(object other) =>
        (other is LargerAssetContents one) && ((one.I == I) && Compare.AlmostEqual(one.f, f) && (one.s == s));

      public override int GetHashCode() => ToString().GetHashCode();
    }

    private int count;

    public void ShowCustomAssetValues() =>
      textComponent.text =
        $"currentFloat asset is {currentFloat}\ninteger asset is {integer}\n" +
        $"str asset is {str}\nboolean asset is {boolean}\n"                   +
        $"larger asset is {largerSample.AnInteger} / {largerSample.AFloat} / {largerSample.AString}";

    public void CheckPersistence() {
      persistent.Value = 12;
      persistent.Save();
      persistent.Value = 33;
      persistent.Load();
      textComponent.text = "Persistent value - expecting 12, got " + persistent;
    }

    public void UpdateIntegerAsset() => integer.Value = (int) integerSlider.value;

    public void UpdateCustomFloat() => StartCoroutine(UpdateCustomFloatCoroutine());

    private IEnumerator UpdateCustomFloatCoroutine() {
      textComponent.text = "";
      count              = 0;
      currentFloat.Value = 0.4f;

      do {
        yield return new WaitForSeconds(0.1f);

        currentFloat.Value += 0.1f;

        if (currentFloat >= maxFloat) {
          textComponent.text += "Float " + ((float) maxFloat) + " reached " + (++count) + " of 5 times\n";

          currentFloat.Set(0);
        }
      } while (count < 5);

      currentFloat.Set(0.499f);
    }

    public void ShowQuote() => textComponent.text = quotes.Pick();

    public void PlayAudioClip() => audioClips.Picker.Play(gameObject);

    public void PlayAudioEvent() => audioClipsEvent.Invoke();

    public void CheckInstance() {
      CustomAsset.Mutable.Float floatRef = CustomAsset.Mutable.Float.Instance("SampleFloatVariable");
      textComponent.text = "Find existing " + floatRef.name + " as " + currentFloat.name + "\n";

      CustomAsset.Mutable.Float newFloat = CustomAsset.Mutable.Float.New("NewFloat");
      newFloat.Set(0.1234f);
      CustomAsset.Mutable.Float secondRef = CustomAsset.Mutable.Float.Instance("NewFloat");

      textComponent.text += "Created " + ((float) newFloat) + " same as " + ((float) secondRef);
    }
  }
}
#endif