// With thanks to Jason Weimann  -- jason@unity3d.college

using System;
using CustomAsset;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CustomAsset {
  [Serializable]
  public class Range : Pick<float> {
    [SerializeField, UsedImplicitly] private float min;
    [SerializeField, UsedImplicitly] private float max;

    public float Min { get { return min; } private set { min = value; } }
    public float Max { get { return max; } private set { max = value; } }

    public Range() { }

    public Range(float min, float max) {
      Min = min;
      Max = max;
    }

    public float Pick() { return Random.Range(min, max); }
  }
}

public class RangeBoundsAttribute : Attribute {
  public RangeBoundsAttribute(float min, float max) {
    Min = min;
    Max = max;
  }

  public float Max { get; private set; }

  public float Min { get; private set; }
}