using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public bool noQuest;

    public Text locationText;
    public Text packageText;
    
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
        noQuest = false;
    }
}
