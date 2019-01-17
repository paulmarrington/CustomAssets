// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;

namespace CustomAsset.Services.Template {
  /// <a href=""></a> //#TBD#//
  [CreateAssetMenu(menuName = "Custom Assets/Services/Template/Service", fileName = "TemplateSelector")]
  public class Service : Referent<Elector, Service, Context>.Service { }
}