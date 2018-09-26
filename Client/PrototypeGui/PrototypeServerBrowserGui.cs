#if CLIENT

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Cube.Networking.MasterServer {
    public class PrototypeServerBrowserGui : MonoBehaviour {
        /// <summary>
        /// first value (string): host name
        /// second value (ushort): port
        /// </summary>
        [Serializable]
        public class OnClickConnectEvent : UnityEvent<string, ushort> { }

        [ReadOnly]
        [SerializeField]
        OnClickConnectEvent _onClickConnect = new OnClickConnectEvent();
        public OnClickConnectEvent onClickConnect {
            get { return _onClickConnect; }
        }

        public Backend backend;

        #region Prefabs
        public GameObject columnPrefab;
        public GameObject textPrefab;
        public GameObject buttonPrefab;
        #endregion
        
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
            TriggerRefresh();
        }
        
        void TriggerRefresh() {
            StartCoroutine(Refresh());
        }

        IEnumerator Refresh() {
            _refreshButton.interactable = false;

            foreach (GameObject tmp in _columns)
                Destroy(tmp);
            _columns.Clear();

            yield return backend.RefreshServerList();

            AddNewColumns(5);

            if (backend.serverList != null) {
                for (int i = 0; i < backend.serverList.Count; i++) {
                    var details = backend.serverList[i];

                    var title = Instantiate(textPrefab, GetColumnTransform(0));
                    title.GetComponent<Text>().text = details.title;

                    var address = Instantiate(textPrefab, GetColumnTransform(1));
                    address.GetComponent<Text>().text = details.address + ":" + 60000;

                    var version = Instantiate(textPrefab, GetColumnTransform(2));
                    version.GetComponent<Text>().text = details.version;

                    var players = Instantiate(textPrefab, GetColumnTransform(3));
                    players.GetComponent<Text>().text = details.players + "/" + details.maxPlayers;

                    var connectButton = Instantiate(buttonPrefab, GetColumnTransform(4));
                    connectButton.GetComponentInChildren<Text>().text = "Connect";

                    connectButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
                        _onClickConnect.Invoke(details.address, 60000);
                    });
                }
            }

            _refreshButton.interactable = true;
        }

        void AddNewColumns(int count) {
            for (int i = 0; i < count; i++) {
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
