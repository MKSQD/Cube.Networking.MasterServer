#if SERVER

using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Cube.Networking.MasterServer;
using Cube;

/// <summary>
/// Pushes the server details every n seconds to MasterServer
/// </summary>
public class RegisterOnMasterServer : MonoBehaviour
{
    public Backend backend;

    public int updateRateSeconds = 30;

    public ServerDetails details;

    void Start()
    {
        StartCoroutine(SendInfosToMasterServer());
    }

    IEnumerator SendInfosToMasterServer()
    {
        while (true) {
            if (details.address.Length == 0) {
                using (var webRequest = UnityWebRequest.Get("https://api.ipify.org")) {
                    yield return webRequest.SendWebRequest();

                    if (webRequest.isNetworkError || webRequest.isHttpError) {
                        Debug.LogError(webRequest.error);
                        continue;
                    }

                    details.address = webRequest.downloadHandler.text;
                    Debug.Log("Your server address (https://api.ipify.org): " + details.address);
                }
            }

            StartCoroutine(backend.UpdateServerDetails(details));

            yield return new WaitForSeconds(updateRateSeconds);
        }
    }
}

#endif
