using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LanguageSelectObject : MonoBehaviour
{
    public QuizMasterScript.language Language;

    void Start()
    {
        gameObject.GetComponentInChildren<TMP_Text>().text = Language.ToString();
    }
}
