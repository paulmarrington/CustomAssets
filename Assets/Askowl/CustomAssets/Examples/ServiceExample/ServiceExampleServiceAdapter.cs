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
      var service = emitter.Context<Service>();
      var error   = service.ErrorMessage;
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
    public class AddDto : DelayedCache<AddDto> {
      public struct Request {
        public int firstValue;
        public int secondValue;
      }
      public Request request;
      public int     result;
    }
    /// Abstract services - one per dto type
    protected abstract string Serve(AddDto dto, Emitter emitter);
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