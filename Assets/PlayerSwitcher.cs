using UnityEngine;
using UnityEngine.XR;

public class PlayerSwitcher : MonoBehaviour
{
    public GameObject fpsPlayer;
    public GameObject xrOrigin;

    void Start()
    {
        bool vrActive = XRSettings.isDeviceActive;

        if (vrActive)
        {
            xrOrigin.SetActive(true);
            fpsPlayer.SetActive(false);
        }
        else
        {
            xrOrigin.SetActive(false);
            fpsPlayer.SetActive(true);
        }
    }
}