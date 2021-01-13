using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSound;
    [SerializeField] int scoreOnPickup = 100;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FindObjectOfType<GameSession>().AddToScore(scoreOnPickup);
        scoreOnPickup = 0;
        AudioSource.PlayClipAtPoint(coinPickupSound, Camera.main.transform.position);
        Destroy(gameObject);
    }
}
