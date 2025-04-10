using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public void SetTextChecking(int index, string newText)
    {
        if (GameManager.GetInstance().firstTimeScene[index])
        {
            text.text = newText;
            GameManager.GetInstance().firstTimeScene[index] = false;
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
