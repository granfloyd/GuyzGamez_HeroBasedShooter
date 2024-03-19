using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class NetworkManagerUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject self;
    public Button startServerButton;
    public Button joinServerButton;//join client
    public string sceneName = "TestingMap";
    NetworkManager networkManager;
    
    void Start()
    {
        networkManager = NetworkManager.Singleton;
        startServerButton.onClick.AddListener(Host);
        joinServerButton.onClick.AddListener(Join);
    }
    public void Host()
    {
        networkManager.StartHost();
        self.SetActive(false);
        //SceneManager.LoadScene(sceneName);

    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            self.SetActive(true);
        }
    }
    public void Join()
    {
        networkManager.StartClient();
        self.SetActive(false);
        //SceneManager.LoadScene(sceneName);

    }
}
