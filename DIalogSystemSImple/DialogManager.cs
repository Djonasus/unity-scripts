using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class DialogManager : MonoBehaviour
{
    public Text characterNameText;
    public Text dialogText;
    public Image characterPortrait;
    public Image backgroundImage;
    public float letterTypingDelay = 0.05f;
    public string[] lines;
    public int letterIndex;


    void Start()
    {
        // Инициализируйте текущий диалог и все диалоги здесь (например, из вашего ресурса данных)
        // Например, allDialogues = LoadDialogues();
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            if (dialogText.text == lines[letterIndex]){
                NextLine();
            } else {
                StopAllCoroutines();
                dialogText.text = lines[letterIndex];
            }
        }
    }
    public void StartDialogue(){
        letterIndex = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine() {
        foreach (char c in lines[letterIndex].ToCharArray()){
            dialogText.text += c;
            yield return new WaitForSeconds(letterTypingDelay);
        }
    }

    void NextLine(){
        if (letterIndex < lines.Length-1){
            letterIndex++;
            dialogText.text = "";
            StartCoroutine(TypeLine());
        } else {
            gameObject.SetActive(false);
        }
    }
}