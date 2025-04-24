using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryBehaviour : MonoBehaviour
{
    [SerializeField]
    private Canvas storyCanvas;
    //[SerializeField]
    //private Canvas nameCanvas;
    [SerializeField]
    private TMP_InputField nameInput;
    [SerializeField]
    private DialogManager dialogM;

    private string[] lines;

    private int index = 0;
    private int numLines = 5;

    private bool changeInProgress;

    private LevelLoader levelLoader;
    private LevelIntroLoader levelIntroLoader;

    [SerializeField] private Animator momAnimator;

    [Header("StartCinematic")]
    //[SerializeField] private Animator momAnimator;

    [Header("EndCinematic")]
    [SerializeField] private Image image;
    [SerializeField] private Image imageCharacter;
    [SerializeField] private Sprite imageGarden;
    //[SerializeField] private Sprite imageMum;

    private AgentDataCollector dataCollector;

    [SerializeField] private LocalizedString[] m_localizedStrings;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.GetInstance().PlayMusicClip(AudioManager.GetInstance().cinematicsMusic);

        changeInProgress = false;
        storyCanvas.gameObject.SetActive(false);

        levelLoader = FindObjectOfType<LevelLoader>();
        levelIntroLoader = FindObjectOfType<LevelIntroLoader>();

        switch (SceneManager.GetActiveScene().name)
        {
            case "StartingCinematic":
                //nameCanvas.gameObject.SetActive(true);
                momAnimator.gameObject.SetActive(true);
                //dataCollector = FindObjectOfType<AgentDataCollector>();
                SetLines();
                StartCoroutine(showIntroMessage());
                break;
            case "FinalCinematic":
                momAnimator.gameObject.SetActive(false);

                SetLines();
                StartCoroutine(showIntroMessage());
                break;
            default:
                break;
        }

        ////levelLoader.StartScene();
    }

    // Update is called once per frame
    void Update()
    {
        if (storyCanvas.isActiveAndEnabled)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                //Skip story
                if (SceneManager.GetActiveScene().name == "StartingCinematic")
                {
                    //Skip story
                    levelLoader.LoadNextLevel("GroceryList");
                }
                else
                {
                    levelLoader.LoadNextLevel("MainMenu");
                }
            }
        }
    }

    IEnumerator showIntroMessage()
    {
        levelIntroLoader.SetIntroText();
        levelIntroLoader.StartDarkTransition();
        yield return new WaitForSeconds(1f);
        //if(nameCanvas)
        //    nameCanvas.gameObject.SetActive(false);
        storyCanvas.gameObject.SetActive(true);
        dialogM.SetText(lines[0]);
    }

    void SetLines()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "StartingCinematic":
                numLines = 5;
                lines = new string[numLines];
                lines[0] = m_localizedStrings[0].GetLocalizedString();
                lines[1] = m_localizedStrings[1].GetLocalizedString();
                lines[2] = m_localizedStrings[2].GetLocalizedString();
                lines[3] = m_localizedStrings[3].GetLocalizedString();
                lines[4] = m_localizedStrings[4].GetLocalizedString();
                break;
            case "FinalCinematic":
                numLines = 6;
                lines = new string[numLines];
                lines[0] = m_localizedStrings[5].GetLocalizedString();
                lines[1] = m_localizedStrings[6].GetLocalizedString();
                lines[2] = m_localizedStrings[7].GetLocalizedString();
                lines[3] = m_localizedStrings[8].GetLocalizedString();
                lines[4] = CalculateScore();
                //Dependiendo estado de los alimentos sacar un dialogo distinto
                lines[5] = CalculateTrolleyScore();
                break;
            default:
                lines[0] = "[Dialogo no definido]";
                break;
        }
        
    }

    string CalculateScore()
    {
        string time = TimeSpan.FromSeconds(GameManager.GetInstance().currentSpentTime).ToString(@"mm\:ss\:ff");
        int numPickedItems = GameManager.GetInstance().pickedListItems;
        int totalItems = GameManager.GetInstance().bakeryFoodList.Count + GameManager.GetInstance().fruitFoodList.Count + GameManager.GetInstance().legumeFoodList.Count +
            GameManager.GetInstance().fridgeFoodList.Count + GameManager.GetInstance().fishFoodList.Count + GameManager.GetInstance().perfumeryFoodList.Count;

        string o = m_localizedStrings[9].GetLocalizedString(time, numPickedItems, totalItems, GameManager.GetInstance().numWrongPickedItems);
        return o;
    }

    string CalculateTrolleyScore()
    {
        GameManager.GetInstance().EvaluateFinalTrolley();
        string line;
        int wrongPosition = GameManager.GetInstance().numElementsWrongPositionTrolley;
        int moderatePosition = GameManager.GetInstance().numElementsModeratePositionTrolley;
        if(wrongPosition == 0 && moderatePosition == 0)
        {
            line = m_localizedStrings[10].GetLocalizedString();
        }
        else if(wrongPosition==0&&moderatePosition<4)
        {
            line = m_localizedStrings[11].GetLocalizedString();
        }
        else if (wrongPosition == 0 && moderatePosition >= 4)
        {
            line = m_localizedStrings[12].GetLocalizedString();
        }
        else
        {
            line = m_localizedStrings[13].GetLocalizedString();
        }
        return line;
    }

    //void completedLine()
    //{
    //    DialogManager.CompleteTextRevealed += showNewText;
    //}

    public void ShowNewText()
    {
        if (!changeInProgress)
        {
            Debug.Log("Next line");
            if (index < lines.Length - 1)
            {
                index++;
                dialogM.SetText(lines[index]);
                ChangeStoryImage();

            }
            else
            {
                if (index == lines.Length - 1)
                {
                    if(SceneManager.GetActiveScene().name == "StartingCinematic")
                    {
                        //Skip story
                        levelLoader.LoadNextLevel("GroceryList");
                    }
                    else
                    {
                        GameManager.GetInstance().SendResultToDB();
                        levelLoader.LoadNextLevel("MainMenu");
                    }

                }
            }
        }
    }

    private void ChangeStoryImage()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "StartingCinematic":
                momAnimator.SetTrigger("Talk");
                break;
            case "FinalCinematic":
                if (index == 1)
                {
                    StartCoroutine(changeBackground());
                }
                momAnimator.SetTrigger("Talk");
                break;
            default:
                break;
        }
    }

    IEnumerator changeBackground()
    {
        changeInProgress = true;
        yield return new WaitForSeconds(2f);
        levelLoader.StartFakeTransition();
        yield return new WaitForSeconds(1f);
        changeInProgress = false;
        image.sprite = imageGarden;
        //imageCharacter.sprite = imageMum;
        momAnimator.gameObject.SetActive(true);
        ShowNewText();
        
    }


}
