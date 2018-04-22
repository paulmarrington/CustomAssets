using UnityEngine.UI;

namespace Events {
  public sealed class TextListener : StringListener<Text> {
    protected override void Change(string value) { Component.text = value; }
  }
}