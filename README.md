# README #

### Setup ###

Checkout or download Core into your Unity Assets folder. All the modules should reside directly in your Assets folder, not in a subfolder.

**Client**
* Add the "ServerBrowser" Component to a new or existing gameobject 
* Set "GUI Parent" to your Canvas or some gameobject inside a Canvas (ServerBrowser GUI will be create as child of this gameobject).
* Set "MasterServerHost" value (e.g. http://127.0.0.1:23888)
* Set the OnClickConnect Event



**Server**
* Add the "RegisterOnMasterServer" Component to a new or existing gameobject  This Component will send ServerDetails every N seconds ("UpdateRateSeconds") to the MasterServer.
* Set "MasterServerHost" value (e.g. http://127.0.0.1:23888)
* Change the ServerDetails in the inspector or in code

To Setup the MasterServer itself please refere to https://bitbucket.org/unique-code/masterserver


### TODO ###

#TODO
