using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveAreaScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        var Object = other.gameObject.GetComponent<ObjectScript>();

        if (Object != null)
            QuizMasterScript.Instance.SubmitAnswer(Object);
    }
}
