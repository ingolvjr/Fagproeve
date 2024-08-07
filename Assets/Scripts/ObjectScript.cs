using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class ObjectScript : MonoBehaviour
{
    [SerializeField]private int _points;
    public int Points {get {return _points;}}
    [SerializeField]private Color color;
    //string of color names in different languages
    public List<string> ColorName;
    [SerializeField] private List<TMP_Text> Counters;
    [SerializeField] private Vector3 _originalPosition;

    void Start()
    {
        GetComponent<MeshRenderer>().material.color = color;
        //adds the visual point counters to a list
        Counters.AddRange(GetComponentsInChildren<TMP_Text>());
        //Stores the objects initial position
        _originalPosition = gameObject.transform.position;
        QuizMasterScript.Instance.ResetPositionEvent.AddListener(_resetPosition);
    }

    public void AddPoint()
    {
        _points++;
        UpdateCanvases();
    }
    public void RemovePoint()
    {
        if (_points > 0 && _points < 3)
            _points--;
        UpdateCanvases();
    }

    private void UpdateCanvases()
    {
        foreach (TMP_Text Counter in Counters)
            Counter.text = _points.ToString();
    }

    private void _resetPosition()
    {
        //very complicated logic to reset the objects position, rotation and velocity
        GetComponent<XRGrabInteractable>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        gameObject.transform.position = _originalPosition;
        transform.rotation = Quaternion.identity;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<XRGrabInteractable>().enabled = true;
    }
}
