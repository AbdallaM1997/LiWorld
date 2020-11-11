using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling : MonoBehaviour
{
    [HideInInspector]
    public int damageammount;
    public bool falling;

    #region Singleton 
    public static Falling instance;
    void Awake()
    {
        instance = this;
        //player = GameObject.FindGameObjectWithTag("Player");
    }
    #endregion
    private void Start()
    {
        damageammount = PlayerController.instance.health;
        falling = false;
    }
    public void SetFalling(bool seting)
    {
        falling = seting;
    }

    public bool GetFalling()
    {
        return falling; 
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.collider.GetComponent<PlayerController>().TakeDamage(damageammount);
        Falling.instance.SetFalling(true);
    }
}
