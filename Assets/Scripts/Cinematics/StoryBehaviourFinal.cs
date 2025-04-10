using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryBehaviourFinal : MonoBehaviour
{
    [SerializeField]
    private Canvas storyCanvas;
    [SerializeField]
    private DialogManager dialogM;

    private string[] lines;

    private int index = 0;
    private int numLines = 5;

    private LevelLoader levelLoader;
    private LevelIntroLoader levelIntroLoader;


    // Start is called before the first frame update
    void Start()
    {
        storyCanvas.gameObject.SetActive(false);

        levelLoader = FindObjectOfType<LevelLoader>();
        levelIntroLoader = FindObjectOfType<LevelIntroLoader>();
        ////levelLoader.StartScene();
        OnClickEnterName();
    }

    // Update is called once per frame
    void Update()
    {
        if (storyCanvas.isActiveAndEnabled)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                //Skip story
                levelLoader.LoadNextLevel("MainMenu");
            }
        }
    }

    public void OnClickEnterName()
    {
        //Todo: Check is name Input is empty
        SetLines();
        //levelLoader.StartTransition();
        StartCoroutine(showIntroMessage());
        //Todo: start comic animation
    }

    IEnumerator showIntroMessage()
    {
        levelIntroLoader.StartDarkTransition();
        yield return new WaitForSeconds(1f);
        storyCanvas.gameObject.SetActive(true);
        dialogM.SetText(lines[0]);
    }

    void SetLines()
    {
        lines = new string[numLines];
        lines[0] = "¡" + GameManager.GetInstance().playerName + "! ¿Puedes venir un momento, por favor?";
        lines[1] = "Aquí estás. Pues verás, tengo una misión para ti.";
        lines[2] = "Tu padre iba ponerse a hacer la comida, pero al parecer nos faltan muchos ingredientes y tenemos un poco de prisa.";
        lines[3] = "¿Crees que podrías conseguirlos todos tu solo, agente " + GameManager.GetInstance().playerInitial + "? ";
        lines[4] = "¿Si? Te veo decidido. Pues aquí tienes la lista. Prepárate para la misión.";
    }

    //void completedLine()
    //{
    //    DialogManager.CompleteTextRevealed += showNewText;
    //}

    public void showNewText()
    {
        Debug.Log("Next line");
        if (index < lines.Length - 1)
        {
            index++;
            dialogM.SetText(lines[index]);
        }
        else
        {
            if(index == lines.Length - 1)
            {
                //Skip story
                GameManager.GetInstance().GoToScene("GroceryList");
            }
        }
    }


}
