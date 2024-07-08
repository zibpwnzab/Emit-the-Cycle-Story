using System;
using System.Collections.Generic;
using UnityEngine;

namespace cherrydev
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private DialogBehaviour dialogBehaviour;
        [SerializeField] private List<GameObject> gameObjects;
        [SerializeField] private EventTrigger TriggerObj;
        [SerializeField] private GameObject TriggerAnim;
        public static GameController instance;
        private int index = 0;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {

            if (gameObjects != null)
            {
                while (index < gameObjects.Count)
                {
                    gameObjects[index].SetActive(false);
                    index++;
                }
            }

            // Подписываемся на событие завершения диалога
            if (dialogBehaviour != null)
            {
                dialogBehaviour.AddListenerToDialogFinishedEvent(OnDialogFinished);
            }
            else
            {
                Debug.LogWarning("DialogBehaviour не присвоен в GameController.");
            }
        }

        private void OnDialogFinished()
        {
            Debug.Log("Диалог завершен!");

            // Запускаем следующий скрипт
            if (gameObjects != null)
            {
                index = 0;
                while (index < gameObjects.Count)
                {
                    gameObjects[index].SetActive(true);
                    index++;
                }
            }
            else
            {
                Debug.LogWarning("NextActionScript не присвоен в GameController.");
            }
        }



        public void EventObjectTrigger()
        {
            
                Debug.Log("eot");
            

        }

        public void EventAnimTrigger()
        {
            FindObjectOfType<ParabolaMovement>().StartCoroutine("Jump");
        }

    }
}
