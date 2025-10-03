using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AgentDataCollector : MonoBehaviour
{

    [SerializeField] private TMP_InputField nameInputA;
    [SerializeField] private TMP_InputField nameInputB;
    [SerializeField] private TMP_Text ageTextA;
    [SerializeField] private TMP_Text ageTextB;
    [SerializeField] private Image boySelectedImageA;
    [SerializeField] private Image girlSelectedImageA;
    [SerializeField] private Image boySelectedImageB;
    [SerializeField] private Image girlSelectedImageB;

    private int age = 7;
    private string genderA = "";
    private string genderB = "";

    private LevelLoader lvlLoader;
    private bool updateSession = true;
    private bool firstPlayerRegistered = false;

    void Start()
    {
        nameInputA.text = "";
        nameInputB.text = "";
        ageTextB.text = age.ToString();
        girlSelectedImageA.gameObject.SetActive(false);
        boySelectedImageA.gameObject.SetActive(false);
        lvlLoader = FindObjectOfType<LevelLoader>();
    }

    public void IncrementAge()
    {
        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickButtonSFX);
        age++;

        if (!firstPlayerRegistered)
        {
            ageTextA.text = age.ToString();
        }
        else
        {
            ageTextB.text = age.ToString();
        }
    }

    public void DecrementAge()
    {
        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickButtonSFX);
        age--;
        if (age < 1)
            age = 1;

        if (!firstPlayerRegistered)
        {
            ageTextA.text = age.ToString();
        }
        else
        {
            ageTextB.text = age.ToString();
        }

    }

    public void SetGender(string value)
    {
        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickButtonSFX);

        Image girl, boy;

        if (!firstPlayerRegistered)
        {
            girl = girlSelectedImageA;
            boy = boySelectedImageA;
        }
        else
        {
            girl = girlSelectedImageB;
            boy = boySelectedImageB;
        }

        if (value == "Femenino")
        {
            girl.gameObject.SetActive(true);
            boy.gameObject.SetActive(false);

        }
        else
        {
            girl.gameObject.SetActive(false);
            boy.gameObject.SetActive(true);
        }

        if (!firstPlayerRegistered)
        {
            genderA = value;
        }
        else
        {
            genderB = value;
        }

    }

    public bool CheckAndSaveAgentData()
    {
        if (genderB == "" || nameInputB.text == "")
        {
            return false;
        }
        else
        {
            DataStorage.UserData userData = DataStorage.Instance.userData;
            userData.Name = nameInputA.text;
            userData.UserAux1 = nameInputB.text;
            DataStorage.Instance.userData.SetData(nameInputA.text, int.Parse(ageTextA.text), genderA, nameInputB.text, ageTextB.text);

            DataStorage.Instance.sessionData.SessionAux1 = genderB;

            GameManager.GetInstance().playerNameA = nameInputA.text;
            GameManager.GetInstance().playerNameB = nameInputB.text;

            if (updateSession)
            {
                GameManager.GetInstance().UpdateSessionID();
                updateSession = false;
            }

            return true;
        }
    }

    private bool NextPlayer()
    {
        if (firstPlayerRegistered)
            return false;

        if (genderA == "" || nameInputA.text == "")
            return true;

        nameInputA.transform.parent.gameObject.SetActive(false);
        ageTextA.transform.parent.gameObject.SetActive(false);
        boySelectedImageA.transform.parent.parent.gameObject.SetActive(false);

        nameInputB.transform.parent.gameObject.SetActive(true);
        ageTextB.transform.parent.gameObject.SetActive(true);
        boySelectedImageB.transform.parent.parent.gameObject.SetActive(true);

        ageTextB.text = age.ToString();

        firstPlayerRegistered = true;
        return true;
    }

    public void StartCinematic()
    {
        if (NextPlayer())
            return;

        if (CheckAndSaveAgentData())
        {
            AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().clickTechButtonSFX);
            lvlLoader.LoadNextLevel("StartingCinematic");
        }
    }
}
