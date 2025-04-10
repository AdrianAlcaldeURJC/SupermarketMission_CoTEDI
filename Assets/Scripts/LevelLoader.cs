using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    public Animator transition;
    public float transitionTime = 1;

    public void LoadNextLevel(string nameScene)
    {
        StartCoroutine(LoadLevel(nameScene));
    }

    IEnumerator LoadLevel(string scene)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(scene);
    }

    public void StartFakeTransition()
    {
        StartCoroutine(FakeLoading());
    }
    IEnumerator FakeLoading()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        transition.SetTrigger("End");
    }

}
