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

    public bool Ready { get { return Device.Running; } }

    public bool Initialising { get { return Device.Initialising; } }
    public bool Offline      { get { return Device.Offline; } }

    public Geodetic.Coordinates Here { get { return Location(Device.Location); } }

    public Geodetic.Coordinates Location(GPSService.LocationData here) {
      return Geodetic.Coords(here.Latitude, here.Longitude);
    }

    /// <inheritdoc />
    public override GPSService Initialise() { return Device = GPSService.Instance; }
  }
}