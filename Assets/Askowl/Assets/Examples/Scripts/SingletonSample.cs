using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonSample : Singleton<SingletonSample> {
  public int value = 0;
}