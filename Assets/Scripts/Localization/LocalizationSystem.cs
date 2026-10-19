using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using TMPro;

public class LocalizationSystem : MonoBehaviour
{
    [SerializeField] UnityEngine.Localization.Settings.LocalizationSettings settings;
    UnityEngine.Localization.Settings.LocalizedStringDatabase stringDatabase;
    // Created here: AddLocalizationJob can be called before Start
    static readonly Dictionary<TMP_Text, (string key, string[] args)> activeJobs = new();
    static readonly Queue<(TMP_Text text, string key, string[] args)> queueJobs = new();
    public static LocalizationSystem instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (settings == null) return;
        stringDatabase = settings.GetStringDatabase();
        settings.OnSelectedLocaleChanged += ChangeLanguage;
    }

    private void OnDestroy()
    {
        if (settings != null) settings.OnSelectedLocaleChanged -= ChangeLanguage;
        if (instance == this) instance = null;
    }

    public static void AddLocalizationJob(TMP_Text text, string key, params string[] args)
    {
        queueJobs.Enqueue((text, key, args));
    }


    public void Update()
    {
        if (queueJobs.Count == 0 || stringDatabase == null)
            return;
        var job = queueJobs.Dequeue();
        if (!job.text)
            return;

        activeJobs[job.text] = (job.key, job.args);

        ExecuteJob(job.text, job.key, job.args);
    }

    private void ExecuteJob(TMP_Text field, string key, string[] args)
    {
        string text = stringDatabase.GetLocalizedString(key);
        if (args != null && args.Length > 0)
        {
            text = string.Format(text, args);
        }
        if (field)
            field.text = text;
    }

    public void ChangeLanguage(Locale locale)
    {
        // Skip texts destroyed together with their scene
        var deadTexts = new List<TMP_Text>();
        foreach (var job in activeJobs)
        {
            if (!job.Key)
            {
                deadTexts.Add(job.Key);
                continue;
            }
            queueJobs.Enqueue((job.Key, job.Value.key, job.Value.args));
        }

        foreach (var text in deadTexts)
        {
            activeJobs.Remove(text);
        }
    }
}
