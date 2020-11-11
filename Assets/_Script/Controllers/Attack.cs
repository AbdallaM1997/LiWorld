using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EZCameraShake;

public class Attack : MonoBehaviour
{
    [HideInInspector]
    public int damage;
    public float attackspeed = 1f;
    public GameObject attackEffect;

    private float attackcooldown = 0f;
    private ThridPersonInput player;

    private void Start()
    {
        //player = transform.parent.GetComponent<ThridPersonInput>();
        damage = Upgrade.instance.GetGlobalDamage();
        //Debug.LogWarning("Damage " + damage);
        if (SceneManager.GetActiveScene().name != "MainMenu")
            player = SelectPlayer.instance.player.GetComponent<ThridPersonInput>();

    }
    private void Update()
    {
        attackcooldown -= Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            if (player.isAttack)
            {
                EnemyController enemy = other.GetComponent<EnemyController>();
                if (other.tag == "Enemy" || other.tag == "Boss")
                {
                    if (!other.GetComponent<EnemyController>().isDied)
                    {
                        MakeAttack(enemy);
                        //Debug.LogWarning("Attack");
                    }
                }
            }
        }
    }

    private void MakeAttack(EnemyController enemy)
    {

        if (attackcooldown <= 0)
        {
            //CameraShaker.Instance.ShakeOnce(3f, 3f, 1f, 2f);
            GameObject instEffect = Instantiate(attackEffect, transform.position, Quaternion.identity);
            enemy.GetComponent<EnemyController>().TakeDamage(damage);
            attackcooldown = 1f / attackspeed;
            Destroy(instEffect, 1f);
        }

    }
}
