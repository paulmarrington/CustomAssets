using Decoupled;
using UnityEngine;
using UnityEngine.UI;

namespace CustomAsset.Mutable {
  /// <inheritdoc />
  [CreateAssetMenu(menuName = "Custom Assets/Device/WebCam"), ValueName("Device")]
  public class WebCamAsset : OfType<WebCamService> {
    private RawImage          rawImage;
    private AspectRatioFitter aspectRatioFitter;
    private int               lastVerticalMirror, lastRotationAngle = 1;

    /// <see cref="OfType{T}.Value"/>
    public WebCamService Device { get { return Value; } private set { Value = value; } }

    public bool Ready { get { return Device.DidUpdateThisFrame; } }

    /// <inheritdoc />
    public override WebCamService Initialise() { return WebCamService.Instance; }

    public void Project(GameObject background) {
      rawImage         = background.GetComponent<RawImage>() ?? background.AddComponent<RawImage>();
      rawImage.texture = Device.Texture;

      aspectRatioFitter = background.GetComponent<AspectRatioFitter>() ??
                          background.AddComponent<AspectRatioFitter>();

      aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;

      Device.Playing = true;
    }

    public void correctForDeviceScreenOrientation() {
      aspectRatioFitter.aspectRatio = Device.AspectRatio;

      int verticalMirror = Device.VerticalMirror ? -1 : 1;

      if (verticalMirror != lastVerticalMirror) {
        rawImage.rectTransform.localScale = new Vector3(x: 1, y: verticalMirror, z: 1);
        lastVerticalMirror                = verticalMirror;
      }

      int rotationAngle = Device.RotationAngle;

      if (rotationAngle != lastRotationAngle) {
        rawImage.rectTransform.localEulerAngles = new Vector3(x: 0, y: 0, z: -rotationAngle);
        lastRotationAngle                       = rotationAngle;
      }
    }
  }
}