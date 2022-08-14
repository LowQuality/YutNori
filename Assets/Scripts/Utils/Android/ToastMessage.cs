using UnityEngine;

namespace Utils.Android
{
public static class ToastMessage
{
    public static void Send(string message)
    {
        var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        var unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        if (unityActivity == null) return;
        var toastClass = new AndroidJavaClass("android.widget.Toast");
        unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0).Call("show");
        }));
    }
}
}