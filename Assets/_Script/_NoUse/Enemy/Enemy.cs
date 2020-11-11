using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharatersStats))]
public class Enemy : Interactable
{
    SelectPlayer playerManager;
    CharatersStats myStats;

    private void Start()
    {
        playerManager = SelectPlayer.instance;
        myStats = GetComponent<CharatersStats>();
    }
    public override void Interact()
    {
        base.Interact();

        // attac the enemy
        CharaterCombat playerCombat =  playerManager.player.GetComponent<CharaterCombat>();
        if(playerCombat != null)
        {
            playerCombat.Attack(myStats);
        }
    }
}
