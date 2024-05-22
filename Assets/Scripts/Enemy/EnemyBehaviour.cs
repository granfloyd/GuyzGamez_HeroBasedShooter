using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UIElements;
using System.Linq;
using UnityEngine.AI;

public class EnemyBehaviour : NetworkBehaviour
{
    [SerializeField] public GameObject Projectile1Prefab;
    [SerializeField] public GameObject Projectile2Prefab;
    [SerializeField] public bool isObjectiveOrineted = false;//if true, the enemy will focus the objective over player
    [SerializeField] public List<Vector3> playerPosition = new List<Vector3>();
    [SerializeField] public BaseAIBehaviour behaviour;
    [SerializeField] public GameObject targetPlayer;
    [SerializeField] public NavMeshAgent navAgent;   
    [SerializeField] public float maxDistanceFromPlayer;
    [SerializeField] public float minDistanceFromPlayer;
    [SerializeField] public float recovery;
    [SerializeField] public float recoveryTimer;
    public float angle;
    protected virtual void Start()
    {
        
        navAgent = GetComponent<NavMeshAgent>();

        CalculateClosestPlayer(); 
        
        behaviour = AIBehaviour.idle;
    }

    public void CalculateClosestPlayer()
    {
        if(PlayerController.Player == null)
        {
            return;
        }
        ClientSendPositionClientRpc(NetworkManager.Singleton.LocalClientId,PlayerController.Player.transform.position);
    }


    [ServerRpc(RequireOwnership = false)]
    public void ServerSetPositionServerRpc(ulong clientid, Vector3 playerposition)
    {
        playerPosition.Insert((int)clientid, playerposition);
        GetClosestPlayer();
    }

    [ClientRpc]
    public void ClientSendPositionClientRpc(ulong clientid ,Vector3 playerposition)
    {
        clientid = NetworkManager.Singleton.LocalClientId;
        playerposition = PlayerController.Player.transform.position;
        ServerSetPositionServerRpc(clientid, playerposition);
    }

    public Vector3 GetClosestPlayer()
    {
        Vector3 enemyPosition = this.transform.position;
        Vector3 closestPlayerPosition = Vector3.zero;
        float closestDistance = Mathf.Infinity;
        GameObject closestPlayer = null;

        foreach (Vector3 playerPos in playerPosition)
        {
            Vector3 directionToPlayer = playerPos - enemyPosition;
            RaycastHit hit;

            // Perform the LOS check
            if (Physics.Raycast(enemyPosition, directionToPlayer, out hit))
            {
                // If the raycast hit the player, calculate the distance
                if (hit.collider.gameObject.tag == "Player")
                {
                    float distance = Vector3.Distance(enemyPosition, playerPos);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPlayerPosition = playerPos;
                        closestPlayer = hit.collider.gameObject; // Update the closest player
                    }

                    // Debug logs
                    Debug.Log("Hit object: " + hit.transform.name);
                    Debug.Log("Distance to object: " + distance);
                }
            }
        }

        targetPlayer = closestPlayer; // Update the target player to the closest player
        return closestPlayerPosition;
    }

    void Update()
    {
        if (recoveryTimer < recovery)
        {
            recoveryTimer += Time.deltaTime;
        }
        if (behaviour != null)
        {
            //Debug.Log(behaviour.ToString());
        }
        
        DetermineBehaviour();
        RunBehaviour();
        // Move towards the closest player
        if (targetPlayer != null)
        {
            transform.LookAt(targetPlayer.transform);
            transform.Rotate(0, 180, 0);       
        }
    }
    protected void RunBehaviour()
    {
        behaviour.UpdateBehaviour(this);
    }
    protected void SwitchBehaviour(BaseAIBehaviour newBehaviour)
    {
        if (behaviour == newBehaviour)
            return;

        if (behaviour != null)
            behaviour.ExitBehaviour(this);

        behaviour = newBehaviour;
        behaviour.EnterBehaviour(this);
    }
    protected virtual void DetermineBehaviour()
    {

    }
}
