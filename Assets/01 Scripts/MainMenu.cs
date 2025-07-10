using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Canvas scorebardCanvas;
    [SerializeField] Canvas optionsCanvas;
    [SerializeField] Canvas mainCanvas;

    [SerializeField] Slider MusicSlider;
    [SerializeField] Slider SFXSlider;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.GetInstance().ResetGameManager();
        scorebardCanvas.gameObject.SetActive(false);
        optionsCanvas.gameObject.SetActive(false);
        mainCanvas.gameObject.SetActive(true);
        AudioManager.GetInstance().PlayMusicClip(AudioManager.GetInstance().generalMusic);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickPlay()
    {
        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickTechButtonSFX);
        GameManager.GetInstance().GoToScene("NameScene");
    }

    public void OnClickScoreboard()
    {
        Debug.Log("Clicked Scoreboard");
    }

    public void OnClickOptions()
    {
        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickButtonSFX);
        Debug.Log("Clicked Options");
        scorebardCanvas.gameObject.SetActive(false);
        optionsCanvas.gameObject.SetActive(!optionsCanvas.gameObject.activeSelf);
        SetSlidersValue();
    }

    public void OnClickExit()
    {
        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickButtonSFX);
        Debug.Log("Clicked Exit");
        Application.Quit();
    }

    public void OnClickDaltonic()
    {
        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickButtonSFX);
        GameManager.GetInstance().daltonicUser = !GameManager.GetInstance().daltonicUser;
    }

    private void SetSlidersValue()
    {
        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickButtonSFX);
        MusicSlider.value = GameManager.GetInstance().musicVolume;
        SFXSlider.value = GameManager.GetInstance().SFXVolume;
    }
}
