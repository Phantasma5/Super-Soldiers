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
        client.loginScreen.SetActive(true);
        lobbyCanvas.SetActive(false);
    }

    private void Start()
    {
        if(client.clientNet.IsConnected())
        {
            client.loginScreen.SetActive(false);
            lobbyCanvas.SetActive(true);
        }
    }

    public void Connect()
    {
        client.ConnectToServer(server.text, int.Parse(port.text));
    }
}
