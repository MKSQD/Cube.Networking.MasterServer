using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Cube.Networking.MasterServer {
    [CreateAssetMenu(menuName = "Cube.Networking/MasterServer/CubeBackend")]
    public class CubeBackend : Backend {
        // Helper struct for de/-serializing.
        struct ServerList {
            public int count;
            public List<ServerDetails> server;
        }

        public string host;

        List<ServerDetails> _serverList;
        public override List<ServerDetails> serverList {
            get { return _serverList; }
        }

        public override IEnumerator RefreshServerList() {
            var webRequest = UnityWebRequest.Get(host + "/api/v1/server/query");
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError) {
                Debug.LogError(webRequest.error);
            }
            else {
                var serializationHelper = JsonUtility.FromJson<ServerList>(webRequest.downloadHandler.text);
                _serverList = serializationHelper.server;
            }
        }

        public override IEnumerator UpdateServerDetails(ServerDetails details) {
            var json = JsonUtility.ToJson(details);

            using (var webRequest = UnityWebRequest.Put(host + "/api/v1/server/put", json)) {
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError || webRequest.isHttpError)
                    Debug.LogError(webRequest.error);
            }
        }
    }
}