using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoachManager : MonoBehaviour
{
    public GameObject pointsUI;

    private string school = "State";

    private int totalWeekRecruitingPoints = 500;
    private int usedWeekRecruitingPoints;

    private List<Player> recruitedPlayers= new List<Player>();

    public CoachManager() { }

    public void AddRecruit(Player player)
    {
        if(recruitedPlayers.Contains(player)) return;
        recruitedPlayers.Add(player);
    }

    public string GetSchool() { return school; }

    public void ResetUsedPoints()
    {
        usedWeekRecruitingPoints = 0;
        pointsUI.GetComponent<TextMeshProUGUI>().text = "Recruiting Points: " + usedWeekRecruitingPoints.ToString() + "/" + totalWeekRecruitingPoints.ToString();
    }

    public void UpdatedUsedWeekRecruitingPoints(int value)
    {
        usedWeekRecruitingPoints += value;
        pointsUI.GetComponent<TextMeshProUGUI>().text = "Recruiting Points: " + usedWeekRecruitingPoints.ToString() + "/" + totalWeekRecruitingPoints.ToString();
    }

    public bool CanPerformAction(int value)
    {
        if (value + usedWeekRecruitingPoints <= totalWeekRecruitingPoints) {
            return true;
        }
        return false;
    }
}
