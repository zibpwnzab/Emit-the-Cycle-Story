using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace cherrydev
{
    public class DialogBehaviour : MonoBehaviour
    {
        [SerializeField] private float dialogCharDelay = 0.05f;
        [SerializeField] private List<KeyCode> nextSentenceKeyCodes = new List<KeyCode> { KeyCode.Space };
        [SerializeField] private bool isCanSkippingText = true;
        [SerializeField] private bool isAutoProceed = false;
        [SerializeField] private float autoProceedDelay = 2.0f; // Добавляем настройку для задержки при автоматическом переключении

        [Space(10)]
        [SerializeField] private UnityEvent onDialogStarted;
        [SerializeField] private UnityEvent onDialogFinished;

        private DialogNodeGraph currentNodeGraph;
        public Node currentNode;

        private int maxAmountOfAnswerButtons;
        private bool isDialogStarted;
        private bool isCurrentSentenceSkipped;

        public event Action OnSentenceNodeActive;
        public event Action<string, string, Sprite> OnSentenceNodeActiveWithParameter;
        public event Action OnAnswerNodeActive;
        public event Action<int, AnswerNode> OnAnswerButtonSetUp;
        public event Action<int> OnMaxAmountOfAnswerButtonsCalculated;
        public event Action<int> OnAnswerNodeActiveWithParameter;
        public event Action<int, string> OnAnswerNodeSetUp;
        public event Action OnDialogTextCharWrote;
        public event Action<string> OnDialogTextSkipped;

        private void Update()
        {
            HandleSentenceSkipping();
        }

        public void StartDialog(DialogNodeGraph dialogNodeGraph)
        {
            if (dialogNodeGraph == null || dialogNodeGraph.nodesList == null || dialogNodeGraph.nodesList.Count == 0)
            {
                Debug.LogWarning("Dialog Graph's node list is empty or null");
                return;
            }

            isDialogStarted = true;
            onDialogStarted?.Invoke();

            currentNodeGraph = dialogNodeGraph;
            DefineFirstNode(dialogNodeGraph);
            CalculateMaxAmountOfAnswerButtons();
            HandleDialogGraphCurrentNode(currentNode);
        }

        public void AddListenerToDialogFinishedEvent(UnityAction action)
        {
            onDialogFinished.AddListener(action);
        }

        public void SetCurrentNodeAndHandleDialogGraph(Node node)
        {
            currentNode = node;
            HandleDialogGraphCurrentNode(currentNode);
        }

        private void HandleDialogGraphCurrentNode(Node currentNode)
        {
            StopAllCoroutines();

            if (currentNode is SentenceNode sentenceNode)
            {
                isCurrentSentenceSkipped = false;

                OnSentenceNodeActive?.Invoke();
                OnSentenceNodeActiveWithParameter?.Invoke(sentenceNode.GetSentenceCharacterName(), sentenceNode.GetSentenceText(), sentenceNode.GetCharacterSprite());

                WriteDialogText(sentenceNode.GetSentenceText());
            }
            else if (currentNode is AnswerNode answerNode)
            {
                int amountOfActiveButtons = 0;
                OnAnswerNodeActive?.Invoke();

                for (int i = 0; i < answerNode.childSentenceNodes.Count; i++)
                {
                    if (answerNode.childSentenceNodes[i] != null)
                    {
                        OnAnswerNodeSetUp?.Invoke(i, answerNode.answers[i]);
                        OnAnswerButtonSetUp?.Invoke(i, answerNode);

                        amountOfActiveButtons++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (amountOfActiveButtons == 0)
                {
                    isDialogStarted = false;
                    onDialogFinished?.Invoke();
                    return;
                }

                OnAnswerNodeActiveWithParameter?.Invoke(amountOfActiveButtons);
            }
        }

        private void DefineFirstNode(DialogNodeGraph dialogNodeGraph)
        {
            if (dialogNodeGraph.nodesList.Count == 0)
            {
                Debug.LogWarning("The list of nodes in the DialogNodeGraph is empty");
                return;
            }

            foreach (Node node in dialogNodeGraph.nodesList)
            {
                currentNode = node;

                if (node is SentenceNode sentenceNode)
                {
                    if (sentenceNode.parentNode == null && sentenceNode.childNode != null)
                    {
                        currentNode = sentenceNode;
                        return;
                    }
                }
            }

            currentNode = dialogNodeGraph.nodesList[0];
        }

        private void WriteDialogText(string text)
        {
            StartCoroutine(WriteDialogTextRoutine(text));
        }

        private IEnumerator WriteDialogTextRoutine(string text)
        {
            foreach (char textChar in text)
            {
                if (isCurrentSentenceSkipped)
                {
                    OnDialogTextSkipped?.Invoke(text);
                    break;
                }

                OnDialogTextCharWrote?.Invoke();

                yield return new WaitForSeconds(dialogCharDelay);
            }

            if (isAutoProceed)
            {
                yield return new WaitForSeconds(autoProceedDelay); // Используем задержку из настройки
                CheckForDialogNextNode();
            }
            else
            {
                yield return new WaitUntil(CheckNextSentenceKeyCodes);
                CheckForDialogNextNode();
            }
        }

        private void CheckForDialogNextNode()
        {
            if (currentNode is SentenceNode sentenceNode)
            {
                if (sentenceNode.childNode != null)
                {
                    currentNode = sentenceNode.childNode;
                    HandleDialogGraphCurrentNode(currentNode);
                }
                else
                {
                    isDialogStarted = false;
                    onDialogFinished?.Invoke();
                }
            }
        }

        private void CalculateMaxAmountOfAnswerButtons()
        {
            foreach (Node node in currentNodeGraph.nodesList)
            {
                if (node is AnswerNode answerNode)
                {
                    if (answerNode.answers.Count > maxAmountOfAnswerButtons)
                    {
                        maxAmountOfAnswerButtons = answerNode.answers.Count;
                    }
                }
            }

            OnMaxAmountOfAnswerButtonsCalculated?.Invoke(maxAmountOfAnswerButtons);
        }

        private void HandleSentenceSkipping()
        {
            if (!isDialogStarted || !isCanSkippingText)
            {
                return;
            }

            if (CheckNextSentenceKeyCodes() && !isCurrentSentenceSkipped)
            {
                isCurrentSentenceSkipped = true;
            }
        }

        private bool CheckNextSentenceKeyCodes()
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began) return true;
            }

#if UNITY_EDITOR
            for (int i = 0; i < nextSentenceKeyCodes.Count; i++)
            {
                if (Input.GetKeyDown(nextSentenceKeyCodes[i]))
                {
                    return true;
                }
            }
#endif
            return false;
        }
    }
}
