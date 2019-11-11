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

    public bool teamGame = true;
    public int teams = 2;

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
        public int team;
        public float health;
        public int weapon;
        public bool teamVote;
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
    public void PlayerIsReady(int weapon, bool teamGame)
    {
        // Who called this RPC: serverNet.SendingClientId
        Debug.Log("Player is ready");

        // Set the isConnected to true on the player
        foreach (Player p in players)
        {
            if (p.clientId == serverNet.SendingClientId)
            {
                p.isReady = true;
                p.weapon = weapon;
                p.teamVote = teamGame;
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
        if (allPlayersReady && players.Count > 1)
        {
            RunGame();
        }
    }

    private void RunGame()
    {
        int teamVotes = 0;
        foreach(var p in players)
        {
            if(p.teamVote)
            {
                teamVotes++;
            }
        }
        if(teamVotes >= players.Count / 2)
        {
            teamGame = true;
            for(int i = 0; i < players.Count; i++)
            {
                players[i].team = i % 2;
                serverNet.CallRPC("SetTeam", players[i].clientId, -1, i % 2);
            }
        }
        else
        {
            teamGame = false;
            foreach(var p in players)
            {
                p.team = -1;
            }
            serverNet.CallRPC("SetTeam", UCNetwork.MessageReceiver.AllClients, -1, -1);
        }
        serverNet.CallRPC("TransitionToGame", UCNetwork.MessageReceiver.AllClients, -1);
        gameState = GameState.maingame;
    }

    private void EndGame()
    {
        serverNet.CallRPC("TransitionToLobby", UCNetwork.MessageReceiver.AllClients, -1);
        gameState = GameState.pregame;
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
        //if (gameState == GameState.maingame)
        //{
        //    if (teamGame)
        //    {
        //        int remainingTeams = 0;
        //        for (int i = 0; i < teams; i++)
        //        {
        //            if (LivePlayersOnTeam(i))
        //            {
        //                remainingTeams++;
        //            }
        //        }
        //        if (remainingTeams <= 1)
        //        {
        //            EndGame();
        //        }
        //    }
        //    else
        //    {
        //        if (!MultipleLivePlayers())
        //        {
        //            EndGame();
        //        }
        //    }
        //}
    }

    [RPCMethod]
    public void Chat(bool global, int team, string message)
    {
        if (global)
        {
            //foreach (Player p in players)
            //{
            //    serverNet.CallRPC("ReceiveChat", p.clientId, -1, global, message);
            //}
            serverNet.CallRPC("ReceiveChat", UCNetwork.MessageReceiver.AllClients, -1, global, message);
        }
        else
        {
            foreach (Player p in players)
            {
                if (p.team == team)
                {
                    serverNet.CallRPC("ReceiveChat", p.clientId, -1, global, message);
                }
            }
        }
    }

    public bool LivePlayersOnTeam(int team)
    {
        foreach(var p in players)
        {
            if(p.team == team && p.health > 0)
            {
                return true;
            }
        }
        return false;
    }

    public bool MultipleLivePlayers()
    {
        int alive = 0;
        foreach(var p in players)
        {
            if(p.health > 0)
            {
                alive++;
                if(alive > 1)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
