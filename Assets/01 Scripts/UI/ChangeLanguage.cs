using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class ChangeLanguage : MonoBehaviour
{
    public Locale locale;

    public void SetNewLocale()
    {
        LocalizationSettings.SelectedLocale = locale;
    }
}
