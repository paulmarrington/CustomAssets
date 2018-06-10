#if UNITY_EDITOR && CustomAssets
using System;
using System.Collections;
using CustomAsset;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Boolean = CustomAsset.Boolean;
using String = CustomAsset.String;

/// <inheritdoc />
/// <summary>
/// Show and update custom asset values to provide examples for their use.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public sealed class CustomAssetsExample : MonoBehaviour {
  [SerializeField] private Float             maxFloat;
  [SerializeField] private Float             currentFloat;
  [SerializeField] private Integer           integer;
  [SerializeField] private String            str;
  [SerializeField] private Boolean           boolean;
  [SerializeField] private Text              textComponent;
  [SerializeField] private LargerAssetSample largerSample;
  [SerializeField] private Integer           persistent;
  [SerializeField] private Float             withMembers;
  [SerializeField] private Quotes            quotes;
  [SerializeField] private Slider            integerSlider;

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
  // ReSharper disable once UnusedMember.Global
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
  // ReSharper disable once UnusedMember.Global
  public void CheckPersistence() {
    persistent.Value = 12;
    persistent.Save();
    persistent.Value = 33;
    persistent.Load();
    textComponent.text = "Persistent value - expecting 12, got " + persistent;
  }

  /// <summary>
  /// Checks that members can be added in the inspector and dunamically. Also tests
  /// persistence by writing them out,clearing then retrieving them again.
  /// </summary>
  // ReSharper disable once UnusedMember.Global
  public void CheckMemberPersistence() {
    withMembers["One"]     = 22;
    withMembers["Dynamic"] = 44;
    withMembers.Save();
    withMembers.Clear();
    withMembers.Load();

    string[] memberNames = withMembers.MemberNames;

    textComponent.text =
      "withMembers has " + memberNames.Length + " and a seed of " + (float) withMembers;

    for (int i = 0; i < memberNames.Length; i++) {
      textComponent.text += "\n" + memberNames[i] + " = " + withMembers[memberNames[i]];
    }
  }

  /// <summary>
  /// Since the component to monitor is a float we need a conversion to CustomAsset.Integer
  /// </summary>
  // ReSharper disable once UnusedMember.Global
  public void UpdateIntegerAsset() { integer.Value = (int) (integerSlider.value); }

  /// <summary>
  /// Button action to test the undating of a CustomAsset
  /// </summary>
  // ReSharper disable once UnusedMember.Global
  public void UpdateCustomFloat() { StartCoroutine(UpdateCustomFloatCoroutine()); }

  // Update is called once per frame
  private IEnumerator UpdateCustomFloatCoroutine() {
    textComponent.text = "";
    count              = 0;

    do {
      yield return new WaitForSeconds(0.1f);

      currentFloat.Value = currentFloat + 1;

      if (!(currentFloat >= maxFloat)) continue;

      textComponent.text +=
        "Float " + ((float) maxFloat) + " reached " + (++count) + " of 5 times\n";
    } while (count < 5);

    currentFloat.Value = 0.499f;
  }

  /// <summary>
  /// Show a random quote in the results text panel
  /// </summary>
  // ReSharper disable once UnusedMember.Global
  public void ShowQuote() { textComponent.text = quotes.Pick(); }

  /// <summary>
  /// Press a button and a random sound fires
  /// </summary>
  // ReSharper disable once UnusedMember.Global
  public void PlayAudioEvent() { audioClipsEvent.Invoke(); }

  /// <summary>
  /// Check Base.Instance for accuracy
  /// </summary>
  // ReSharper disable once UnusedMember.Global
  public void CheckInstance() {
    Float floatRef = Base.Instance<Float>("SampleFloatVariable");
    textComponent.text = "Find existing " + floatRef.name + " as " + currentFloat.name + "\n";

    Float newFloat = Base.Instance<Float>("NewFloat");
    newFloat.Value = 1234;
    Float secondRef = Base.Instance<Float>("NewFloat");

    textComponent.text += "Created " + ((float) newFloat) + " same as " + ((float) secondRef);
  }
}
#endif