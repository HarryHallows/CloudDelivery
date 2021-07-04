using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public bool noQuest;

    public Text locationText;
    //public Text packageText;

    [System.Serializable]
    public class Location
    {
        public string name;
        //public string packageName;
        public Transform area;
        public GameObject package;
    }

    public List<Location> locations;
    public Location currentLocation;

    // Start is called before the first frame update
    void Start()
    {
        noQuest = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(noQuest)
        {
            GiveQuest();
        }
    }

    public void GiveQuest()
    {
        //Give quest location at random and package
        currentLocation = locations[Random.Range(0, locations.Count)];
        locationText.text = currentLocation.name;
        //packageText.text = currentLocation.packageName;
        noQuest = false;
    }
}
