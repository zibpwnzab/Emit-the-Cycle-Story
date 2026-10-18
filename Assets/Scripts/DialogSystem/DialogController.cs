using cherrydev;
using TMPro;
using UnityEngine;

public class DialogController : ISignal
{
    [SerializeField] private DialogBehaviour dialogBehaviour;
    [SerializeField] private DialogNodeGraph graph;
    [SerializeField] private Vector2 carmaBorders;
    [SerializeField] private bool needSignal;
    [SerializeField] private ISignal signal;
    [SerializeField] private TMP_Text carmaText;
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private bool hasBalckScreen;

    private bool _dialogDone;
    private bool _dialogFinished;
    private int _cachedKarma = int.MinValue;

    private void Start()
    {
        UpdateKarmaDisplay();
    }

    private void Update()
    {
        if (KarmaManager.Instance == null) return;

        UpdateKarmaDisplay();

        if (_dialogDone) return;

        int currentKarma = KarmaManager.Instance.GetCarma();
        if (currentKarma < carmaBorders.x || currentKarma > carmaBorders.y) return;

        if (needSignal)
        {
            if (signal != null && signal.Evaluate())
            {
                StartDialogSequence();
            }
        }
        else
        {
            StartDialogSequence();
        }
    }

    private void UpdateKarmaDisplay()
    {
        if (carmaText == null || KarmaManager.Instance == null) return;

        int currentKarma = KarmaManager.Instance.GetCarma();
        if (currentKarma != _cachedKarma)
        {
            _cachedKarma = currentKarma;
            carmaText.text = $"CURRENT KARMA: {currentKarma}";
        }
    }

    private void StartDialogSequence()
    {
        _dialogDone = true;
        if (dialogBehaviour != null)
        {
            dialogBehaviour.StartDialog(graph);
        }

        if (hasBalckScreen && blackScreen != null)
        {
            blackScreen.SetActive(true);
        }
    }

    public void FinishDialog()
    {
        _dialogFinished = true;

        if (hasBalckScreen && blackScreen != null)
        {
            blackScreen.SetActive(false);
        }

        if (KarmaManager.Instance != null && dialogBehaviour != null && dialogBehaviour.currentNode != null)
        {
            KarmaManager.Instance.AddCarma(dialogBehaviour.currentNode.answerCarmaValue);
        }
    }

    public void ResetDialogCarma()
    {
        if (KarmaManager.Instance != null)
        {
            KarmaManager.Instance.SetCarma(0);
        }
    }

    public override bool Signal() => _dialogFinished;
}