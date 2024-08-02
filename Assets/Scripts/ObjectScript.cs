using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectScript : MonoBehaviour
{
    [SerializeField]private int _points;
    public int Points {get {return _points;}}
    [SerializeField]private Color color;
    public List<string> ColorName;
    [SerializeField] private List<TMP_Text> Counters;
    [SerializeField] private Vector3 _originalPosition;

    void Start()
    {
        GetComponent<MeshRenderer>().material.color = color;
        Counters.AddRange(GetComponentsInChildren<TMP_Text>());
        _originalPosition = gameObject.transform.position;
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
    private void ResetPosition()
    {
        gameObject.transform.position = _originalPosition;
    }
}
