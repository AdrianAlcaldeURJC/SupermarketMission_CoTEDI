using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Canvas scoreboardCanvas;
    [SerializeField] Canvas optionsCanvas;
    [SerializeField] Canvas mainCanvas;

    [SerializeField] Slider MusicSlider;
    [SerializeField] Slider SFXSlider;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.GetInstance().ResetGameManager();
        scoreboardCanvas.gameObject.SetActive(false);
        optionsCanvas.transform.GetChild(0).gameObject.SetActive(false);
        mainCanvas.gameObject.SetActive(true);
        AudioManager.GetInstance().PlayMusicClip(AudioManager.GetInstance().generalMusic);
    }

    public void OnClickPlay()
    {
        ClearAllData();
        AudioManager.GetInstance()?.PlaySFXClip(AudioManager.GetInstance().clickTechButtonSFX);
        GameManager.GetInstance().GoToScene("NameScene");
    }

    public void OnClickScoreboard()
    {
    }

    public void OnClickOptions()
    {
        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickButtonSFX);
        scoreboardCanvas.gameObject.SetActive(false);
        optionsCanvas.transform.GetChild(0).gameObject.SetActive(!optionsCanvas.transform.GetChild(0).gameObject.activeSelf);
        SetSlidersValue();
    }

    public void OnClickExit()
    {
        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickButtonSFX);
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

    private void ClearAllData()
    {
        GameManager.GetInstance().ResetGameManager();
        Destroy(FindObjectOfType<ObstaclesManagerSingleton>(true)?.gameObject);
        Destroy(FindObjectOfType<MazeMovement>(true)?.gameObject);
    }
}
