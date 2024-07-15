using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class RectangleMan : HeroBase
{
    //[SerializeField] private Vector3 hitPoint;
    //[SerializeField] public GameObject grapplePrefab;
    //private GameObject hitPointInstance;
    
    //public LineRenderer lineRendererRM;
    private void Start()
    {
        if (IsOwner)
        {
            
            PlayerCamera.iscamset = false;
            PlayerController.Player.BaseAbility1 = new Grapple();
            //PlayerController.Player.baseAbility1 = new Ability(3f, 0);
            //PlayerController.Player.baseAbility2 = new Ability(3f, 0);
            //PlayerController.Player.baseAbility3 = new Ability(1f, 0);
            HeroBase player = PlayerController.Player;
            HeroUI.Instance.SetUltSlider();
            if (player == null)
            {
                Debug.LogError("Player is not set yet.");
                return;
            }
            Cursor.lockState = CursorLockMode.Locked;
        }

    }
    protected new void Update()
    {
        base.Update();
        if (IsOwner)
        {
            HeroBase player = PlayerController.Player;
            player.BaseAbility1.AbilityUpdate();
            //player.baseAbility2.UpdateTimer();
            //player.baseAbility3.UpdateTimer();
            //HeroUI.Instance.UpdateAbilityCD(player.baseAbility1, HeroUI.Instance.ability1Text);
            //HeroUI.Instance.UpdateAbilityCD(player.baseAbility2, HeroUI.Instance.ability2Text);
        }
        //if (isOut && lineRenderer != null)
        //{
        //    UpdateGrappleLine();
        //}
        //if (isReTracking)
        //{
        //    ReTrack();
        //}
    }
    private void UpdateGrappleLine()
    {
        //if (Physics.Linecast(transform.position, hitPoint, out RaycastHit hit))
        //{
        //    if (hit.collider.gameObject != hitPointInstance)
        //    {
        //        lineRendererRM.enabled = false;
        //        return;
        //    }
        //}

        // If there's clear LOS, draw the line
        //lineRendererRM.enabled = true;
        //lineRendererRM.SetPosition(0, transform.position);
        //lineRendererRM.SetPosition(1, hitPoint);
    }
    public override void PrimaryFire()
    {
        if (IsOwner)
        {
            HeroBase player = PlayerController.Player;

        }
    }
    public override void SecondaryFire()
    {
        if (IsOwner)
        {
            HeroBase player = PlayerController.Player;
        }
    }
    void AssignBulletToPlayer(GameObject spawnedGameObject, ulong clientid)
    {
        NetworkObject bulletNetworkObject = spawnedGameObject.GetComponent<NetworkObject>();
        bulletNetworkObject.SpawnWithOwnership(clientid);
        if (!IsOwnedByServer)
            bulletNetworkObject.NetworkHide(clientid);

    }


    public override void Ability1()
    {
        HeroBase player = PlayerController.Player;
        if (IsOwner)
        {
            player.BaseAbility1.Use();
            if (player.BaseAbility1.duration > 0)
            {
                Invoke("Ability1End", player.BaseAbility1.duration);
            }
        }
    }

    public override void Ability1End()
    {
        base.Ability1End();
    }
    //public override void Ability1()
    //{
    //    if (IsOwner)
    //    {
    //        Debug.Log("pressed");
    //        HeroBase player = PlayerController.Player;
    //        player.BaseAbility1.Use();
    //        //if (isOut)
    //        //{
    //        //    lineRenderer.enabled = false;
    //        //    isReTracking = true;
    //        //    totalGrappleDistance = Vector3.Distance(player.transform.position, hitPoint);
    //        //    return;
    //        //}
    //        //if (Physics.Raycast(player.transform.position, player.tempGunAngle, out RaycastHit hit, 20.0f))
    //        //{
    //        //    hitPoint = hit.point;
    //        //    hitPointInstance = Instantiate(grapplePrefab, transform.position, player.orientation.localRotation.normalized);
    //        //    hitPointInstance.GetComponent<ClientProjectile>().SetMovement(player.tempGunAngle.normalized, 20);
    //        //    isOut = true;
    //        //    if (lineRenderer != null)
    //        //    {
    //        //        lineRenderer.enabled = true;
    //        //        lineRenderer.SetPosition(0, transform.position);
    //        //        lineRenderer.SetPosition(1, hitPoint);
    //        //    }
    //        //}
    //    }
    //}
    private void ReTrack()
    {
        if (IsOwner)
        {
            //HeroBase player = PlayerController.Player;
            //Vector3 direction = hitPoint - transform.position;
            //float distanceToMove = grappleSpeed * Time.deltaTime;
            //
            //if (coveredGrappleDistance + distanceToMove > totalGrappleDistance * 0.95f)
            //{
            //    player.controller.Move(direction.normalized * (totalGrappleDistance * 0.95f - coveredGrappleDistance));
            //    player.isAffectedByGravity = true;
            //    isReTracking = false;
            //    isOut = false;
            //    coveredGrappleDistance = 0f;
            //    Destroy(hitPointInstance);
            //}
            //else
            //{
            //    player.controller.Move(direction.normalized * distanceToMove);
            //    coveredGrappleDistance += distanceToMove;
            //    player.isAffectedByGravity = false;
            //}
            //if (!isReTracking && lineRenderer != null)
            //{
            //    lineRenderer.enabled = false;
            //}
        }
    }


    public override void Ability2()
    {
        if (IsOwner)
        {

        }
    }

    public override void Ability3()
    {
        if (IsOwner)
        {

        }
    }
}
