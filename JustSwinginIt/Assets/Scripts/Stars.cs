using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject star;
    public GameObject emptyStar;
    public GameObject level1;
    public GameObject level2;
    public GameObject level3;
    
    void Start()
    {
        int level1Stars = PlayerPrefs.GetInt("level1Complete") + PlayerPrefs.GetInt("level1Coins") + PlayerPrefs.GetInt("level1Time");
        Debug.Log("Level 1 Stars = " + level1Stars);
        for (int i = 0; i < 3; i++)
        {
            GameObject newStar;
            if (i <= level1Stars - 1)
            {
                newStar = Instantiate(star);
            }
            else
            {
                newStar = Instantiate(emptyStar);
            }
            newStar.transform.SetParent(level1.transform);
        }

        int level2Stars = PlayerPrefs.GetInt("level2Complete") + PlayerPrefs.GetInt("level2Coins") + PlayerPrefs.GetInt("level2Time");
        for (int i = 0; i < 3; i++)
        {
            GameObject newStar;
            if (i <= level2Stars - 1)
            {
                newStar = Instantiate(star);
            }
            else
            {
                newStar = Instantiate(emptyStar);
            }
            newStar.transform.SetParent(level2.transform);
        }

        int level3Stars = PlayerPrefs.GetInt("level3Complete") + PlayerPrefs.GetInt("level3Coins") + PlayerPrefs.GetInt("level3Time");
        for (int i = 0; i < 3; i++)
        {
            GameObject newStar;
            if (i <= level3Stars - 1)
            {
                newStar = Instantiate(star);
            }
            else
            {
                newStar = Instantiate(emptyStar);
            }
            newStar.transform.SetParent(level3.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
