using JetBrains.Annotations;
using UnityEngine;

namespace CustomAsset {
  public abstract partial class OfType<T> {
    [Header("Variability"), SerializeField, Tooltip("Allow the data to be changed")]
    private bool readWrite;

    [SerializeField, Tooltip("Save to storage")]
    private bool persistent;

    private string Key { get { return string.Format("{0}:{1}", name, typeof(T)); } }

    /// <summary>
    /// Load the last previously saved value from persistent storage.
    /// </summary>
    /// <remarks><a href="http://customassets.marrington.net#custom-asset-persistence">More...</a></remarks>
    [UsedImplicitly]
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