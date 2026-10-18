using UnityEngine;
using UnityEngine.UI;

public class SaveGame : MonoBehaviour
{
    public const string SavePrefsKey = "SaveGame";

    [SerializeField] private Text saveDataText;

    private void Start()
    {
        if (saveDataText == null)
        {
            saveDataText = GetComponent<Text>();
        }

        string savedValue = PlayerPrefs.GetString(SavePrefsKey, string.Empty);
        if (!string.IsNullOrEmpty(savedValue) && saveDataText != null)
        {
            saveDataText.text = savedValue;
        }
    }

    public void SaveCurrentText()
    {
        if (saveDataText != null)
        {
            PlayerPrefs.SetString(SavePrefsKey, saveDataText.text);
            PlayerPrefs.Save();
        }
    }
}