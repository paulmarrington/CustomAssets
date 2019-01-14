//// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages
//
//using Askowl;
//using UnityEngine;
//using UnityEngine.UI;
//
//namespace CustomAsset.Mutable {
//  /// <a href="http://bit.ly/2RbzvKP">Access web-cam footage as a custom asset</a> <inheritdoc />
//  [CreateAssetMenu(menuName = "Custom Assets/Device/WebCam"), Labels("Device")]
//  public class WebCamAsset : OfType<WebCamService> {
//    private RawImage          rawImage;
//    private AspectRatioFitter aspectRatioFitter;
//    private int               lastVerticalMirror, lastRotationAngle = 1;
//    private bool              ready;
//
//    /// <a href="http://bit.ly/2RbzvKP">Reference to the matching hardware driver</a>
//    public WebCamService Device { get => Value; private set => Value = value; }
//
//    /// <a href="http://bit.ly/2RbzvKP">The camera can be considered ready when it has done the first update</a>
//    public bool Ready => ready || (ready = Device.DidUpdateThisFrame);
//
//    /// <a href="http://bit.ly/2RbzvKP">Fetching the hardware driver</a> <inheritdoc />
//    protected override void Initialise() => Device = WebCamService.Instance;
//
//    /// <a href="http://bit.ly/2RbzvKP">Given a canvas, project the camera image on it</a>
//    public void Project(UnityEngine.GameObject background) {
//      rawImage         = background.GetComponent<RawImage>() ?? background.AddComponent<RawImage>();
//      rawImage.texture = Device.Texture;
//
//      aspectRatioFitter = background.GetComponent<AspectRatioFitter>() ??
//                          background.AddComponent<AspectRatioFitter>();
//
//      aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
//
//      Device.Playing = true;
//    }
//
//    /// <a href="http://bit.ly/2RbzvKP">When the phone is rotated the image can rotate to match. This changes the aspect ratio. Call this method occasionally to make the changes.</a>
//    public void CorrectForDeviceScreenOrientation() {
//      aspectRatioFitter.aspectRatio = Device.AspectRatio;
//
//      int verticalMirror = Device.VerticalMirror ? -1 : 1;
//
//      if (verticalMirror != lastVerticalMirror) {
//        rawImage.rectTransform.localScale = new Vector3(x: 1, y: verticalMirror, z: 1);
//        lastVerticalMirror                = verticalMirror;
//      }
//
//      int rotationAngle = Device.RotationAngle;
//
//      if (rotationAngle != lastRotationAngle) {
//        rawImage.rectTransform.localEulerAngles = new Vector3(x: 0, y: 0, z: -rotationAngle);
//        lastRotationAngle                       = rotationAngle;
//      }
//    }
//  }
//}
