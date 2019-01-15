// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages

namespace CustomAsset.Services {
  /// <a href=""></a> //#TBD#//
  //[CreateAssetMenu(menuName = "Custom Assets/Services/Template/Referent", fileName = "Template")]
  public class Template : Referent<Elector, Service, Context> { }

  /// <a href=""></a> //#TBD#//
  //[CreateAssetMenu(menuName = "Custom Assets/Services/Template/Selector", fileName = "TemplateSelector")]
  public class Elector : Referent<Elector, Service, Context>.Elector { }

  /// <a href=""></a> //#TBD#//
  //[CreateAssetMenu(menuName = "Custom Assets/Services/Template/Service", fileName = "TemplateSelector")]
  public class Service : Referent<Elector, Service, Context>.Service { }

  /// <a href=""></a> //#TBD#//
  //[CreateAssetMenu(menuName = "Custom Assets/Services/Template/Context", fileName = "TemplateContext")]
  public class Context : Referent<Elector, Service, Context>.Context { }
}