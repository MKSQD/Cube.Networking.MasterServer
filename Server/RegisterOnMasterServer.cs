#if SERVER

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Core.Networking.Server;

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

            StartCoroutine(PutInfos());

            yield return new WaitForSeconds(updateRateSeconds);
        }
    }

    IEnumerator PutInfos() {
        var json = JsonUtility.ToJson(details);
        using (UnityWebRequest www = UnityWebRequest.Put(masterServerHost + "/api/v1/server/put", json)) {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
                Log.Error(www.error);
        }
    }
}

#endif
