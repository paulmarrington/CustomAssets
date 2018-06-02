// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

namespace CustomAsset {
  using System.Linq;
  using JetBrains.Annotations;
  using UnityEngine;

  /// <summary>
  /// Static helper class to reduce scaffolding when working with components
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#components">More...</a></remarks>
  public static class Components {
    /// <summary>
    /// Find a component by type given it's GameObject parent. If not found, a global find is attempted.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#componentsfindingameobject">More...</a></remarks>
    /// <param name="gameObject">The GameObject we are expecting the component to be inside of</param>
    /// <typeparam name="T">Type of component we are looking for</typeparam>
    /// <returns>a reference to the named object</returns>
    
    public static T Find<T>(GameObject gameObject) where T : Object {
      return gameObject.GetComponent<T>() ?? Find<T>();
    }

    /// <summary>
    /// Find a component of the supplied type and optionally with the specified name
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#componentsfindtname">More...</a></remarks>
    /// <param name="name">Name of component as set in Unity editor. Null to use any name</param>
    /// <typeparam name="T">Type of component we are looking for</typeparam>
    /// <returns>a reference to the named object</returns>
    public static T Find<T>(string name = null) where T : Object {
      name = string.IsNullOrEmpty(name) ? typeof(T).Name : name;
      T[] objects = Object.FindObjectsOfType<T>();

      foreach (T obj in objects.Where(obj => obj.name.Equals(name))) return obj;

      T resource = Resources.Load<T>(path: name);
      return resource;
    }

    /// <summary>
    /// Create a new GameObject, name it and add a component of the specified type.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#componentscreatetname">More...</a></remarks>
    /// <param name="name">The name given to both the game object and contained component</param>
    /// <typeparam name="T">Type of component we are creating</typeparam>
    /// <returns>a reference to the game object containing the newly minted component</returns>
    
    public static T Create<T>(string name = null) where T : Component {
      GameObject gameObject = new GameObject();

      T instance = Create<T>(gameObject, name);
      gameObject.name = instance.name;
      return instance;
    }

    /// <summary>
    /// Create a new GameObject, name it and add a component of the specified type.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#componentscreatetgameobjectname">More...</a></remarks>
    /// <param name="gameObject">The existing game object to which we are adding a new component</param>
    /// <param name="name">The name given to both the game object and contained component</param>
    /// <typeparam name="T">Type of component we are creating</typeparam>
    /// <returns>a reference to the game object containing the newly minted component</returns>
    
    public static T Create<T>(GameObject gameObject, string name = null)
      where T : Component {
      T instance = gameObject.AddComponent<T>();
      instance.name = name ?? typeof(T).ToString();
      return instance;
    }
  }
}