using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruitingManager : MonoBehaviour
{
    private Player currentPlayer;
    public CoachManager coachManager;
    public UIManager uiManager;

    //recruiting costs
    private int scoutCost = 25;
    private int letterCost = 25;
    private int visitCost = 75;

    //recruiting boosts
    private int maxScoutingRecruitBoost = 6;
    private int maxLetterRecruitBoost = 8;
    private int maxVisitRecruitBoost = 25;

    public RecruitingManager() { }

    public void UpdateCurrentPlayer(Player player)
    {
        currentPlayer = player;
    }

    public void UIScout()
    {
        if (Scout(coachManager.GetSchool()))
        {
            coachManager.UpdatedUsedWeekRecruitingPoints(scoutCost);
            ReloadUI();
        }
    }

    public bool Scout(string school)
    {
        if(currentPlayer != null && currentPlayer.CanScout(school)) {
            Debug.Log($"{school} scouting {currentPlayer.GetName()}.");
            currentPlayer.Scout(school);
            currentPlayer.IncreaseRecruitingInterest(school, UnityEngine.Random.Range(1, maxScoutingRecruitBoost));

            return true;
        }
        return false;
    }

    public void UILetter()
    {
        if (SendLetter(coachManager.GetSchool()))
        {
            coachManager.UpdatedUsedWeekRecruitingPoints(letterCost);
            ReloadUI();
        }
    }

    public bool SendLetter(string school)
    {
        if (currentPlayer != null && currentPlayer.CanRecruit(school))
        {
            Debug.Log($"{school} sending letter to {currentPlayer.GetName()}.");
            currentPlayer.IncreaseRecruitingInterest(school, UnityEngine.Random.Range(1, maxLetterRecruitBoost));
            currentPlayer.RecruitedThisWeek(school);

            return true;
        }
        return false;
    }

    public void UIVisit()
    {
        if (OfferVisit(coachManager.GetSchool()))
        {
            coachManager.UpdatedUsedWeekRecruitingPoints(visitCost);
            ReloadUI();
        }
    }

    public bool OfferVisit(string school)
    {
        if (currentPlayer != null && currentPlayer.CanRecruit(school) && currentPlayer.CanVisit(school))
        {
            Debug.Log($"{school} offering visit to {currentPlayer.GetName()}.");
            currentPlayer.Visit(school);
            currentPlayer.IncreaseRecruitingInterest(school, UnityEngine.Random.Range(10, maxVisitRecruitBoost));
            currentPlayer.RecruitedThisWeek(school);

            return true;
        }
        return false;
    }

    private void ReloadUI()
    {
        uiManager.UpdateQuickPlayerViewer(currentPlayer);
        uiManager.ReloadPlayers();
    }

    public int GetScoutCost() { return scoutCost; }

    public int GetLetterCost() {  return letterCost; }

    public int GetVisitCost() { return visitCost; }
}
