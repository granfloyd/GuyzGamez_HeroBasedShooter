using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Globalization;
public class Grapple : AbilityBase
{
    private Vector3 hitPoint;
    private bool isOut = false;
    private bool isReTracking = false;
    private float grappleSpeed = 40f;
    private float totalGrappleDistance = 0f;
    private float coveredGrappleDistance = 0f;
    public Grapple()
    {
        cooldown = 4f;
        duration = 0f;
        timer = cooldown;
        durationTimer = 0;
        isInstanced = false;
        isActive = false;
        Debug.Log("Grapple created");
    }
    public override void Use()
    {
        if (IsReady())
        {
            base.Use();
            CastGrapple();
            Debug.Log("used Grapple");
        }
    }
    public override void AbilityUpdate()
    {
        base.AbilityUpdate();
        if (isOut)
        {
            UpdateGrappleLine();
        }
        if (isReTracking)
        {
            ReTrack();
        }
    }

    public override void End()
    {
        base.End();
        HeroBase player = PlayerController.Player;

    }
    
    private void CastGrapple()
    {
        HeroBase player = PlayerController.Player;
        if (isOut)
        {
            Debug.Log("Grapple out");
            player.GetComponent<SpawnObject>().SetGameObjectServerRpc(false);
            isReTracking = true;
            totalGrappleDistance = Vector3.Distance(player.transform.position, hitPoint);
            return;
        }
        if (Physics.Raycast(player.transform.position, player.tempGunAngle, out RaycastHit hit, 20.0f))
        {
            Debug.Log("Grapple hit");
                player.GetComponent<SpawnObject>().SpawnObjectLocal(
                NetworkManager.Singleton.LocalClientId,
                player.transform.position,
                player.orientation.localRotation.normalized);

                player.GetComponent<SpawnObject>().SpawnObjectServerRpc(
                NetworkManager.Singleton.LocalClientId,
                player.transform.position,
                player.orientation.localRotation.normalized);

            hitPoint = hit.point;

            
            isOut = true;
            player.GetComponent<SpawnObject>().SetGameObjectServerRpc(true);
            player.GetComponent<SpawnObject>().lineRendererRM.SetPosition(0, player.transform.position);
            player.GetComponent<SpawnObject>().lineRendererRM.SetPosition(1, hitPoint);
        }
        else
        {
            Debug.Log("Grapple missed");
        }
    }
    private void UpdateGrappleLine()
    {
        HeroBase player = PlayerController.Player;
        if (Physics.Linecast(player.transform.position, hitPoint, out RaycastHit hit))
        {
            //if (hit.collider.gameObject != CubeGameobject)
            //{
            //    player.GetComponent<SpawnObject>().SetGameObjectServerRpc(false);
            //    return;
            //}
        }

        //If there's clear LOS, draw the line
        player.GetComponent<SpawnObject>().SetGameObjectServerRpc(true);
        player.GetComponent<SpawnObject>().lineRendererRM.SetPosition(0, player.transform.position);
        player.GetComponent<SpawnObject>().lineRendererRM.SetPosition(1, hitPoint);
    }

    private void ReTrack()
    {
        HeroBase player = PlayerController.Player;
        Vector3 direction = hitPoint - player.transform.position;
        float distanceToMove = grappleSpeed * Time.deltaTime;

        if (coveredGrappleDistance + distanceToMove > totalGrappleDistance * 0.95f)
        {
            player.controller.Move(direction.normalized * (totalGrappleDistance * 0.95f - coveredGrappleDistance));
            player.isAffectedByGravity = true;
            isReTracking = false;
            isOut = false;
            coveredGrappleDistance = 0f;
            //Destroy(hitPointInstance);
        }
        else
        {
            player.controller.Move(direction.normalized * distanceToMove);
            coveredGrappleDistance += distanceToMove;
            player.isAffectedByGravity = false;
        }
        if (!isReTracking)
        {
            player.GetComponent<SpawnObject>().SetGameObjectServerRpc(false);
        }
    }
    public override bool IsReady()
    {
        return base.IsReady();
    }
    public override void UpdateTimer()
    {
        base.UpdateTimer();
    }


}
