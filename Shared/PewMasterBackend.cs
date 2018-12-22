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
        string _host = "http://pixelsiege.net/master";

        [NonSerialized]
        int id = 0;

        List<ServerDetails> _serverList = new List<ServerDetails>();
        public override List<ServerDetails> serverList {
            get { return _serverList; }
        }

        public override IEnumerator RefreshServerList() {
            var webRequest = UnityWebRequest.Get(_host + "/query.php?game=" + game);
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError) {
                Debug.LogError(webRequest.error);
            } else {
                _serverList.Clear();

                var text = webRequest.downloadHandler.text;

                var lines = text.Split('\n');

                if(lines.Length > 0) {
                    var numResults = int.Parse(lines[0]);

                    for (int i = 0; i < numResults; ++i) {

                        var tk = lines[i + 1].Split('|');
                        if (tk.Length < 2) {
                            Debug.LogError("Not enough tokens");
                            continue;
                        }

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
                var url = _host + "/update.php?action=create&game=" + game + "&hostname=" + details.title + "&ip=" + details.address;
                using (var webRequest = UnityWebRequest.Get(url)) {
                    yield return webRequest.SendWebRequest();

                    if (webRequest.isNetworkError || webRequest.isHttpError)
                        Debug.LogError(webRequest.error);

                    id = int.Parse(webRequest.downloadHandler.text);
                }
            } else {
                using (var webRequest = UnityWebRequest.Get(_host + "/update.php?action=update&id=" + id)) {
                    yield return webRequest.SendWebRequest();

                    if (webRequest.isNetworkError || webRequest.isHttpError)
                        Debug.LogError(webRequest.error);
                }
            }
        }
    }
}