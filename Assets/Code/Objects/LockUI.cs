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

    [SerializeField]  private string password;
    private string _currentAttempt;
    private bool _isCorrect = false;
    void Start()
    {
        _currentAttempt = "";
        for (int i = 0; i < numbers.Count; i++)
        {
            int c = 0 + i;
            numbers[i].onClick.AddListener(() => EnterDigit(c));
        }
        resetButton.onClick.AddListener(ResetPassword);
        enterButton.onClick.AddListener(EnterPassword);
    }

    // Update is called once per frame
    void Update()
    {
        passwordField.text = _currentAttempt;
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.E)) ShowPanel();
#endif
    }

    public bool SetPassword(string newPassword)
    {
        int b;
        if (!int.TryParse(newPassword.ToString(), out b)) return false;
        
        if (newPassword.Length > passwordMaxLength) return false;

        password = newPassword;

        if (newPassword.Length == passwordMaxLength) return true;

        return true;
        while (password.Length < passwordMaxLength)
        {
            password = "0" + password;
        }
        return true;
    }


    private void ResetPassword()
    {
        _currentAttempt = "";
    }

    public void EnterPassword()
    {
        if (_currentAttempt.Equals(password))
        {
            Debug.Log("PASSWORD: "+ _currentAttempt + " IS CORRECT!");
            _isCorrect = true;
        }
        else
        {
            
            Debug.Log("PASSWORD: " + _currentAttempt + " IS WRONG!");
        }
        _currentAttempt = "";
    }
    private void EnterDigit(int digit)
    {
        if (_currentAttempt.Length < passwordMaxLength)
        {
            _currentAttempt += digit.ToString();
        }
    }

    public void ShowPanel()
    {
        panel.SetActive(true);
        bachGround.SetActive(true);
    }
    public void HidePanel()
    {
        panel.SetActive(false);
        bachGround.SetActive(false);
    }

    public bool IsCorrect()
    {
        return _isCorrect;
    }
}
