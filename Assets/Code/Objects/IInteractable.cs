using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public bool Interact(GameObject gameObject, Animator animator);
    public bool StopInteraction(GameObject gameObject, Animator animator);
}
