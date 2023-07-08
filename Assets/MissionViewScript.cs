using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionViewScript : MonoBehaviour
{
    [SerializeField, Required] private List<TextMeshProUGUI> missionTextComponents;

    private void Start()
    {
        WriteRemainingLevelsToMissionBoard();
    }

    private void WriteRemainingLevelsToMissionBoard()
    {
        List<Level> remainingLevelsCopy = PersistentGameManager.Instance.RemainingLevelsCopy;
        for (int i = 0; i < missionTextComponents.Count; i++)
        {
            missionTextComponents[i].text = i < remainingLevelsCopy.Count ? remainingLevelsCopy[i].Name : "";
            missionTextComponents[i].GetComponentInParent<Button>().interactable = i == 0;
        }
    }

    public void StartNextMission()
    {
        PersistentGameManager.Instance.StartNewLevel();
    }
}