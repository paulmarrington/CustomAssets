// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using System.Linq;
using Askowl;
using UnityEngine;

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
    public int usagesRemaining;
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
        IsRandom        = order <= Order.RoundRobin
      , ExhaustiveBelow = order == Order.RandomExhaustive ? services.Length : 0
      , Choices         = services
      };
    }

    /// <a href="">Get the next service instance given selection order and repetitions</a> //#TBD#//
    public ServiceAdapter Instance {
      get {
        if (--usagesRemaining > 0) return currentService;
        currentService  = selector.Pick();
        usagesRemaining = currentService.usageBalance;
        return currentService;
      }
    }

    /// <a href="">If the last service fails, ask for another. If none work, returns null</a> //#TBD#//
    public ServiceAdapter Next {
      get {
        currentService  = selector.Next();
        usagesRemaining = currentService.usageBalance;
        return currentService;
      }
    }

    /// <a href="">Parent class for decoupled services</a>
    [Serializable] public abstract class ServiceAdapter : Base, Server {
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

      /// <a href=""></a> //#TBD#//
      protected virtual string Serve<T>(T dto, Emitter emitter) => throw new NotImplementedException();

      /// <a href=""></a> //#TBD#//
      public Service<T> Service<T>() where T : DelayedCache<T> {
        var service = Cache<Service<T>>.Instance;
        service.Server       = this;
        service.Dto          = DelayedCache<T>.Instance;
        service.Emitter      = Emitter.SingleFireInstance.Listen(logOnResponse);
        service.ErrorMessage = null;
        service.Emitter.Context(service);
        return service;
      }

      /// <a href=""></a> //#TBD#//
      public Emitter Call<T>(Service<T> service) {
        service.ErrorMessage = null;
        service.ErrorMessage = Serve(service.Dto, service.Emitter);
        if (service.ErrorMessage == default) return service.Emitter;
        service.Emitter.Dispose(); // since it does not get fired
        return null;
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
  public interface Server {
    /// <a href=""></a> //#TBD#//
    Emitter Call<T>(Service<T> service);
  }

  /// <a href=""></a> //#TBD#//
  public class Service : IDisposable {
    /// <a href="">Is default for no error, empty for no logging of a message else error message</a> //#TBD#//
    public string ErrorMessage { get; set; }

    /// <a href=""></a> //#TBD#//
    public Emitter Emitter;

    /// <a href=""></a> //#TBD#//
    public Server Server;

    public void Dispose() => Emitter?.Dispose(); // Dto disposed of by the same command
  }

  /// <a href=""></a> //#TBD#//
  public class Service<T> : Service {
    /// <a href=""></a> //#TBD#//
    public T Dto;

    /// <a href=""></a> //#TBD#//
    public Emitter Call() => Server.Call<T>(this);
  }
}