using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : NetworkBehaviour
{
    public bool isducttape;
    public int maxHealth;
    public NetworkVariable<int> currentHealth = new NetworkVariable<int>(200);
    public bool isPlayer;
    public float totalWidth = 300f;
    public float blockHeight = 50;
    public float blockWidth;
    public int healthPerBlock = 25;
    public Transform healthBlockStart;
    public GameObject healthBlockPrefab;
    public List<GameObject> healthBlockList = new List<GameObject>();
    
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            InitializeHealth();
        }
        else
        {
            currentHealth.OnValueChanged += OnHealthChange;
            InitializeHealth();
        }
    }
    private void InitializeHealth()
    {
        currentHealth.Value = maxHealth; // Set the initial health value
        CreateHealthBar();
    }
    void CreateHealthBar()
    {
        int totalBlocks = maxHealth / healthPerBlock;
        blockWidth = totalWidth / totalBlocks;

        if(isPlayer)
        {
            for (int i = 0; i < totalBlocks; i++)
            {
                if (i == 0)
                {
                    Debug.Log("Spawning payer hp bar");
                    GameObject block = Instantiate(healthBlockPrefab, healthBlockStart);
                    block.GetComponent<RectTransform>().sizeDelta = new Vector2(blockWidth, blockHeight);
                    healthBlockList.Add(block);
                }
                else
                {

                    GameObject lastBlock = healthBlockList[healthBlockList.Count - 1];
                    RectTransform rt = lastBlock.GetComponent<RectTransform>();
                    float rightSide = rt.position.x + rt.rect.width + 1.5f;
                    Vector3 newPos = new Vector3(rightSide, rt.position.y, rt.position.z);
                    GameObject newBlock = Instantiate(healthBlockPrefab, newPos, Quaternion.identity, healthBlockStart);
                    newBlock.GetComponent<RectTransform>().sizeDelta = new Vector2(blockWidth, blockHeight);
                    healthBlockList.Add(newBlock);
                }
            }
        }
        else//for world space
        {
            blockWidth *= 0.5f;
            blockHeight *= 0.5f;
            for (int i = 0; i < totalBlocks; i++)
            {
                if (i == 0)
                {
                    GameObject block = Instantiate(healthBlockPrefab, healthBlockStart);
                    block.GetComponent<RectTransform>().sizeDelta = new Vector2(blockWidth, blockHeight);
                    healthBlockList.Add(block);
                }
                else
                { 
                    GameObject lastBlock = healthBlockList[healthBlockList.Count - 1];
                    RectTransform rt = lastBlock.GetComponent<RectTransform>();

                    Vector3 lastBlockPosition = rt.localPosition; // Use localPosition for relative positioning
                    float rightSide = lastBlockPosition.x + blockWidth + 0.5f; // Adjusted to add blockWidth correctly

                    Vector3 newPos = new Vector3(rightSide, lastBlockPosition.y, lastBlockPosition.z);
                    GameObject newBlock = Instantiate(healthBlockPrefab, healthBlockStart);
                    newBlock.GetComponent<RectTransform>().localPosition = newPos; // Set localPosition instead
                    newBlock.GetComponent<RectTransform>().sizeDelta = new Vector2(blockWidth, blockHeight);
                    healthBlockList.Add(newBlock);
                }
            }
        }
        
    }
    private void OnHealthChange(int previousHealth, int newHealth)
    {
        //Debug.Log("Health changed from " + previousHealth + " to " + newHealth);
        currentHealth.Value = newHealth;
    }
    private void FixedUpdate()
    {
        if (isducttape)
        {
            currentHealth.Value -= 25;
            Updatehp();
            isducttape = false;

        }
    }
    

    public void CalculateDamage(int damage)
    {
        //Debug.Log("Calculating damage");
        //Debug.Log("Damage un modded: " + damage);
        int calculatedDamage = PlayerController.Player.gameObject.GetComponent<Modifiers>().ApplyToDamage(damage);
        //Debug.Log("Damage modded: " + calculatedDamage);
        ApplyDamageServerRpc(calculatedDamage);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ApplyDamageServerRpc(int damage)
    {
        if (!IsServer) return;

        currentHealth.Value -= damage;
        Updatehp();
        UpdateClientHealthClientRpc();
    }

    [ClientRpc]
    private void UpdateClientHealthClientRpc()
    {
        Updatehp();
    }
    private void Updatehp()
    {
        int fullBlocks = currentHealth.Value / healthPerBlock;
        int partialBlockHealth = currentHealth.Value % healthPerBlock;
        for (int i = 0; i < healthBlockList.Count; i++)
        {
            Slider healthBlockSlider = healthBlockList[i].GetComponent<Slider>();
            GameObject fillarea = healthBlockList[i].transform.GetChild(1).gameObject;
            if (i < fullBlocks)
            {
                healthBlockSlider.value = healthPerBlock; // Full health for this block
                fillarea.SetActive(true); // Make sure the block is visible
            }
            else if (i == fullBlocks)
            {
                healthBlockSlider.value = partialBlockHealth; // Partial health for the last block
                fillarea.SetActive(partialBlockHealth > 0); // Make sure the block is visible if there is health
            }
            else
            {
                healthBlockSlider.value = 0; // No health for remaining blocks
                fillarea.SetActive(false); // Hide the block
            }
        }
    }
    void Update()
    {
        OrientTowardsCamera();
    }

    private void OrientTowardsCamera()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
}