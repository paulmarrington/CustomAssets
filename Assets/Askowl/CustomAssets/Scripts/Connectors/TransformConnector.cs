// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href=""></a> //#TBD#//
  public class TransformConnector : MonoBehaviour {
    /// <a href=""></a> //#TBD#//
    public float ScaleX {
      set => transform.localScale = new Vector3(value, transform.localScale.y, transform.localScale.z);
    }

    /// <a href=""></a> //#TBD#//
    public float ScaleY {
      set => transform.localScale = new Vector3(transform.localScale.x, value, transform.localScale.z);
    }

    /// <a href=""></a> //#TBD#//
    public float ScaleZ {
      set => transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, value);
    }

    /// <a href=""></a> //#TBD#//
    public float RotationX {
      set => transform.eulerAngles = new Vector3(value * 360, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    /// <a href=""></a> //#TBD#//
    public float RotationY {
      set => transform.eulerAngles = new Vector3(transform.eulerAngles.x, value * 360, transform.eulerAngles.z);
    }

    /// <a href=""></a> //#TBD#//
    public float RotationZ {
      set => transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, value * 360);
    }
  }
}