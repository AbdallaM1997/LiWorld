using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CoinPickup : MonoBehaviour
{
    public GameObject pickUpEffect;
    public AudioSource audioSource;

    private int coinAmount;

    private void OnTriggerEnter(Collider other)
    {
        coinAmount = Random.Range(1,9);

        if (other.tag == "Player")
        {
            audioSource.Play();
            Pickup();
        }
    }

    void Pickup()
    {
        GameObject temp = Instantiate(pickUpEffect, transform.position, Quaternion.identity);
        SpwanSystem.instance.SetCoinCount(coinAmount);
        Destroy(gameObject,0.2f);
        Destroy(temp, 0.2f);
    }
}
