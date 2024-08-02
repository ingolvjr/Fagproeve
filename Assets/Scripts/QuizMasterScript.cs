using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuizMasterScript : MonoBehaviour
{
    public static QuizMasterScript Instance { get; private set;}
    [SerializeField] private List<ObjectScript> ObjectList;
    [SerializeField] private ObjectScript _correctObject;
    private List<ObjectScript> ObjectsToRemove = new List<ObjectScript>();
    public List<string> Colors;
    public enum language
    {
        Norwegian,
        English
    }
    [SerializeField] private language _language;
    public language Language {get => _language;}
    void Awake()
    {
         if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);

        ObjectList.AddRange(FindObjectsOfType<ObjectScript>());
        ObjectList.Reverse();
    }

    void Start()
    {
        foreach (ObjectScript Object in ObjectList)
        {
            foreach (ObjectScript otherObject in ObjectList)
                if ((otherObject != Object) && (otherObject.ColorName[1] == Object.ColorName[1]) && (!ObjectsToRemove.Contains(Object)))
                    {
                    ObjectsToRemove.Add(otherObject);
                    }
        }
        
        foreach (ObjectScript Object in ObjectsToRemove)
        {
            ObjectList.Remove(Object);
            Destroy(Object.gameObject);
        }
        
        foreach (ObjectScript Object in ObjectList)
        Colors.Add(Object.ColorName[(int) _language]);

        RandomObject();
    }

    private void RandomObject()
    {
        if (!ObjectList.Any())
            return;
        var randomIndex = Random.Range(0, ObjectList.Count);
        _correctObject = ObjectList[randomIndex];

    }

    public void SubmitAnswer(ObjectScript Object)
    {
        if (Object == _correctObject)
            Object.AddPoint();
        else
            Object.RemovePoint();
    
        if (Object.Points >= 3)
            ObjectList.Remove(Object);

        RandomObject();
    }
}
