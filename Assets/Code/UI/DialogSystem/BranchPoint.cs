using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BranchPoint
{
    [TextArea]
    public string question;
    public Answer[] answers;
}
