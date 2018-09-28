using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Cube.Networking.MasterServer {
    /// <summary>
    /// Backend for http://pixelsiege.net/master/
    /// </summary>
    [CreateAssetMenu(menuName = "Cube.Networking/MasterServer/PewMasterBackend")]
    public class PewMasterBackend : Backend {
        public string game;
        public string host = "http://pixelsiege.net/master";
        
        [NonSerialized]
        int id = 0;

        List<ServerDetails> _serverList = new List<ServerDetails>();
        public override List<ServerDetails> serverList {
            get { return _serverList; }
        }

        public override IEnumerator RefreshServerList() {
            var webRequest = UnityWebRequest.Get(host + "/query.php?game=" + game);
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError) {
                Debug.LogError(webRequest.error);
            }
            else {
                _serverList.Clear();

                var text = webRequest.downloadHandler.text;

                var end = text.IndexOf('\n');
                if (end == -1) {
                    end = text.Length;
                }

                var numResults = int.Parse(text.Substring(0, end));

                if (end != -1) {
                    text = text.Substring(end + 1);

                    for (int i = 0; i < numResults; ++i) {
                        end = text.IndexOf('\n');

                        string serverDesc;
                        if (end != -1) {
                            serverDesc = text.Substring(0, end);

                            text = text.Substring(end);
                        }
                        else {
                            serverDesc = text;
                        }
                        
                        var tk = serverDesc.Split('|');

                        var details = new ServerDetails {
                            title = tk[0],
                            address = tk[1]
                        };
                        _serverList.Add(details);
                    }
                }
            }
        }

        public override IEnumerator UpdateServerDetails(ServerDetails details) {
            if (id == 0) {
                using (var webRequest = UnityWebRequest.Get(host + "/update.php?action=create&game=" + game + "&hostname=" + details.title + "&ip=" + details.address)) {
                    yield return webRequest.SendWebRequest();

                    if (webRequest.isNetworkError || webRequest.isHttpError)
                        Debug.LogError(webRequest.error);

                    id = int.Parse(webRequest.downloadHandler.text);
                }
            }
            else {
                using (var webRequest = UnityWebRequest.Get(host + "/update.php?action=update&id=" + id)) {
                    yield return webRequest.SendWebRequest();

                    if (webRequest.isNetworkError || webRequest.isHttpError)
                        Debug.LogError(webRequest.error);
                }
            }
        }
    }
}