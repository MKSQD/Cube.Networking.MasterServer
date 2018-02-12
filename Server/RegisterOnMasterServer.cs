#if SERVER

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cube.Networking.MasterServer;

/// <summary>
/// Pushes the server details every n seconds to MasterServer
/// </summary>
public class RegisterOnMasterServer : MonoBehaviour {

    public int updateRateSeconds = 30;
    public string masterServerHost;

    public ServerDetails details;

    void Start () {
        StartCoroutine(SendInfosToMasterServer());
    }

    IEnumerator SendInfosToMasterServer() {
        while(true) {
            if (masterServerHost.Length == 0) {
                Log.Error("MasterServer host not set in ServerBrowser.");
                break;
            }

            if (details.address.Length == 0) {
                using (var webRequest = UnityWebRequest.Get("https://api.ipify.org")) {
                    yield return webRequest.SendWebRequest();

                    if (webRequest.isNetworkError || webRequest.isHttpError) {
                        Log.Error(webRequest.error);
                        continue;
                    }

                    details.address = webRequest.downloadHandler.text;
                    Debug.Log("Your server address (https://api.ipify.org): " + details.address);
                }
            }
            
            StartCoroutine(MasterServerNetworkInterface.UpdateServerDetails(masterServerHost, details));

            yield return new WaitForSeconds(updateRateSeconds);
        }
    }
}

#endif
