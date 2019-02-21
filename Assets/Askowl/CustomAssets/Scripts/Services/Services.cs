// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using System.Linq;
using Askowl;
using UnityEngine;
using UnityEngine.Assertions;

namespace CustomAsset.Services {
  /// <a href="">Separate selection and service from context for easy Inspector configuration</a> //#TBD#//
  public class Services<TS, TC> : Manager
    where TS : Services<TS, TC>.ServiceAdapter
    where TC : Services<TS, TC>.Context {
    /// <a href=""></a> //#TBD#//
    // ReSharper disable MissingXmlDoc
    public enum Order { TopDown, RoundRobin, Random, RandomExhaustive }
    // ReSharper restore MissingXmlDoc

    [SerializeField] private TS[]  services = default;
    [SerializeField] private TC    context  = default;
    [SerializeField] private Order order    = Order.TopDown;

    /// <a href="">Used in testing.</a> //#TBD#//
    public Selector<TS> selector;
    /// <a href=""></a> //#TBD#//
    [HideInInspector] public int usagesRemaining;
    /// <a href=""></a> //#TBD#//
    [HideInInspector] public TS currentService;

    /// <inheritdoc />
    protected override void Initialise() {
      // only use services that are valid for the current context
      var useful = services.Where(service => service.context.Equals(context) && service.IsExternalServiceAvailable());
      services = useful.ToArray();
      // service processing order is dependent on the priority each gives
      Array.Sort(array: services, comparison: (x, y) => x.priority.CompareTo(value: y.priority));

      selector = new Selector<TS> {
        IsRandom        = order > Order.RoundRobin
      , ExhaustiveBelow = order == Order.RandomExhaustive ? services.Length + 1 : 0
      , Choices         = services
      };
      usagesRemaining = 0;
    }

    /// <a href="">Get the next service instance given selection order and repetitions</a> //#TBD#//
    public TI Instance<TI>() where TI : TS {
      if (order == Order.TopDown) {
        selector.Top();
      } else if (--usagesRemaining > 0) {
        return (TI) currentService;
      }
      currentService  = selector.Pick();
      usagesRemaining = currentService.usageBalance;
      return (TI) currentService;
    }

    /// <a href="">If the last service fails, ask for another. If none work, returns null</a> //#TBD#//
    public TI Next<TI>() where TI : TS {
      currentService = selector.Next();
      if (currentService == default) return default;
      usagesRemaining = currentService.usageBalance;
      return (TI) currentService;
    }

    /// <a href=""></a> //#TBD#//
    public Emitter CallService(Service service) => CallServiceFiber.Go((this, Instance<TS>(), service)).OnComplete; //#TBD#//

    private class CallServiceFiber : Fiber.Closure<(CallServiceFiber, Services<TS, TC> manager, TS server, Service service)> {
      protected override void Activities(Fiber fiber) =>
        fiber.Begin
             .WaitFor(_ => MethodCache.Call(scope.server, "Call", new object[] {scope.service.Reset()}) as Emitter)
             .Until(_ => !scope.service.Error || ((scope.server = scope.manager.Next<TS>()) == null));
    }

    /// <a href="">Parent class for decoupled services</a>
    [Serializable] public abstract class ServiceAdapter : Base {
      /// <a href=""></a> //#TBD#//
      [SerializeField] internal int priority = 1;
      /// <a href=""></a> //#TBD#//
      [SerializeField] internal int usageBalance = 1;
      /// <a href=""></a> //#TBD#//
      [SerializeField] public TC context = default;

      /// <a href=""></a> //#TBD#//
      protected Log.MessageRecorder Log;
      /// <a href=""></a> //#TBD#//
      protected Log.EventRecorder Error;

      /// <a href="">Concrete service implements this to prepare for action</a> //#TBD#//
      protected abstract void Prepare();

      /// <a href=""></a> //#TBD#//
      public abstract bool IsExternalServiceAvailable();

      /// <a href="">Registered with Emitter to provide common logging</a> //#TBD#/
      protected abstract void LogOnResponse(Emitter emitter);
      private Emitter.Action logOnResponse;

      /// <a href="">Override to initialise concrete service instances</a>
      protected override void Initialise() {
        base.Initialise();
        Log           = Askowl.Log.Messages();
        Error         = Askowl.Log.Errors();
        logOnResponse = LogOnResponse;
        Prepare();
      }
    }

    /// <a href=""></a> //#TBD#//
    [Serializable] public class Context : Base {
      /// <a href="">Production, Staging, Test, Dev or user defined environment</a> //#TBD#//
      [SerializeField] private Environment environment = default;

      /// <a href=""></a> //#TBD#//
      protected virtual bool Equals(Context other) => base.Equals(other) && Equals(environment, other.environment);

      /// <inheritdoc />
      public override int GetHashCode() {
        // ReSharper disable NonReadonlyMemberInGetHashCode
        unchecked { return (base.GetHashCode() * 397) ^ (environment != null ? environment.GetHashCode() : 0); }
        // ReSharper restore NonReadonlyMemberInGetHashCode
      }
    }
  }
  /// <a href=""></a> //#TBD#//
  public class Service : IDisposable {
    /// <a href="">Is default for no error, empty for no logging of a message else error message</a> //#TBD#//
    public string ErrorMessage { get; set; }

    /// <a href=""></a> //#TBD#//
    public Boolean Error => ErrorMessage != null;

    /// <a href=""></a> //#TBD#//
    public Emitter Emitter => emitter ?? (emitter = Emitter.SingleFireInstance);
    private Emitter emitter;

    public void Dispose() {
      ErrorMessage = null;
      var em = Emitter;
      emitter = null; // so we don't spin out
      em?.Dispose();  // Dto disposed of by the same command
    }

    /// <a href=""></a> //#TBD#//
    public Service Reset() {
      Dispose();
      return this;
    }
  }

  /// <a href=""></a> //#TBD#//
  public class Service<T> : Service where T : DelayedCache<T> {
    /// <a href=""></a> //#TBD#//
    public static Service<T> Instance {
      get {
        var service = Cache<Service<T>>.Instance;
        service.Dto = DelayedCache<T>.Instance;
        service.Reset();
        return service;
      }
    }

    /// <a href=""></a> //#TBD#//
    public T Dto;

    public override string ToString() => Dto.ToString();
  }
}