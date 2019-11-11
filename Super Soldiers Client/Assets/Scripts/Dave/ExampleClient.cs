using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SSRPCs;

[RPCClass]
public class ExampleClient : MonoBehaviour
{
    public ClientNetwork clientNet;
    float timeToSend = 5.0f;

    public string pregame;
    public string maingame;

    // Get the instance of the client
    static ExampleClient instance = null;
    
    // Are we in the process of logging into a server
    private bool loginInProcess = false;

    public GameObject loginScreen;

    public GameObject lobbyScreen;

    public GameObject myPlayer;

    public int weapon = 0;

    // Singleton support
    public static ExampleClient GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("ExampleClient is uninitialized");
            return null;
        }
        return instance;
    }

    // Use this for initialization
    void Awake()
    {
        // Make sure we have a ClientNetwork to use
        if (clientNet == null)
        {
            clientNet = GetComponent<ClientNetwork>();
        }
        if (clientNet == null)
        {
            clientNet = (ClientNetwork)gameObject.AddComponent(typeof(ClientNetwork));
        }
    }
    
    // Start the process to login to a server
    public void ConnectToServer(string aServerAddress, int aPort)
    {
        if (loginInProcess)
        {
            return;
        }
        loginInProcess = true;
        
        ClientNetwork.port = aPort;
        clientNet.Connect(aServerAddress, ClientNetwork.port, "", "", "", 0);
    }


    public void UpdateState(int x, int y, string player)
    {
        // Update the visuals for the game
    }

    [RPCMethod]
    public void TestRPC(int aInt)
    {
        Debug.Log("RPC Test has been called with " + aInt.ToString());
    }

    public void NewClientConnected(long aClientId, string aValue)
    {
        Debug.Log("RPC NewClientConnected has been called with " + aClientId + " " + aValue);
    }

    // Networking callbacks
    // These are all the callbacks from the ClientNetwork
    void OnNetStatusNone()
    {
        Debug.Log("OnNetStatusNone called");
    }
    void OnNetStatusInitiatedConnect()
    {
        Debug.Log("OnNetStatusInitiatedConnect called");
    }
    void OnNetStatusReceivedInitiation()
    {
        Debug.Log("OnNetStatusReceivedInitiation called");
    }
    void OnNetStatusRespondedAwaitingApproval()
    {
        Debug.Log("OnNetStatusRespondedAwaitingApproval called");
    }
    void OnNetStatusRespondedConnect()
    {
        Debug.Log("OnNetStatusRespondedConnect called");
    }
    void OnNetStatusConnected()
    {
        loginScreen.SetActive(false);
        lobbyScreen.SetActive(true);
        Debug.Log("OnNetStatusConnected called");

        clientNet.AddToArea(1);
    }

    void OnNetStatusDisconnecting()
    {
        Debug.Log("OnNetStatusDisconnecting called");

        if (myPlayer)
        {
            clientNet.Destroy(myPlayer.GetComponent<NetworkSync>().GetId());
        }
    }
    void OnNetStatusDisconnected()
    {
        Debug.Log("OnNetStatusDisconnected called");
        SceneManager.LoadScene(pregame);
        
        loginInProcess = false;

        if (myPlayer)
        {
            clientNet.Destroy(myPlayer.GetComponent<NetworkSync>().GetId());
        }
    }
    public void OnChangeArea()
    {
        Debug.Log("OnChangeArea called");

        // Tell the server we are ready
        myPlayer = clientNet.Instantiate("Player", new Vector3(Random.Range(-10,10), 0, 0), Quaternion.identity);
        myPlayer.GetComponent<NetworkSync>().AddToArea(1);
    }

    // RPC Called by the server once it has finished sending all area initization data for a new area
    public void AreaInitialized()
    {
        Debug.Log("AreaInitialized called");
    }
    
    void OnDestroy()
    {
        if (myPlayer)
        {
            clientNet.Destroy(myPlayer.GetComponent<NetworkSync>().GetId());
        }
        if (clientNet.IsConnected())
        {
            clientNet.Disconnect("Peace out");
        }
    }

    //SendChat(true, 0, "Testing Chat System"); 
    public void SendChat(bool global, int team, string message)
    {
        clientNet.CallRPC("Chat", UCNetwork.MessageReceiver.ServerOnly, -1, global, team, message);
    }

    [RPCMethod]
    public void ReceiveChat(bool global, string message)
    {
        string channel = "";
        if(global)
        {
            channel = "all";
        }
        else
        {
            channel = "team";
        }
        References.userInterface.UpdateChatLog(channel+": "+message);
    }

    [RPCMethod]
    public void TransitionToGame()
    {
        SceneManager.LoadScene(maingame);
        foreach(var p in GameObject.FindGameObjectsWithTag("Player"))
        {
            p.transform.position = new Vector3(
                p.transform.position.x,
                0,
                p.transform.position.z);
        }
    }

    [RPCMethod]
    public void TransitionToLobby()
    {
        SceneManager.LoadScene(pregame);
    }

    public void ReadyUpTeamGame()
    {
        clientNet.CallRPC("PlayerIsReady", UCNetwork.MessageReceiver.ServerOnly, -1, weapon, true);
    }

    public void ReadyUpFFA()
    {
        clientNet.CallRPC("PlayerIsReady", UCNetwork.MessageReceiver.ServerOnly, -1, weapon, false);
    }
    [RPCMethod]
    public void SetTeam(int team)
    {
        myPlayer.GetComponent<PlayerController>().team = team;
    }
}


