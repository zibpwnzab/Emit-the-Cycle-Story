using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifes : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private GameObject heartImage1;
    [SerializeField] private GameObject heartImage2;
    [SerializeField] private GameObject heartImage3;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    
    void Update()
    {
        if (player.Lifes < 3)
        {
            heartImage3.SetActive(false);
        }

        if (player.Lifes == 1) 
        { 
        heartImage2.SetActive(false);
        }
    }
}
