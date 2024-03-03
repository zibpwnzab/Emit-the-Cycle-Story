using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Locks info")]
    [SerializeField] List<Lock> locks;
    [SerializeField] int passwordLength;
    public Dictionary<int, string> passwords;
    void Start()
    {
        SetRandomPasswords();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetRandomPasswords()
    {
        if (locks == null) return;
        if (locks.Count == 0) return;
        passwords = new();
        for (int i = 0; i < locks.Count; i++)
        {
            string password = "";
            for (int j = 0; j < passwordLength; j++)
                password += Random.Range(0, 10).ToString();
            passwords.Add(i, password);
            locks[i].SetPassword(password);
            Debug.Log("Password:" + password);
        }
    }
}
