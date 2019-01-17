// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;

namespace CustomAsset.Services {
  /// <a href=""></a> //#TBD#//
  [CreateAssetMenu(menuName = "Custom Assets/Services/Template/Selector", fileName = "TemplateSelector")]
  public class TemplateElector : Referent<TemplateElector, TemplateService, TemplateContext>.Elector { }
}