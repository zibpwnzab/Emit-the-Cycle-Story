using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cherrydev
{
    [Serializable]
    public class GameObjectWithFlag
    {
        public GameObject gameObject;
        public bool activateAfterDelay;
    }
    public class GameController : MonoBehaviour
    {
        [SerializeField] private DialogBehaviour dialogBehaviour;
        [SerializeField] private List<GameObjectWithFlag> gameObjectsWithFlags;
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

            if (gameObjectsWithFlags != null)
            {
                while (index < gameObjectsWithFlags.Count)
                {
                    gameObjectsWithFlags[index].gameObject.SetActive(false);
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

            if (gameObjectsWithFlags != null)
            {
                index = 0;
                while (index < gameObjectsWithFlags.Count)
                {
                    if (gameObjectsWithFlags[index].activateAfterDelay)
                    {
                        StartCoroutine(ActivateAfterDelay(gameObjectsWithFlags[index].gameObject, 3f));
                    }
                    else
                    {
                        gameObjectsWithFlags[index].gameObject.SetActive(true);
                    }
                    index++;
                }
            }
            else
            {
                Debug.LogWarning("NextActionScript не присвоен в GameController.");
            }
        }

        private IEnumerator ActivateAfterDelay(GameObject gameObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            gameObject.SetActive(true);
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
