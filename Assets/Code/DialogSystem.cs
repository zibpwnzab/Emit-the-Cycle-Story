using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialogSystem : MonoBehaviour
{
    public string[] lines;
    public float speedText;
    public Text dialogText;
    private int index;
    public Button buttonOne;
    public Button buttonTwo;
    public Button buttonThree;
    public int count = 0;

    private void Start()
    {
        dialogText.text = string.Empty;
        StartDialog();
        buttonOne.onClick.AddListener(TaskOnClickOne);
        buttonTwo.onClick.AddListener(TaskOnClickTwo);
        buttonThree.onClick.AddListener(TaskOnClickThree);

    }

    private void TaskOnClickOne()
    {
        skipText();
        count++;
    }
    private void TaskOnClickTwo()
    {
        skipText();
    }
    private void TaskOnClickThree()
    {
        skipText();
        count--;
    }

    void StartDialog()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray()) {
        dialogText.text += c;
            yield return new WaitForSeconds(speedText);
        }
    }

    public void skipText()
    {
        if (dialogText.text == lines[index])
        {
            NextLine();
        }
        else 
        {
        StopAllCoroutines();
            dialogText.text = lines[index]; 
        }
    }

    private void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            dialogText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else 
        {
            gameObject.SetActive(false);
        }
    }
}
