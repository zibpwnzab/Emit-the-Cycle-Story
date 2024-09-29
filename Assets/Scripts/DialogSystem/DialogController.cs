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
        // ќтображаем текущую карму при старте, если текстовое поле доступно
        if (carmaText)
        {
            var carma = KarmaManager.Instance.GetCarma();
            carmaText.text = $"CURRENT CARMA: {carma}";
        }
    }

    void Update()
    {
        // ќбновл€ем отображение кармы в UI
        var carma = KarmaManager.Instance.GetCarma();
        if (carmaText) carmaText.text = $"CURRENT CARMA: {carma}";

        // ≈сли диалог уже завершен, выходим
        if (_dialogDone) return;

        // ѕровер€ем, находитс€ ли текуща€ карма в пределах допустимых границ
        if (!(carmaBorders.x <= carma && carma <= carmaBorders.y)) return;

        // ≈сли нужен сигнал дл€ начала диалога
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
        // ѕосле завершени€ диалога добавл€ем значение кармы, указанное в текущем ответе
        KarmaManager.Instance.AddCarma(dialogBehaviour.currentNode.answerCarmaValue);
        Debug.Log("GG");
    }

    public void ResetDialogCarma()
    {
        // —брасываем карму через KarmaManager
        KarmaManager.Instance.SetCarma(0);
    }

    public override bool Signal()
    {
        return _dialogFinished;
    }
}
