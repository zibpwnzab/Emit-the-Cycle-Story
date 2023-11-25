using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour, IInteractable
{
    [SerializeField] LockUI lockUI;
    public bool Interact(GameObject gameObject, Animator animator)
    {
        lockUI.ShowPanel();
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (lockUI == null) lockUI = FindObjectOfType<LockUI>();
    }

    // Update is called once per frame
    public bool SetPassword(string password)
    {
        return lockUI.SetPassword(password);
    }
}
