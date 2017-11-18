#if CLIENT

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Core.Networking.MasterServer {

    public class ServerBrowserGUI : MonoBehaviour {

#region Prefabs
        public GameObject columnPrefab;
        public GameObject textPrefab;
        public GameObject buttonPrefab;
#endregion

        public MasterServerNetworkInterface masterServer;
        public ServerBrowser.OnClickConnectEvent onClickConnect;

        [SerializeField]
        ScrollRect _scrollView;

        [SerializeField]
        Button _refreshButton;

        List<GameObject> _columns;

        void Awake() {
            _refreshButton.onClick.AddListener(() => {
                StartCoroutine(Refresh());
            });

            _columns = new List<GameObject>();
        }

        void Start() {
            StartCoroutine(Refresh());
        }

        IEnumerator Refresh() {
            _refreshButton.interactable = false;
            
            foreach (GameObject tmp in _columns)
                Destroy(tmp);
            _columns.Clear();

            masterServer.RequsetServerListAsync();
            while (!masterServer.isDone)
                yield return null;

            AddNewColumns(5);

            if (masterServer.serverList != null) {
                for (int i = 0; i < masterServer.serverList.Count; i++) {
                    var details = masterServer.serverList[i];

                    var title = Instantiate(textPrefab, GetColumnTransform(0));
                    title.GetComponent<Text>().text = details.title;

                    var address = Instantiate(textPrefab, GetColumnTransform(1));
                    address.GetComponent<Text>().text = details.address + ":" + details.port;

                    var version = Instantiate(textPrefab, GetColumnTransform(2));
                    version.GetComponent<Text>().text = details.version;

                    var players = Instantiate(textPrefab, GetColumnTransform(3));
                    players.GetComponent<Text>().text = details.players + "/" + details.maxPlayers;

                    var connectButton = Instantiate(buttonPrefab, GetColumnTransform(4));
                    connectButton.GetComponentInChildren<Text>().text = "Connect";

                    connectButton.GetComponent<Button>().onClick.AddListener(() => {
                        onClickConnect.Invoke(details.address, details.port);
                    });
                }
            }

            _refreshButton.interactable = true;
        }

        void AddNewColumns(int count) {
            for(int i = 0; i < count; i++) {
                var column = Instantiate(columnPrefab, _scrollView.content.transform);
                column.name = "column_" + _columns.Count;
                _columns.Add(column);
            }
        }

        Transform GetColumnTransform(int index) {
            return _columns[index].transform;
        }
    }
}

#endif
