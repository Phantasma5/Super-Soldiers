using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleScreenLogic : MonoBehaviour {

    public Text server;
    public Text port;
    public ExampleClient client;
    public GameObject lobbyCanvas;

    private void Awake()
    {
        //client.loginScreen.SetActive(true);
        //lobbyCanvas.SetActive(false);
    }

    private void Start()
    {        
    }

    public void Connect()
    {
        client.ConnectToServer(server.text, int.Parse(port.text));
    }

    private void Update()
    {
        //if (client.clientNet.IsConnected())
        //{
        //    client.loginScreen.SetActive(false);
        //}
        //else
        //{
        //    client.loginScreen.SetActive(true);
        //}
    }
}
