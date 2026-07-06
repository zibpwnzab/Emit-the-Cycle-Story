using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cherrydev;

public class DialogController : ISignal
{
    [SerializeField] DialogBehaviour dialogBehaviour;
    [SerializeField] DialogNodeGraph graph;
    [SerializeField] Vector2 carmaBorders;
    [SerializeField] bool needSignal;
    [SerializeField] ISignal signal;
    [SerializeField] TMPro.TMP_Text carmaText;
    [SerializeField] GameObject blackScreen;
    [SerializeField] bool hasBalckScreen;
    bool _dialogDone = false;
    bool _dialogFinished = false;

    void Start()
    {
        if (carmaText)
        {
            var carma = KarmaManager.Instance.GetCarma();
            carmaText.text = $"CURRENT CARMA: {carma}";
        }
    }

    void Update()
    {
        var carma = KarmaManager.Instance.GetCarma();
        if (carmaText) carmaText.text = $"CURRENT CARMA: {carma}";

        if (_dialogDone) return;

        if (!(carmaBorders.x <= carma && carma <= carmaBorders.y)) return;

        if (needSignal)
        {
            if (signal.Signal())
            {
                StartDialogSequence();
            }
        }
        else
        {
            StartDialogSequence();
        }
    }

    private void StartDialogSequence()
    {
        _dialogDone = true;
        dialogBehaviour.StartDialog(graph);
        if (hasBalckScreen) blackScreen.SetActive(true);
    }

    public void FinishDialog()
    {
        _dialogFinished = true;
        if (hasBalckScreen) blackScreen.SetActive(false);
        KarmaManager.Instance.AddCarma(dialogBehaviour.currentNode.answerCarmaValue);
        Debug.Log("GG");
    }

    public void ResetDialogCarma()
    {
        KarmaManager.Instance.SetCarma(0);
    }

    public override bool Signal()
    {
        return _dialogFinished;
    }
}
