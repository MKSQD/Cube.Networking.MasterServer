using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Networking.MasterServer {

    /// <summary>
    /// Component to specify settings and handles creation of the server browser gui.
    /// </summary>
    public class ServerBrowser : MonoBehaviour {

        /// <summary>
        /// first value (string): host name
        /// second value (ushort): port
        /// </summary>
        [Serializable]
        public class OnClickConnectEvent : UnityEvent<string, ushort> { }

        /// <summary>Parent of server browser gui.</summary>
        public GameObject guiParent;

        public string masterServerHostName;
        public OnClickConnectEvent onClickConnect;

        void Awake() {
            if (guiParent == null) {
                Log.Error("Reference to canvas not set.");
                return;
            }
            
            if (masterServerHostName.Length == 0) {
                Log.Error("MasterServer host name not set.");
                return;
            }

            var gui = Instantiate(Prefabs.ServerBrowserGUI, guiParent.transform);
            var serverBrowserGUI = gui.GetComponent<ServerBrowserGUI>();
            serverBrowserGUI.masterServer = new MasterServerNetworkInterface(masterServerHostName);
            serverBrowserGUI.onClickConnect = onClickConnect;
        }

    }

}
