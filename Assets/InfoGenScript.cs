using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoGenScript : MonoBehaviour
{
    [SerializeField] TMP_Text Name;
    [SerializeField] TMP_Text Birth;
    [SerializeField] TMP_Text Sex;
    [SerializeField] TMP_Text Exp;
    [SerializeField] TMP_Text Home;
    [SerializeField] TMP_Text ID;
    List<string> regionList = new List<string> { "Krakaly","Ruskva","Baring","Nermen","Diannmarch","Zarbosch","Corzburg","Farccinni","Sungaria", "Lavonusgal", };
    List<string> nameListFirstM = new List<string> { "Konrad ", "Oskar ", "Robert", "James", "Sam ", "Strai-Upp ", "Ryan ", "Sam ", "Mickey ", "Ronald ", "George ", "John ", "Nozomu ", "Faust ", "Hazama ", };
    List<string> nameListFirstF = new List<string> { "Ella ", "Holly ", "Chiri ", "Kafuka ", "Cindy ", "Helen ", "Carry ", "Diana ", "Diane ", };
    List<string> nameListLast = new List<string> { "Dirlewanger", "Khan", "Fant", "Day", "Gengstar", "Sung", "Gosling", "Hyde", "Mouse", "Macdonald", "Hammilton", "Winterberg", "Itoshiki", "Faustings", "Honoka", "Kitsu", "Fuura", "Lindy", "Wood", "Handler", "Burnwood", "Hourse", "Egbert", "Smith", "Johnson", };
    // Start is called before the first frame update
    void Start()
    {
        
        

        Birth.text = Random.Range(1940, 1971) + "." + Random.Range(1, 13) + "." + Random.Range(1, 31);

        if (Random.Range(1,3) > 1)
        {
            Sex.text = "Male";
        }
        else
        {
            Sex.text = "Female";
        }

        Exp.text = Random.Range(1983, 1989) + "." + Random.Range(1, 13) + "." + Random.Range(1, 31);

        
        if (Random.Range(1, 3) > 1)
        {
            if (Random.Range(1, 3) > 1)
            {
                Home.text = Home.text + "North ";
            }
            else
            {
                Home.text = Home.text + "South ";
            }
        }
        else
        {
            if (Random.Range(1, 3) > 1)
            {
                Home.text = Home.text + "East ";
            }
            else
            {
                Home.text = Home.text + "West ";
            }
        }
        Home.text = Home.text + regionList[Random.Range(0, regionList.Count)];

        ID.text = Random.Range( 10000 , 100000) + "-" + Random.Range(10000, 100000);

        if (Sex.text == "Male")
        {
            Name.text = nameListFirstM[Random.Range(0, nameListFirstM.Count)];
        }
        else
        {
            Name.text = nameListFirstF[Random.Range(0, nameListFirstF.Count)];
        }
        if (Random.Range(1, 3) > 1)
        {
            Name.text = Name.text + nameListLast[Random.Range(0, nameListLast.Count)];
        }
        else
        {
            Name.text = Name.text + nameListLast[Random.Range(0, nameListLast.Count)] + " " + nameListLast[Random.Range(0, nameListLast.Count)];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
