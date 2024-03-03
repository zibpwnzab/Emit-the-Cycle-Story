using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public bool triger;
    private PlayerController player;
    [SerializeField] private GameObject gameoverImage;
    private void Start()
    {
      player = GameObject.Find("Player").GetComponent<PlayerController>();
        
    }

    private void Update()
    {
        if (player.Lifes == 0)
        {
            gameoverImage.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        player.Lifes -= 1;
        
    }
        
    

    private void OnTriggerExit(Collider other)
    {
        
    }
}
