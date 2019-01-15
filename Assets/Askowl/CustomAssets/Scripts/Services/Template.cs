// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages

using CustomAsset.Services;
using UnityEngine;

namespace CustomAsset.Services {
  /// <a href=""></a> //#TBD#//
  [CreateAssetMenu(menuName = "CustomAsset/Services/Template/Referent", fileName = "Template")]
  public class Template : Referent<Elector, Service, Context> { }

  /// <a href=""></a> //#TBD#//
  [CreateAssetMenu(menuName = "CustomAsset/Services/Template/Selector", fileName = "TemplateSelector")]
  public class Elector : Template.Elector { }

  /// <a href=""></a> //#TBD#//
  [CreateAssetMenu(menuName = "CustomAsset/Services/Template/Service", fileName = "TemplateSelector")]
  public class Service : Template.Service { }

  /// <a href=""></a> //#TBD#//
  [CreateAssetMenu(menuName = "CustomAsset/Services/Template/Context", fileName = "TemplateContext")]
  public class Context : Template.Context { }
}