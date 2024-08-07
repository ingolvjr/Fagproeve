using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LanguageSelectArea : MonoBehaviour
{
    private QuizMasterScript.language _language;

    void OnTriggerEnter(Collider collider)
    {
        var LanguageObject = collider.gameObject.GetComponent<LanguageSelectObject>();
        if (LanguageObject != null)
        {
            _language = LanguageObject.Language;
        }
    }
    public void AttemptLanguageChange() {
        QuizMasterScript.Instance.SetLanguage(_language);
    }
}
