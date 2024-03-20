using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUI : NetworkBehaviour
{
    public Button startServerButton;
    public Button startClientButton;
    public PlayerController pc;
    // Start is called before the first frame update
    void Start()
    {
       startServerButton.onClick.AddListener(Host);
       startClientButton.onClick.AddListener(Client);
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }
    void Host()
    {
        NetworkManager.Singleton.StartHost();
        RemoveFromScreen();
    }
    void Client()
    {
        NetworkManager.Singleton.StartClient();
        RemoveFromScreen();
    }

    private void RemoveFromScreen()
    {
        startServerButton.gameObject.SetActive(false);
        startClientButton.gameObject.SetActive(false);
    }
    void OnClientConnected(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            SetPlayerController();
        }
    }
    public void SetPlayerController()
    {
        if (NetworkManager.LocalClient == null || NetworkManager.LocalClient.PlayerObject == null)
        {
            Debug.LogError("PlayerObject is not set yet.");
            return;
        }

        if (HeroSelectUI.Instance == null)
        {
            Debug.LogError("HeroSelectUI instance is not set yet.");
            return;
        }

        pc = NetworkManager.LocalClient.PlayerObject.GetComponent<PlayerController>();
        HeroSelectUI.Instance.playerController = pc;
    }
}
