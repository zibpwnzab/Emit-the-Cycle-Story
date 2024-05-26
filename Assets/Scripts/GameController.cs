using UnityEngine;

namespace cherrydev
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private DialogBehaviour dialogBehaviour;
        [SerializeField] private GameObject nextActionObject;
        [SerializeField] private EventTrigger TriggerObj;
        [SerializeField] private GameObject TriggerAnim;

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



        private void EventObjectTrigger()
        {
            if (TriggerObj.inAction = true) 
            {
                Debug.Log("dd");
            }

        }

        private void EventAnimTrigger()
        {


        }
    }
}

