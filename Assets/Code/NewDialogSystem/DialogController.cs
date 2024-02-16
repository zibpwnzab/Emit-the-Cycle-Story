using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cherrydev;

public class DialogController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] DialogBehaviour dialogBehaviour;
    [SerializeField] DialogNodeGraph graph;
    [SerializeField] bool needSignal;
    [SerializeField] ISignal signal;
    [SerializeField] TMPro.TMP_Text carmaText;
    bool _dialogDone = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (carmaText) carmaText.text = $"CURRENT CARMA: {PlayerPrefs.GetInt(PlayerController.PLAYER_CARMA_KEY)}";
        if (_dialogDone) return;
        if (needSignal)
        {
            if (signal.Signal())
            {
                dialogBehaviour.StartDialog(graph);
                _dialogDone = true;
            }
        }
        else
        {
            _dialogDone = true;
            dialogBehaviour.StartDialog(graph);
        }
        
    }

    public void FinishDialog()
    {
        PlayerPrefs.SetInt(PlayerController.PLAYER_CARMA_KEY, PlayerPrefs.GetInt(PlayerController.PLAYER_CARMA_KEY) + dialogBehaviour.currentNode.answerCarmaValue);
    }

    public void ResetDialogCarma()
    {
        PlayerPrefs.SetInt(PlayerController.PLAYER_CARMA_KEY, 0);
    }
}
