using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialogSystem : MonoBehaviour
{
    public string[] lines1;
    public string[] lines2;
    public string[] lines3;
    public float speedText;
    public Text dialogText;
    private int index = 0;
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
        index++;
        skipText();
        count++;
    }
    private void TaskOnClickTwo()
    {
        index++;
        skipText2();
    }
    private void TaskOnClickThree()
    {
        index++;
        skipText3();
        count--;
    }

    void StartDialog()
    {
        //index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines1[index].ToCharArray()) {
        dialogText.text += c;
            yield return new WaitForSeconds(speedText);
        }
    }

    public void skipText()
    {
        
        if (dialogText.text == lines1[index])
        {
            
            NextLine();
        }
        else 
        {
        StopAllCoroutines();
            dialogText.text = lines1[index]; 
        }
    }

    private void NextLine()
    {
        if (index < lines1.Length - 1)
        {
            
            dialogText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else 
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator TypeLine2()
    {
        foreach (char c in lines2[index].ToCharArray())
        {
            dialogText.text += c;
            yield return new WaitForSeconds(speedText);
        }
    }

    public void skipText2()
    {
         
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
        if (index < lines2.Length - 1)
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
        if (index < lines3.Length - 1)
        {
            
            dialogText.text = string.Empty;
            StartCoroutine(TypeLine3());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
