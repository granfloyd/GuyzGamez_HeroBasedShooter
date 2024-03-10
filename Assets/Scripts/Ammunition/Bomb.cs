using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Projectile
{
    public GameObject lilSwaggyPrefab;
    public float launchForce = 30f;
    public int numberOfPrefabs = 7;
    private void Update()
    {
        transform.Rotate(0, speed * 5 * Time.deltaTime,0);
    }
    public override void HandleCollision(Collision other)
    {
        if (other.gameObject.tag != "Player")
        {
            rb.velocity = Vector3.zero;
            for (int i = 0; i < numberOfPrefabs; i++)
            {
                float angle = i * 360f / numberOfPrefabs;
                Quaternion rotation = Quaternion.Euler(0, angle, 0);
                GameObject lilSwaggy = Instantiate(lilSwaggyPrefab, transform.position, rotation);
                Rigidbody lilSwaggyRb = lilSwaggy.GetComponent<Rigidbody>();
                if (lilSwaggyRb != null)
                {
                    Vector3 launchDirection = lilSwaggy.transform.up + lilSwaggy.transform.forward;
                    lilSwaggyRb.AddForce(launchDirection * launchForce, ForceMode.VelocityChange);
                }
                Destroy(lilSwaggy, 2f);
            }

            Destroy(gameObject);
        }
    }
}
