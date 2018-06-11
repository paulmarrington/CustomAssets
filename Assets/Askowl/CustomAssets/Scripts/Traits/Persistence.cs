// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;

namespace CustomAsset {
  public abstract partial class OfType<T> {
    [Header("Variability"), SerializeField, Tooltip("Allow the data to be changed")]
    private bool readWrite = true;

    [SerializeField, Tooltip("Save to storage")]
    private bool persistent;

    /// <summary>
    /// Does the CustomAsset have permission to change it's values?
    /// </summary>
    public bool ChangeAllowed { get { return (readWrite || persistent); } }

    private bool AmChanging {
      get {
        if (!ChangeAllowed) {
          Debug.LogErrorFormat("{0} / {1} is read-only so cannot be changed", name, typeof(T));
        }

        return ChangeAllowed;
      }
    }

    private string Key { get { return string.Format("{0}:{1}", name, typeof(T)); } }

    /// <summary>
    /// Get or set the Persistence flag to override the inspector setting
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#instance">More...</a></remarks>
    public bool Persistent { get { return persistent; } set { persistent = value; } }

    /// <summary>
    /// Load the last previously saved value from persistent storage.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#custom-asset-persistence">More...</a></remarks>
    protected TS Loader<TS>() {
      if (!persistent) return default(TS);

      string json = PlayerPrefs.GetString(Key, defaultValue: "");

      return JsonUtility.FromJson<TS>(json);
    }

    /// <summary>
    /// Save data to persistent storage for the next run.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#custom-asset-persistence">More...</a></remarks>
    /// <param name="data">data to save</param>
    /// <typeparam name="TS">Class for save data</typeparam>
    // ReSharper disable once MemberCanBePrivate.Global
    protected void Saver<TS>(TS data) {
      if (persistent) PlayerPrefs.SetString(Key, JsonUtility.ToJson(data));
    }
  }
}