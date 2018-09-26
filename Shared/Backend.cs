using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cube.Networking.MasterServer {
    public abstract class Backend : ScriptableObject {
        public abstract List<ServerDetails> serverList {
            get;
        }

        public abstract IEnumerator RefreshServerList();
        public abstract IEnumerator UpdateServerDetails(ServerDetails details);
    }
}