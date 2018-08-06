using Askowl;
using Decoupled;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <remarks><a href="http://unitydoc.marrington.net/Mars#asset-1">More...</a></remarks>
  /// <inheritdoc cref="Decoupled.GPSService" />
  [CreateAssetMenu(menuName = "Custom Assets/Device/GPS"), ValueName("Device")]
// ReSharper disable once InconsistentNaming
  public class GPSAsset : OfType<GPSService> {
    /// Access to the underlying service. <see cref="OfType{T}.Value"/>
    public GPSService Device { get { return Value; } set { Value = value; } }

    /// <summary>
    /// Poll to see if GPS is ready to use - take care, it may be still settling
    /// </summary>
    public bool Ready => Device.Running;

    /// <summary>
    /// Poll to see if the GPS is still initialising
    /// </summary>
    public bool Initialising => Device.Initialising;

    /// <summary>
    /// The GPS is offline if we cannot access or enable it
    /// </summary>
    public bool Offline => Device.Offline;

    /// <summary>
    /// Retrieve the current location in Geodic coordinates
    /// </summary>
    public Geodetic.Coordinates Here => Location(Device.Location);

    /// <summary>
    /// Convert device service coordinates to Geodetic ones for further calculations.
    /// </summary>
    /// <param name="here">GPSService location to convert</param>
    /// <returns>Geodetic.Coordinates to calculate bearings, distance and more</returns>
    public static Geodetic.Coordinates Location(GPSService.LocationData here) =>
      Geodetic.Coords(here.Latitude, here.Longitude);

    /// <inheritdoc />
    public override void Initialise() { Device = GPSService.Instance; }
  }
}