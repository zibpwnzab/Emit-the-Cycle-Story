using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private PlayerController player;
    private void Start()
    {
        player = GameObject.Find("SM_Chr_Homeless_Male_01").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        player.Lifes -= 1;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(player.Lifes);
    }
}
