using UnityEngine;

public class Lock : ISignal, IInteractable
{
    // Locks sharing one LockUI also share its password and state
    [SerializeField] LockUI lockUI;

    // Lazy: LevelManager2.Start may call SetPassword before Start
    private LockUI UI
    {
        get
        {
            if (lockUI == null) lockUI = FindFirstObjectByType<LockUI>();
            return lockUI;
        }
    }

    public bool Interact(GameObject gameObject, Animator animator)
    {
        if (UI == null) return false;
        if (UI.IsCorrect())
            return true;
        UI.ShowPanel();
        return true;
    }

    public bool SetPassword(string password)
    {
        return UI != null && UI.SetPassword(password);
    }

    override public bool Signal()
    {
        return UI != null && UI.IsCorrect();
    }

    public bool StopInteraction(GameObject gameObject, Animator animator)
    {
        if (UI != null) UI.HidePanel();
        return false;
    }
}
