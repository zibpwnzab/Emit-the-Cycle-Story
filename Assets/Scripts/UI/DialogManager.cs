/*using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private List<DialogController> dialogControllers;
    private int currentDialogIndex = 0;

    void Start()
    {
        if (dialogControllers.Count > 0)
        {
            StartDialog(currentDialogIndex);
        }
    }

    void Update()
    {
        if (dialogControllers[currentDialogIndex].IsDialogFinished())
        {
            SwitchToNextDialog();
        }
    }

    private void StartDialog(int index)
    {
        dialogControllers[index].StartDialog();
    }

    private void SwitchToNextDialog()
    {
        if (currentDialogIndex < dialogControllers.Count - 1)
        {
            currentDialogIndex++;
            StartDialog(currentDialogIndex);
        }
        else
        {
            Debug.Log("All dialogs finished");
        }
    }
}
*/