using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using TMPro;

public class LocalizationSystem : MonoBehaviour
{
    [SerializeField] UnityEngine.Localization.Settings.LocalizationSettings settings;
    UnityEngine.Localization.Settings.LocalizedStringDatabase stringDatabase;
    static Dictionary<TMP_Text, (string key, string[] args)> activeJobs;
    static Queue<(TMP_Text text, string key, string[] args)> queueJobs;
    public static LocalizationSystem instance;
    private void Start()
    {
        activeJobs = new();
        queueJobs = new();
        stringDatabase = settings.GetStringDatabase();
        settings.OnSelectedLocaleChanged += ChangeLanguage;
    }

    public static void AddLocalizationJob(TMP_Text text, string key, params string[] args)
    {
        queueJobs.Enqueue((text, key, args));
    }


    public void Update()
    {
        if (queueJobs.Count == 0)
            return;
        var job = queueJobs.Dequeue();

        if (!activeJobs.ContainsKey(job.text))
            activeJobs.Add(job.text, (job.key, job.args));
        else activeJobs[job.text] = (job.key, job.args);

        ExecuteJob(job.text, job.key, job.args);
    }

    private void ExecuteJob(TMP_Text field, string key, string[] args)
    {
        string text = stringDatabase.GetLocalizedString(key);
        if (args.Length > 0)
        {
            text = string.Format(text, args);
            Debug.Log(text);
        }
        if (field)
            field.text = text;
    }

    public void ChangeLanguage(Locale locale)
    {
        foreach (var job in activeJobs)
        {
            queueJobs.Enqueue((job.Key, job.Value.key, job.Value.args));
        }
    }
}
