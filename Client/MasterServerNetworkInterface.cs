#if CLIENT

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Cube.Networking.MasterServer {
    /// <summary>
    /// Handles communication between client and MasterServer.
    /// </summary>
    public class MasterServerNetworkInterface {
        /// <summary>
        /// Helper struct for de/-serializing.
        /// </summary>
        struct ServerList {
            public int count;
            public List<ServerDetails> server;
        }

        string _masterServerHost;

        List<ServerDetails> _serverList;
        public List<ServerDetails> serverList {
            get { return _serverList; }
        }

        public MasterServerNetworkInterface(string masterServerHost) {
            _masterServerHost = masterServerHost;
        }

        /// <summary>
        /// Try to load server list.
        /// </summary>
        /// <example>
        ///     IEnumerator Refresh() {
        ///         ...
        ///         yield return masterServer.RefreshServerList();
        ///         var serverList = masterServer.serverList;
        ///         ...
        ///     }
        /// </example>
        public IEnumerator RefreshServerList() {
            var webRequest = UnityWebRequest.Get(_masterServerHost + "/api/v1/server/query");
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError) {
                Log.Error(webRequest.error);
            } else {
                var serializationHelper = JsonUtility.FromJson<ServerList>(webRequest.downloadHandler.text);
                _serverList = serializationHelper.server;
            }
        }

        /// <summary>
        /// Pushes the server details to MasterServer
        /// </summary>
        /// <example>
        ///     //1
        ///     StartCoroutine(MasterServerNetworkInterface.UpdateServerDetails(masterServerHost, details));
        ///     
        ///     //2
        ///     IEnumerator UpdateDetails() {
        ///         ...
        ///         yield return MasterServerNetworkInterface.UpdateServerDetails(masterServerHost, details);
        ///         ...
        ///     }
        /// </example>
        public static IEnumerator UpdateServerDetails(string masterServerHost, ServerDetails details) {
            var json = JsonUtility.ToJson(details);
            using (var webRequest = UnityWebRequest.Put(masterServerHost + "/api/v1/server/put", json)) {
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError || webRequest.isHttpError)
                    Log.Error(webRequest.error);
            }
        }
    }
}

#endif
