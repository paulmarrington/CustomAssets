// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;

namespace CustomAsset.Services {
  /// <a href=""></a><inheritdoc /> //#TBD#//
  [CreateAssetMenu(menuName = "Custom Assets/Services/Template/Service", fileName = "TemplateSelector")]
  public abstract class TemplateServiceConnector : Services<TemplateServiceConnector, TemplateContext>.ServiceConnector { }
}