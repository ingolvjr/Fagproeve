using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class QuizMasterScript : MonoBehaviour
{
    public static QuizMasterScript Instance { get; private set;}
    public UnityEvent ResetPositionEvent;
    [SerializeField] private List<ObjectScript> _objectList;
    [SerializeField] private ObjectScript _correctObject;
    private List<ObjectScript> _objectsToRemove = new List<ObjectScript>();
    public List<string> Colors;
    public enum language
    {
        Norwegian,
        English
    }
    [SerializeField] private language _language;
    [SerializeField] private TMP_Text _billBoardText;
    public language Language {get => _language;}
    void Awake()
    {
         if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);

        _objectList.AddRange(FindObjectsOfType<ObjectScript>());
        _objectList.Reverse();
    }

    void Start()
    {
        foreach (ObjectScript Object in _objectList)
        {
            foreach (ObjectScript otherObject in _objectList)
                if ((otherObject != Object) && (otherObject.ColorName[(int) _language] == Object.ColorName[(int) _language]) && (!_objectsToRemove.Contains(Object)))
                    {
                    _objectsToRemove.Add(otherObject);
                    }
        }
        
        foreach (ObjectScript Object in _objectsToRemove)
        {
            _objectList.Remove(Object);
            Destroy(Object.gameObject);
        }
        
        foreach (ObjectScript Object in _objectList)
        Colors.Add(Object.ColorName[(int) _language]);

        _randomObject();
    }

    private void _randomObject()
    {
        if (!_objectList.Any())
            return;
        var randomIndex = Random.Range(0, _objectList.Count);
        if (_objectList[randomIndex] == _correctObject)
        {
            _randomObject();
            return;
        }
        _correctObject = _objectList[randomIndex];
        _updateBillboard();
    }

    public void SubmitAnswer(ObjectScript Object)
    {
        if (Object == _correctObject)
            _correctObject.AddPoint();
        else
            _correctObject.RemovePoint();
    
        if (Object.Points >= 3)
            _objectList.Remove(Object);

        _randomObject();
        ResetPositionEvent.Invoke();
    }

    private void _updateBillboard()
    {
        _billBoardText.text = _correctObject.ColorName[(int) _language];
    }
}
