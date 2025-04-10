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
    private Image chicoSelectedImage;
    [SerializeField]
    private Image chicaSelectedImage;

    private int age = 7;
    private string gender = "";

    private LevelLoader lvlLoader;

    // Start is called before the first frame update
    void Start()
    {
        nameInput.text = "";
        ageText.text = age.ToString();
        chicaSelectedImage.gameObject.SetActive(false);
        chicoSelectedImage.gameObject.SetActive(false);
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
            chicaSelectedImage.gameObject.SetActive(true);
            chicoSelectedImage.gameObject.SetActive(false);

        }
        else
        {
            chicaSelectedImage.gameObject.SetActive(false);
            chicoSelectedImage.gameObject.SetActive(true);
        }
        gender = value;
    }

    public bool CheckAndSaveAgentData()
    {
        if (nameInput.text != "" && gender != "")
        {
            GameManager.GetInstance().SavePlayerData(nameInput.text, age, gender);
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
