using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleScreenLogic : MonoBehaviour
{

    public Text server;
    public Text port;
    public ExampleClient client;
    public GameObject loginCanvas;

    private void Awake()
    {
        //client.loginScreen.SetActive(true);
        //lobbyCanvas.SetActive(false);
    }

    private void Start()
    {
        if (!ExampleClient.GetInstance().firstLaunch)
        {
            loginCanvas.SetActive(false);
        }
    }

    public void Connect()
    {
        client.ConnectToServer(server.text, int.Parse(port.text));
    }

    private void Update()
    {
        if (ExampleClient.GetInstance().uiState == ExampleClient.UIState.login)
        {
            if (!loginCanvas.activeInHierarchy)
                loginCanvas.SetActive(true);
        }
        else
        {
            if (loginCanvas.activeInHierarchy)
                loginCanvas.SetActive(false);
        }
    }
}
