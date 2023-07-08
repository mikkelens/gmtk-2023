using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorShuffler : MonoBehaviour
{
    public List<GameObject> colorObjects = new List<GameObject>();
    public Transform[] positions;
    public List<Transform> positionsList = new List<Transform>();

    [SerializeField] GameObject obsRed, obsYellow, obsBlue, obsGreen;
    Vector3 redPos, yellowPos, bluePos, greenPos;
    int redInt, yellowInt, blueInt, greenInt;

    void Start()
    {
        
       

        addIndexes();
        StartCoroutine(shuffleThis());
    }

    IEnumerator shuffleThis()
    {
        
        
            yield return new WaitForSeconds(0.1f);
            ShuffleRed();
            yield return new WaitForSeconds(0.1f);
            ShuffleYellow();
            yield return new WaitForSeconds(0.1f);
            ShuffleGreen();
            yield return new WaitForSeconds(0.1f);
            ShuffleBlue();
            yield return new WaitForSeconds(0.1f);
            addIndexes();
            //yield break;
        
    }

    void ShuffleRed()
    {
        //red
        redPos = obsRed.transform.position;
        redInt = Random.Range(0, positionsList.Count);
        redPos = positionsList[redInt].transform.position;
        obsRed.transform.position = redPos;
        positionsList.RemoveAt(redInt);
    }

    void ShuffleYellow()
    {
        //yellow
        yellowPos = obsYellow.transform.position;
        yellowInt = Random.Range(0, positionsList.Count);
        yellowPos = positionsList[yellowInt].transform.position;
        obsYellow.transform.position = yellowPos;
        positionsList.RemoveAt(yellowInt);
    }

    void ShuffleGreen()
    {
        //green
        greenPos = obsGreen.transform.position;
        greenInt = Random.Range(0, positionsList.Count);
        greenPos = positionsList[greenInt].transform.position;
        obsGreen.transform.position = greenPos;
        positionsList.RemoveAt(greenInt);
    }

    void ShuffleBlue()
    {
        //blue
        bluePos = obsBlue.transform.position;
        blueInt = Random.Range(0, positionsList.Count);
        bluePos = positionsList[blueInt].transform.position;
        obsBlue.transform.position = bluePos;
        positionsList.RemoveAt(blueInt);
    }


    void addIndexes()
    {
        for (int i = 0; i < positions.Length; i++)
        {
            positionsList.Add(positions[i]);
        }
    }
}
