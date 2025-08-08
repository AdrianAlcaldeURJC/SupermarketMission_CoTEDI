using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AgentDataCollector : MonoBehaviour
{

    [SerializeField]
    private TMP_InputField nameInput;
    [SerializeField]
    private TMP_Text ageText;
    [SerializeField]
    private Image boySelectedImage;
    [SerializeField]
    private Image girlSelectedImage;

    private int age = 7;
    private string gender = "";

    private LevelLoader lvlLoader;
    private bool updateSession = true;

    void Start()
    {
        nameInput.text = "";
        ageText.text = age.ToString();
        girlSelectedImage.gameObject.SetActive(false);
        boySelectedImage.gameObject.SetActive(false);
        lvlLoader = FindObjectOfType<LevelLoader>();
    }

    public void IncrementAge()
    {
        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickButtonSFX);
        age++;
        ageText.text = age.ToString();
    }

    public void DecrementAge()
    {
        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickButtonSFX);
        age--;
        if (age < 1)
            age = 1;
        ageText.text = age.ToString();
    }

    public void SetGender(string value)
    {
        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickButtonSFX);

        if (value == "Femenino")
        {
            girlSelectedImage.gameObject.SetActive(true);
            boySelectedImage.gameObject.SetActive(false);

        }
        else
        {
            girlSelectedImage.gameObject.SetActive(false);
            boySelectedImage.gameObject.SetActive(true);
        }
        gender = value;
    }

    public bool CheckAndSaveAgentData()
    {
        if (nameInput.text != "" && gender != "")
        {
            DataStorage.UserData userData = DataStorage.Instance.userData;
            userData.Name = nameInput.text;

            DataStorage.Instance.userData.setData(nameInput.text, age, gender);
            if(updateSession)
            {
                GameManager.GetInstance().UpdateSessionID();
                updateSession = false;
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    public void StartCinematic()
    {
        if (CheckAndSaveAgentData())
        {
            AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickTechButtonSFX);

            lvlLoader.LoadNextLevel("StartingCinematic");
        }
    }
}
