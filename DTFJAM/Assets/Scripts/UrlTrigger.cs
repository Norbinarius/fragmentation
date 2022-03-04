using UnityEngine;
using System.Runtime.InteropServices;

public class UrlTrigger : MonoBehaviour
{
    [SerializeField] private ShipController ship;
    [SerializeField] private string url;
    private bool isActivated;

    [DllImport("__Internal")]
    private static extern void OpenNewTab(string url);

    private void OnTriggerStay2D(Collider2D col)
    {
        if(col.tag == Constants.Tags.PLAYER && ship.Rb.velocity.magnitude < 0.3f && !isActivated)
        {
            OpenLinkJSPlugin(url);
            isActivated = true;
        }
    }

    private void OpenLinkJSPlugin(string url)
    {
		#if UNITY_WEBGL
             OpenNewTab(url);
        #elif !UNITY_EDITOR
            Application.OpenURL(url);
        #endif

	}
}
