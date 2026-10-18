using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockUI : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject bachGround;
    [SerializeField] List<Button> numbers;
    [SerializeField] Button resetButton;
    [SerializeField] Button enterButton;
    [SerializeField] TMPro.TMP_Text passwordField;
    [SerializeField] int passwordMaxLength;
    [SerializeField] string wrongMessege = "WRONG";
    [SerializeField] string correctMessege = "CORRECT";
    [SerializeField] float timeAfterCorrect;
    [SerializeField]  private string password;
    private string _currentAttempt;
    private bool _isCorrect = false;
    void Start()
    {
        _currentAttempt = "";
        SetFieldText(_currentAttempt);
        for (int i = 0; i < numbers.Count; i++)
        {
            int c = 0 + i;
            if (numbers[i] != null) numbers[i].onClick.AddListener(() => EnterDigit(c));
        }
        if (resetButton != null) resetButton.onClick.AddListener(ResetPassword);
        if (enterButton != null) enterButton.onClick.AddListener(EnterPassword);
    }

    private void SetFieldText(string text)
    {
        if (passwordField != null) passwordField.text = text;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.E)) ShowPanel();
#endif
    }

    public bool SetPassword(string newPassword)
    {
        // int.TryParse fails on passwords longer than 9 digits
        if (string.IsNullOrEmpty(newPassword)) return false;
        foreach (char ch in newPassword)
        {
            if (ch < '0' || ch > '9') return false;
        }

        if (newPassword.Length > passwordMaxLength) return false;

        password = newPassword;
        return true;
    }


    private void ResetPassword()
    {
        _currentAttempt = "";
        SetFieldText(_currentAttempt);

    }

    IEnumerator CloseAfterCorrect()
    {
        yield return new WaitForSeconds(timeAfterCorrect);
        HidePanel();
    }
    public void EnterPassword()
    {
        if (_currentAttempt.Length == 0)
        {
            SetFieldText(_currentAttempt);
        }
        else
        if (_currentAttempt.Equals(password))
        {
            Debug.Log("PASSWORD: "+ _currentAttempt + " IS CORRECT!");
            _isCorrect = true;
            StartCoroutine(CloseAfterCorrect());
            SetFieldText(correctMessege);
        }
        else
        {
            
            Debug.Log("PASSWORD: " + _currentAttempt + " IS WRONG!");
            SetFieldText(wrongMessege);
        }
        _currentAttempt = "";
    }
    private void EnterDigit(int digit)
    {
        if (_currentAttempt.Length < passwordMaxLength)
        {
            _currentAttempt += digit.ToString();
        }
        SetFieldText(_currentAttempt);
    }

    public void ShowPanel()
    {
        if (panel != null) panel.SetActive(true);
        if (bachGround != null) bachGround.SetActive(true);
    }
    public void HidePanel()
    {
        if (panel != null) panel.SetActive(false);
        if (bachGround != null) bachGround.SetActive(false);
    }

    public bool IsCorrect()
    {
        return _isCorrect;
    }
}
