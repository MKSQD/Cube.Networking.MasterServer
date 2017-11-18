#if CLIENT

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Core.Networking.MasterServer {
    /// <summary>
    /// Handles communication between client and MasterServer.
    /// </summary>
    public class MasterServerNetworkInterface {
        /// <summary>
        /// Helper struct for serializing.
        /// </summary>
        struct ServerList {
            public int count;
            public List<ServerDetails> server;
        }

        string _masterServerHost;
        UnityWebRequestAsyncOperation _lastRequest;

        List<ServerDetails> _serverList;
        public List<ServerDetails> serverList {
            get { return _serverList; }
        }

        bool _isDone = true;
        public bool isDone {
            get { return _isDone; }
        }

        public MasterServerNetworkInterface(string masterServerHost) {
            _masterServerHost = masterServerHost;
        }

        /// <summary>
        /// Try to load server list.
        /// </summary>
        /// <example>
        ///     //Blocking
        ///     IEnumerator Refresh() {
        ///         ...
        ///         masterServer.RequsetServerListAsync();
        ///             while (!masterServer.isDone)
        ///         yield return null;
        ///         ...
        ///     }
        ///     
        ///     //Nonblocking
        ///     ...
        ///     masterServer.RequsetServerListAsync().completed += OnServerListLoaded;
        ///     ...
        /// </example>
        /// <returns>
        /// Pending AsyncOperation for last request or new AsyncOperation
        /// </returns>
        public UnityWebRequestAsyncOperation RequsetServerListAsync() {
            if (!_isDone)
                return _lastRequest;
            _isDone = false;

            UnityWebRequest webRequest = UnityWebRequest.Get(_masterServerHost + "/api/v1/server/query");
            _lastRequest = webRequest.SendWebRequest();
            _lastRequest.completed += RequestCompleted;

            return _lastRequest;
        }

        void RequestCompleted(AsyncOperation asyncOperation) {
            var webOperation = (UnityWebRequestAsyncOperation)asyncOperation;

            if (webOperation.webRequest.isNetworkError || webOperation.webRequest.isHttpError) {
                Debug.LogError(webOperation.webRequest.error);
            } else {
                var serializationHelper = JsonUtility.FromJson<ServerList>(webOperation.webRequest.downloadHandler.text);
                _serverList = serializationHelper.server;
            }

            _isDone = true;
        }
    }
}

#endif
