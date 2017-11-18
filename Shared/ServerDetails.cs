using System;

[Serializable]
public struct ServerDetails {
    public string address;
    public ushort port;

    public string version;

    public ushort players;
    public ushort maxPlayers;

    public string title;
}
