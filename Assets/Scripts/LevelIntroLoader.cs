using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelIntroLoader : MonoBehaviour
{

    public Animator transition;
    public float transitionTime = 1;

    //Intro text
    //public string introSceneText;
    [SerializeField] TMP_Text introText;

    private string text;

    [SerializeField]
    private DialogManager dialogM;

    public void SetIntroText()
    {
        //Definir frases segun nombre escena
        switch (SceneManager.GetActiveScene().name)
        {
            case "StartingCinematic":
                text = "¡" + GameManager.GetInstance().playerName + "! ¿Puedes venir un momento, por favor ? ";
                break;
            case "FinalCinematic":
                //transition.SetTrigger("ForceBlack");
                Debug.Log("blaaaaaack");
                //StartDarkTransition();
                text = "Uff, por fin en la caja.";
                break;
            default: 
                break;
        }
       
    }

    public void StartDarkTransition()
    {
        Debug.Log("Oscurecer");
        StartCoroutine(showIntrotext());
    }

    IEnumerator showIntrotext()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        dialogM.SetText(text);
    }

    public void StartLightTransition()
    {
        Debug.Log("Aclarar");
        StartCoroutine(hideIntrotext());
    }

    IEnumerator hideIntrotext()
    {
        transition.SetTrigger("End");
        yield return new WaitForSeconds(transitionTime);
        //this.gameObject.SetActive(false);
    }
}
