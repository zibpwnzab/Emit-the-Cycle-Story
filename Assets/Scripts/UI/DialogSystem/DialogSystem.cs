using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using static System.Collections.Specialized.BitVector32;

public class DialogSystem : MonoBehaviour
{
    public float speedText;
    public Text dialogText;
    private int index = 0;
    private int section = 0;
    public Button buttonOne;
    public Button buttonTwo;
    public Button buttonThree;
    public int count = 0;
    [SerializeField] private DialogueTree text;
    bool skipLineTriggered;
    [SerializeField] Button[] answerObjects;
    [SerializeField] GameObject answerBox;
    [SerializeField] GameObject dialogueBox;
    bool answerTriggered;
    int answerIndex;

    private void Start()
    {
        dialogText.text = string.Empty;
        
        buttonOne.onClick.AddListener(TaskOnClickOne);
    }

    private void TaskOnClickOne()
    {
        StartCoroutine(TypeLine());
        count++;
    }

    private void TaskOnClickTwo()
    {

        //skipText2();
    }
    private void TaskOnClickThree()
    {

        //skipText3();
        count--;
    }

    void StartDialog()
    {
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in text.sections[section].dialogue[index])
        {
            dialogText.text += c;
            yield return new WaitForSeconds(speedText);
        }
        index++;
        
    }

    public void skipText()
    {
        index++;
        if (dialogText.text.Length == text.sections[section].dialogue.Length)
        {

            NextLine();
        }
        else
        {
            StopAllCoroutines();
            dialogueBox.SetActive(false);
            dialogText.text = text.sections[section].branchPoint.question;
            ShowAnswers(text.sections[section].branchPoint);
            dialogText.text = text.sections[section].dialogue[index];
        }
    }

    private void NextLine()
    {
        
        if (index < text.sections[section].dialogue[index].Length)
        {

            dialogText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void ShowAnswers(BranchPoint branchPoint)
    {
        
        answerBox.SetActive(true);
        for (int i = 0; i < branchPoint.answers.Length; i++)
        {
            if (i < branchPoint.answers.Length)
            {
                answerObjects[i].GetComponentInChildren<TextMeshProUGUI>().text = branchPoint.answers[i].answerLabel;
                answerObjects[i].gameObject.SetActive(true);
            }
            else
            {
                answerBox.SetActive(false);
            }
        }
    }
    public void AnswerQuestion(int answer)
    {
        answerIndex = answer;
        answerTriggered = true;
    }

    /*IEnumerator TypeLine2()
    {
        foreach (char c in lines2[index].ToCharArray())
        {
            dialogText.text += c;
            yield return new WaitForSeconds(speedText);
        }
    }

    public void skipText2()
    {
        index++;
        if (dialogText.text == lines2[index])
        {

            NextLine2();
        }
        else
        {
            StopAllCoroutines();
            dialogText.text = lines2[index];
        }
    }

    private void NextLine2()
    {
        if (index < lines2.Length)
        {

            dialogText.text = string.Empty;
            StartCoroutine(TypeLine2());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator TypeLine3()
    {
        foreach (char c in lines3[index].ToCharArray())
        {
            dialogText.text += c;
            yield return new WaitForSeconds(speedText);
        }
    }

    public void skipText3()
    {
        index++;
        if (dialogText.text == lines3[index])
        {

            NextLine3();
        }
        else
        {
            StopAllCoroutines();
            dialogText.text = lines3[index];
        }
    }

    private void NextLine3()
    {
        if (index < lines3.Length)
        {

            dialogText.text = string.Empty;
            StartCoroutine(TypeLine3());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }*/

}