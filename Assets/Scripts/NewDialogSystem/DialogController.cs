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

    }

    void Update()
    {
        var carma = LevelManager.Instance.GetCarma();
        if (carmaText) carmaText.text = $"CURRENT CARMA: {carma}";
        if (_dialogDone) return;

        if (!(carmaBorders.x <= carma && carma <= carmaBorders.y)) return;
        if (needSignal)
        {
            if (signal.Signal())
            {
                dialogBehaviour.StartDialog(graph);
                if (hasBalckScreen == true) blackScreen.SetActive(true);
                _dialogDone = true;
            }
        }
        else
        {
            _dialogDone = true;
            dialogBehaviour.StartDialog(graph);
            if (hasBalckScreen == true) blackScreen.SetActive(true);
        }

    }

    public void FinishDialog()
    {
        _dialogFinished = true;
        if (hasBalckScreen == true) blackScreen.SetActive(false);
        LevelManager.Instance.AddCarma(dialogBehaviour.currentNode.answerCarmaValue);
        if (_dialogFinished) Debug.Log("GG");
    }

    public void ResetDialogCarma()
    {
        LevelManager.Instance.SetCarma(0);
        LevelManager.Instance.Save();
    }

    public override bool Signal()
    {
        return _dialogFinished;
    }
}
