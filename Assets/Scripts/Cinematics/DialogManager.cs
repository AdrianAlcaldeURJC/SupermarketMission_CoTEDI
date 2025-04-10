using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]

public class DialogManager : MonoBehaviour
{
    //Todo: Limpiar todo lo de avanzar rapido el texto porque no se usa

    private TMP_Text textBox;

    //Typewriter functionality
    private int currentCharacterIndex;
    private Coroutine typeWriterCoroutine;
    private bool readyForNewText = true;

    private WaitForSeconds simpleDelay;
    private WaitForSeconds puntuactionDelay;

    [Header("Typewriting Settings")]
    [SerializeField]
    private float charactersPerSecond = 30;
    [SerializeField]
    private float interpuntuactionDelay = 0.5f;

    //Skipping Functionality
    public bool currentlySkipping { get; private set; }
    private WaitForSeconds skipDelay;

    [Header("Skip Options")]
    [SerializeField]
    private bool quickSkip;
    [SerializeField]
    [Min(1)] private int skipSpeedUp = 5;

    // Event Functionality
    private WaitForSeconds textBoxFullEventDelay;
    [SerializeField]
    [Range(0.1f, 0.5f)] private float sendDoneDelay = 0.2f;

    [SerializeField] private StoryBehaviour sb;
    [SerializeField] private LevelIntroLoader introLoader;


    private void Awake()
    {
        textBox = GetComponent<TMP_Text>();
        simpleDelay = new WaitForSeconds(1 / charactersPerSecond);
        puntuactionDelay = new WaitForSeconds(interpuntuactionDelay);

        skipDelay = new WaitForSeconds(1 / (charactersPerSecond * skipSpeedUp));
        textBoxFullEventDelay = new WaitForSeconds(sendDoneDelay);
    }

    // Start is called before the first frame update
    void Start()
    {
        textBox.ForceMeshUpdate();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)|| Input.GetKeyDown("space"))
        {
            if(textBox.maxVisibleCharacters <= textBox.textInfo.characterCount -1)
            {
                Skip();
            }
            else
            {
                if (readyForNewText&&sb!=null)
                {
                    sb.ShowNewText();
                }
                if(readyForNewText&&sb == null&& introLoader != null)
                {
                    introLoader.StartLightTransition();
                }
            }
            
        }
    }

    private void Skip()
    {
        if (!currentlySkipping)
        {
            currentlySkipping = true;
            if (typeWriterCoroutine != null)
                StopCoroutine(typeWriterCoroutine);
            textBox.maxVisibleCharacters = textBox.textInfo.characterCount;
            readyForNewText = true;
            currentlySkipping = false;
        }
    }

    public void SetText(string text)
    {
        textBox.ForceMeshUpdate();

        if (!readyForNewText)
            return;
        readyForNewText = false;

        if (typeWriterCoroutine != null)
            StopCoroutine(typeWriterCoroutine);

        textBox.text = text;
        textBox.maxVisibleCharacters = 0;
        currentCharacterIndex = 0;

        typeWriterCoroutine = StartCoroutine(Typewriter());
    }

    private IEnumerator Typewriter()
    {
        TMP_TextInfo textInfo = textBox.textInfo;

        while (currentCharacterIndex < textInfo.characterCount)
        {
            var lastCharacterIndex = textInfo.characterCount - 1;
            //Es el fianl de la frase, y hace la pausa de final y para la ejecucion de la funcion
            if(currentCharacterIndex == lastCharacterIndex)
            {
                textBox.maxVisibleCharacters++;
                yield return textBoxFullEventDelay;
                readyForNewText = true;
                yield break;
            }

            char character = textInfo.characterInfo[currentCharacterIndex].character;
            textBox.maxVisibleCharacters++;

            if (!currentlySkipping && (character == '?' || character == '.' || character == ',' || character == ':' || character == ';' || character == '!' || character == '-'))
            {
                yield return puntuactionDelay;
            }
            else
            {
                yield return currentlySkipping? skipDelay: simpleDelay;
            }

            currentCharacterIndex++; 
            
        }
    }
}
