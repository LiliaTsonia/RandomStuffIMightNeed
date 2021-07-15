using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class UserLocation : MonoBehaviour
{
    private const string TOAST_CLASS_NAME = "android.widget.Toast";

    //private const string LOCATION_NOT_ENABLED = "User has not enabled location";
    private const string LOCATION_UNKNOWN = "Unable to determine device location";
    private const string LOCATION_SERVICE_UNAVAILABLE = "Service wasn`t initialized";
    private const string LOCATION_SERVICE_INITIALIZED = "Service started successfully";

    /// <summary>
    /// Test button to show user location
    /// Optional, can be removed
    /// </summary>
    [SerializeField] private Button _locationButton;

    private bool _serviceInitialized;

    public bool ServiceEnabled => Input.location.isEnabledByUser;

    private void Awake()
    {
        RequestUserPermission();
    }

    private void Start()
    {
        _locationButton.onClick.AddListener(FindLocation);
        StartCoroutine(StartLocationService());
    }

    private IEnumerator StartLocationService()
    {
        if (!ServiceEnabled)
        {
            RequestUserPermission();
            yield break;
        }

        Input.location.Start();

        while (Input.location.status == LocationServiceStatus.Initializing)
        {
            yield return new WaitForSeconds(1);
        }

        switch (Input.location.status)
        {
            case LocationServiceStatus.Failed:
                _serviceInitialized = false;
                ShowMessage(LOCATION_UNKNOWN);
                break;
            case LocationServiceStatus.Running:
                _serviceInitialized = true;
                ShowMessage(LOCATION_SERVICE_INITIALIZED, 0);
                yield break;
        }
    }

    private void RequestUserPermission()
    {
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }
#elif UNITY_IOS
    PlayerSettings.iOS.locationUsageDescription = "Details to use location";
#endif
    }

    private void FindLocation()
    {
        var message = _serviceInitialized ?
            "Latitude : " + Input.location.lastData.latitude
            + " Longitude : " + Input.location.lastData.longitude
            + " Altitude : " + Input.location.lastData.altitude
            : LOCATION_SERVICE_UNAVAILABLE;

        ShowMessage(message);
    }

    private void ShowMessage(string message, int duration = 1)
    {
        var toastClass = new AndroidJavaClass(TOAST_CLASS_NAME);
        var toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", GetUnityActivity(), message, duration);

        toastObject.Call("show");
    }

    private AndroidJavaObject GetUnityActivity()
    {
        var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        return unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus && Input.location.status != LocationServiceStatus.Initializing)
        {
            StartCoroutine(StartLocationService());
        }
        else
        {
            Input.location.Stop();
        }
    }
}
