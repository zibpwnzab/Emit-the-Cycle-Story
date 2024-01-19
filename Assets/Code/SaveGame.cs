using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SaveGame : MonoBehaviour
{
    private GameObject Savedata;

    private void Start()
    {
        if (PlayerPrefs.GetString("SaveGame") != "") 
        {
            Savedata.GetComponent<Text>().text = PlayerPrefs.GetString("SaveGame");
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        PlayerPrefs.SetString("SaveGame", Savedata.GetComponent<Text>().text);
    }
}
