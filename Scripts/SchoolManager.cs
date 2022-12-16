using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolManager : MonoBehaviour
{
    public GameManager gameManager;
    public RecruitingManager recruitingManager;


    //THIS WILL HANDLE ALL THE NPC SCHOOL RECRUITING
    private List<string> schools = new List<string>(){ "Central", "East", "MidWest", "North", "NorthEast", "NorthWest", "South", "SouthEast", "SouthWest", "Tech", "West" };


    public void Recruit()
    {
        foreach(string school in schools)
        {
            Debug.Log($"-----RECRUITING FOR {school}--------");
            List<Player> players = gameManager.SortByInterest("Descending", school);
            int totalWeekRecruitingPoints = 500;
            int usedWeekRecruitingPoints = 0;
            foreach (Player player in players) {
                if(usedWeekRecruitingPoints < totalWeekRecruitingPoints)
                {
                    recruitingManager.UpdateCurrentPlayer(player);
                    if (player.CanScout(school) && CanPerformAction(recruitingManager.GetScoutCost(), usedWeekRecruitingPoints, totalWeekRecruitingPoints))
                    {
                        recruitingManager.Scout(school);
                        usedWeekRecruitingPoints += recruitingManager.GetScoutCost();
                    }

                    if (player.CanRecruit(school) && player.CanVisit(school) && CanPerformAction(recruitingManager.GetVisitCost(), usedWeekRecruitingPoints, totalWeekRecruitingPoints))
                    {
                        recruitingManager.OfferVisit(school);
                        usedWeekRecruitingPoints += recruitingManager.GetVisitCost();
                    }
                    else if (player.CanRecruit(school) && CanPerformAction(recruitingManager.GetLetterCost(), usedWeekRecruitingPoints, totalWeekRecruitingPoints))
                    {
                        recruitingManager.SendLetter(school);
                        usedWeekRecruitingPoints += recruitingManager.GetLetterCost();
                    }

                    Debug.Log($"{school} recruited {player.GetName()}. Current interest: {player.GetRecruitingInterest(school)}%.");
                }
                else
                {
                    break;
                }
            }
        }
    }



    private bool CanPerformAction(int value, int usedWeekRecruitingPoints, int totalWeekRecruitingPoints)
    {
        if (value + usedWeekRecruitingPoints <= totalWeekRecruitingPoints)
        {
            return true;
        }
        return false;
    }

    public List<string> GetSchools() { return schools; }
}
