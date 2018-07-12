using Askowl;
using Decoupled;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <inheritdoc cref="Decoupled.GPSService" />
  [CreateAssetMenu(menuName = "Custom Assets/Device/GPS"), ValueName("Device")]
// ReSharper disable once InconsistentNaming
  public class GPSAsset : OfType<GPSService> {
    /// <see cref="OfType{T}.Value"/>
    public GPSService Device { get { return Value; } set { Value = value; } }

    public bool                 Ready        => Device.Running;
    public bool                 Initialising => Device.Initialising;
    public bool                 Offline      => Device.Offline;
    public Geodetic.Coordinates Here         => Location(Device.Location);

    public Geodetic.Coordinates Location(GPSService.LocationData here) =>
      Geodetic.Coords(here.Latitude, here.Longitude);

    /// <inheritdoc />
    public override void Initialise() { Device = GPSService.Instance; }
  }
}