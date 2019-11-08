using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;
using SSRPCs;

[RPCClass]
public class ExampleServer : MonoBehaviour
{    
    public enum GameState
    {
        pregame,
        maingame,
        postgame
    }
    public static GameState gameState = GameState.pregame;

    public static ExampleServer instance;

    public ServerNetwork serverNet;

    public int portNumber = 603;
    
    // Stores a player
    class Player
    {
        public long clientId;
        public string playerName;
        public bool isReady;
        public bool isConnected;
    }
    List<Player> players = new List<Player>();
    int currentActivePlayer;

    // Use this for initialization
    void Awake()
    {
        instance = this;

        // Initialization of the server network
        ServerNetwork.port = portNumber;
        if (serverNet == null)
        {
            serverNet = GetComponent<ServerNetwork>();
        }
        if (serverNet == null)
        {
            serverNet = (ServerNetwork)gameObject.AddComponent(typeof(ServerNetwork));
            Debug.Log("ServerNetwork component added.");
        }

        //serverNet.EnableLogging("rpcLog.txt");
    }

    // A client has just requested to connect to the server
    void ConnectionRequest(ServerNetwork.ConnectionRequestInfo data)
    {
        Debug.Log("Connection request from " + data.username);

        // We either need to approve a connection or deny it
        if (gameState == GameState.pregame)
        {
            Player newPlayer = new Player();
            newPlayer.clientId = data.id;
            newPlayer.playerName = data.username;
            newPlayer.isConnected = false;
            newPlayer.isReady = false;
            players.Add(newPlayer);

            serverNet.ConnectionApproved(data.id);
        }
        else
        {
            serverNet.ConnectionDenied(data.id);
        }
    }

    void OnClientConnected(long aClientId)
    {
        // Set the isConnected to true on the player
        foreach (Player p in players)
        {
            if (p.clientId == aClientId)
            {
                p.isConnected = true;
            }
        }
    }

    [RPCMethod]
    public void PlayerIsReady()
    {
        // Who called this RPC: serverNet.SendingClientId
        Debug.Log("Player is ready");

        // Set the isConnected to true on the player
        foreach (Player p in players)
        {
            if (p.clientId == serverNet.SendingClientId)
            {
                p.isReady = true;
            }
        }

        // Are all of the players ready?
        bool allPlayersReady = true;
        foreach (Player p in players)
        {
            if (!p.isReady)
            {
                allPlayersReady = false;
            }
        }
        if (allPlayersReady)
        {
            gameState = GameState.maingame;
        }
    }

    [RPCMethod]
    public void HiFive(int id)
    {
        serverNet.CallRPC("ReceiveHiFive", serverNet.SendingClientId, id);
    }

    void OnClientDisconnected(long aClientId)
    {
        // Set the isConnected to true on the player
        foreach (Player p in players)
        {
            if (p.clientId == aClientId)
            {
                players.Remove(p);
            }
        }
    }

    private void Update()
    {
        foreach(var player in serverNet.GetAllObjects())
        {

        }
    }

    [RPCMethod]
    public void SendChat(bool all, string message)
    {
        if(all)
        {
            serverNet.CallRPC("ReceiveChat", serverNet.SendingClientId, message);
        }
        else
        {

        }
    }
}
