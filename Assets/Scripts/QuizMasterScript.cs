using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class QuizMasterScript : MonoBehaviour
{
    public static QuizMasterScript Instance { get; private set;}
    // event objects subscribe to for resetting position
    public UnityEvent ResetPositionEvent;
    // List of all objects
    [SerializeField] private List<ObjectScript> _objectList;
    // Correct object user must submit to get points
    [SerializeField] private ObjectScript _correctObject;
    // temporary List used to make sure there are no duplicate objects
    private List<ObjectScript> _objectsToRemove;

    //Custom type for selecting language in a user-friendly way
    public enum language
    {
        Norwegian,
        English
    }
    //Variable holding active language and a public readonly variable with active language
    [SerializeField] private language _language;
    [SerializeField] private TMP_Text _billBoardText;
    [SerializeField] private List<string> _finishText;
    public language Language {get => _language;}
    //Reference to the billboard text element
    [SerializeField] private TMP_Text _billBoardText;
    void Awake()
    {
        //making sure there is only one QuizManager in the scene and sets up easy calls to the active QuizManager
         if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
        //Makes a list of all ColorObjects and reverses it since FindObjectsOfType starts at the bottom of the hierarchy
        _objectList.AddRange(FindObjectsOfType<ObjectScript>());
        _objectList.Reverse();
    }

    void Start()
    {
        //puts all duplicate color objects into a separate list for later deletion
        foreach (ObjectScript Object in _objectList)
        {
            foreach (ObjectScript otherObject in _objectList)
                // For each Color object see if there is another one with the same color name and put it into the other list to be deleted unless the current color object is already in the other list
                if ((otherObject != Object) && (otherObject.ColorName[(int) _language] == Object.ColorName[(int) _language]) && (!_objectsToRemove.Contains(Object)))
                    {
                    _objectsToRemove.Add(otherObject);
                    }
        }
        // remove all color objects in this list from the original list and then delete them from the scene
        foreach (ObjectScript Object in _objectsToRemove)
        {
            _objectList.Remove(Object);
            Destroy(Object.gameObject);
        }

        _randomObject();
    }
        //pick a random color object from the list, set it as the correct color object and update the billboard
    private void _randomObject()
    {
        if (!_objectList.Any()) 
        {
            _correctObject = null;
            _billBoardText.text = _finishText[(int)_language];
            return;
        }
        var randomIndex = Random.Range(0, _objectList.Count);
        if (_objectList[randomIndex] == _correctObject && _objectList.Count > 1)
        {
            _randomObject();
            return;
        }
        _correctObject = _objectList[randomIndex];
        _updateBillboard();
    }

    public void SubmitAnswer(ObjectScript Object)
    {
        StartCoroutine(_submitAnswer(Object));
    }
        // changes the billboard text color depending on whether the submitted answer is correct or not, randomly chooses a new correct color object and adds/removes points from the corresponding color object
    private IEnumerator _submitAnswer(ObjectScript Object)
    {
        ResetPositionEvent.Invoke();
        if (Object == _correctObject)
        {
            _correctObject.AddPoint();
            _billBoardText.color = new Color(0, 255, 0, 255);
        }
        else{
            _correctObject.RemovePoint();
            _billBoardText.color = new Color(255, 0, 0, 255);
        }
        if (Object.Points >= 3)
            _objectList.Remove(Object);
        yield return new WaitForSeconds(2f);
        _billBoardText.color = new Color(255, 255, 255, 255);
        _randomObject();
    }

    private void _updateBillboard()
    {
        _billBoardText.text = _correctObject.ColorName[(int) _language];
    }
}
