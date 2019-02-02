// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages

using Askowl;
using UnityEditor;
// ReSharper disable MissingXmlDoc

namespace CustomAsset.Services {
  /// <a href=""></a><inheritdoc /> //#TBD#//
  public abstract class
    ServiceExampleServiceAdapter : Services<ServiceExampleServiceAdapter, ServiceExampleContext>.ServiceAdapter {
    #region Service Support
    /// <a href=""></a> //#TBD#//
    protected override void Prepare() { }

    protected override void LogOnResponse(Emitter emitter) {
      var dto   = emitter.Context<ServiceDto>();
      var error = dto.ErrorMessage;
      if (error != default) {
        if (!string.IsNullOrEmpty(error)) Error($"Service Error: {error}");
      } else {
        Log("Warning", $"LogOnResponse for '{GetType().Name}");
      }
    }

    // Code that is common to all services belongs here
    #endregion

    #region Public Interface
    // Methods calling code will use to call a service - over and above concrete interface methods below ones defined below.
    #endregion

    #region Service Interface Methods
    // List of virtual interface methods that all concrete service adapters need to implement.

    // **************** Start of ServiceExampleServiceMethod **************** //
    /// A service dto contains data required to call service and data returned from said call
    public class ServiceExampleServiceDto : Cached<ServiceExampleServiceDto>, ServiceDto {
      public string  ErrorMessage { get; set; }
      public Emitter Emitter      { get; set; }
      public string  Result;
      public void    Clear() { } // clear previous results if necessary
    }
    /// Abstract services - one per dto type
    protected abstract void Serve(ServiceExampleServiceDto dto);
    // **************** End of ServiceExampleServiceMethod **************** //
    #endregion

    #region Compiler Definition
    #if ServiceExampleServiceFor
    public override bool IsExternalServiceAvailable() => true;
    #else
    public override bool IsExternalServiceAvailable() => false;
    #endif

    [InitializeOnLoadMethod] private static void DetectService() {
      bool usable = DefineSymbols.HasPackage("") || DefineSymbols.HasFolder("");
      DefineSymbols.AddOrRemoveDefines(addDefines: usable, named: "ServiceExampleServiceFor");
    }
    #endregion
  }
}