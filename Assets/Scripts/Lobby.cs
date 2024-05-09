using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public static class Lobby
{
    public const ulong PLAYER_ONE = NetworkManager.ServerClientId;//aka host
    public static ulong PLAYER_TWO;
    public static ulong PLAYER_THREE;
}
