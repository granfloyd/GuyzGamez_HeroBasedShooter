using System.Net;
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Relay.Models;
using TMPro;
using Unity.Networking.Transport.Relay;
using UnityEngine.SceneManagement;

public class NetworkUI : MonoBehaviour
{ 
    public GameObject hostClientUI;
    public GameObject hostPickMapUI;//map select
    public GameObject clientEnterCodeUI;//enter code to joni host

    public Button serverButton;
    public Button clientButton;
    public Button backButton;

    public Button mapButton;

    public TMP_Text inputFieldText;
    public Button inputFieldTextButton;

    public bool isHost = false;
    public bool isClient = false;
    public string map1 = "TestingMap";

    private async void Start()
    {
        await UnityServices.InitializeAsync();//pasuses here until reply

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        serverButton.onClick.AddListener(SelectHost);
        clientButton.onClick.AddListener(SelectClient);
        backButton.onClick.AddListener(RemoveOrAdd);

        mapButton.onClick.AddListener(JoinMap);//host now makes game 
        inputFieldTextButton.onClick.AddListener(JoinGame);//trys to connect client to game

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }
    private async void CreateRelay()
    {
        try
        {
            //parm is how many clients not including host so max is 4 players
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log("Join code: " + joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();
            SceneManager.LoadScene(map1, LoadSceneMode.Single);
        }
        catch (RelayServiceException e)
        {
            Debug.LogError(e);
        }

    }

    private async void JoinRelay(string joinCode)
    {
        try
        {
            
            joinCode = inputFieldText.text;
            joinCode = joinCode.Substring(0, 6);
            Debug.Log("Joining relay with code: " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
            SceneManager.LoadScene(map1, LoadSceneMode.Single);
        }
        catch (RelayServiceException e)
        {
            Debug.LogError(e);
        }
    }
    void SelectHost()
    {
        isHost = true;
        RemoveOrAdd();
    }
    void SelectClient()
    {
        isClient = true;
        RemoveOrAdd();
    }
    private void RemoveOrAdd()
    {
        if(!backButton.gameObject.activeSelf)
        {
            if (isHost)
            {
                hostClientUI.SetActive(false);
                hostPickMapUI.SetActive(true);
            }
            else if (isClient)
            {
                hostClientUI.SetActive(false);
                clientEnterCodeUI.SetActive(true);
            }
            backButton.gameObject.SetActive(true);
        }
        else
        {
            backButton.gameObject.SetActive(false);
            hostClientUI.SetActive(true);
        }

    }

    private void JoinMap()
    {
        CreateRelay();        
    }
    private void JoinGame()
    {
        string joinCode = inputFieldText.text;
        JoinRelay(joinCode);
        
    }

    void OnClientConnected(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            //SetPlayerController();
        }
    }
}
