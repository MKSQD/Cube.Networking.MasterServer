# README #

### Setup ###

Checkout or download Core into your Unity Assets folder. All the modules should reside directly in your Assets folder, not in a subfolder.

**Client**
* Instantiate Client/GUI/ServerBrowserGui.prefab
* Initialize ServerBrowser. E.g:
var serverBrowserGui = Instantiate(serverBrowserGuiPrefab, MyCanvas.instance.transform);

var component = serverBrowserGui.GetComponent<ServerBrowserGui>();
component.onClickConnect.AddListener(ConnectToServer);
component.Initialize("http://127.0.0.1:23888");

...

void ConnectToServer(string host, ushort port)
{
}

**Server**
* Add the "RegisterOnMasterServer" Component to a new or existing gameobject  This Component will send ServerDetails every N seconds ("UpdateRateSeconds") to the MasterServer.
* Set "MasterServerHost" value (e.g. http://127.0.0.1:23888)
* Change the ServerDetails in the inspector or in code

To Setup the MasterServer itself please refere to https://bitbucket.org/unique-code/masterserver


### TODO ###

#TODO
