using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DialogueSection
{
    [TextArea]
    public string[] dialogue;
    public bool endAfterDialogue;
    public BranchPoint branchPoint;
}
