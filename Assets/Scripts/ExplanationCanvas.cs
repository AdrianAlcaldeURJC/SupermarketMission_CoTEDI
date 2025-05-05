using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Components;
using TMPro;

public class ExplanationCanvas : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    public void Close()
    {
        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickTechButtonSFX);
        this.gameObject.SetActive(false);
        EventManager.OnTimerStart();
    }

    public void AutoClose()
    {
        this.gameObject.SetActive(false);
        EventManager.OnTimerStart();
    }

    public void SetTextChecking(string i_localizationTable, string i_localizationKey, int i_index)
    {
        if (GameManager.GetInstance().firstTimeScene[i_index])
        {
            LocalizeStringEvent strEvent    = text.gameObject.GetComponent<LocalizeStringEvent>();

            strEvent.OnUpdateString.RemoveAllListeners();
            GameManager.GetInstance().UpdateTMPtoLocalization(strEvent, text, i_localizationTable, i_localizationKey, true);
            GameManager.GetInstance().firstTimeScene[i_index] = false;
        }
        else
        {
            this.AutoClose();
        }
    }

    public void SetText(string newText)
    {
        text.text = newText;
    }
}
