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
        private const float DefaultActivationDelay = 3f;

        [SerializeField] private DialogBehaviour dialogBehaviour;
        [SerializeField] private List<GameObjectWithFlag> gameObjectsWithFlags;
        [SerializeField] private EventTrigger TriggerObj;
        [SerializeField] private GameObject TriggerAnim;

        public static GameController instance;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            if (gameObjectsWithFlags != null)
            {
                foreach (var item in gameObjectsWithFlags)
                {
                    if (item?.gameObject != null)
                    {
                        item.gameObject.SetActive(false);
                    }
                }
            }

            if (dialogBehaviour != null)
            {
                dialogBehaviour.AddListenerToDialogFinishedEvent(OnDialogFinished);
            }
        }

        private void OnDialogFinished()
        {
            if (gameObjectsWithFlags == null) return;

            foreach (var item in gameObjectsWithFlags)
            {
                if (item?.gameObject == null) continue;

                if (item.activateAfterDelay)
                {
                    StartCoroutine(ActivateAfterDelay(item.gameObject, DefaultActivationDelay));
                }
                else
                {
                    item.gameObject.SetActive(true);
                }
            }
        }

        private IEnumerator ActivateAfterDelay(GameObject targetObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            if (targetObject != null)
            {
                targetObject.SetActive(true);
            }
        }

        public void EventObjectTrigger() { }

        public void EventAnimTrigger()
        {
            ParabolaMovement parabola = FindFirstObjectByType<ParabolaMovement>();
            if (parabola != null)
            {
                StartCoroutine(parabola.Jump());
            }
        }
    }
}