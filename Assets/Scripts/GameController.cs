using System;
using UnityEngine;

namespace cherrydev
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private DialogBehaviour dialogBehaviour;
        [SerializeField] private GameObject nextActionObject;
        [SerializeField] private EventTrigger TriggerObj;
        [SerializeField] private GameObject TriggerAnim;
        public static GameController instance;
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

            if (nextActionObject != null)
            {
                nextActionObject.SetActive(false);
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
            if (nextActionObject != null)
            {
                nextActionObject.SetActive(true);
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
