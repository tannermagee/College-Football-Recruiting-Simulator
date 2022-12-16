using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UIManager uiManager;
    public GameObject weekUI;
    public CoachManager coachManager;
    public SchoolManager schoolManager;

    private int currentWeek = 1;
    private int totalWeeks = 13;
    private PlayerGenerator playerGenerator;
    private List<string> schools;

    // Start is called before the first frame update
    void Start()
    {
        schools = new List<string>(schoolManager.GetSchools().ToArray());
        schools.Add(coachManager.GetSchool());
        playerGenerator = new PlayerGenerator(schools);
        uiManager.UpdateQuickPlayerViewer(playerGenerator.GetPlayers()[0]);
        uiManager.LoadPlayers(playerGenerator.GetPlayers());
        Debug.Log($"It is currently week {currentWeek}, out of {totalWeeks} total weeks.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetCurrentWeek() { return currentWeek; }

    public void IncreaseCurrentWeek()
    {
        if(currentWeek < totalWeeks )
        {
            // UPDATE AI RECRUITING
            schoolManager.Recruit();

            // Player Commit Logic
            foreach (Player player in playerGenerator.GetPlayers())
            {
                //RESET SCOUTING
                player.ResetScouting();
                player.ResetRecruiting();

                if (!player.Committed())
                {
                    bool didCommit = player.DidCommit();
                    if (didCommit)
                    {
                        Debug.Log($"{player.GetName()} committed to {player.GetCommittedSchool()}.");
                    }
                }
            }

            uiManager.UpdateQuickPlayerViewer(0);
            uiManager.ReloadPlayers();
            coachManager.ResetUsedPoints();

            currentWeek++;
            weekUI.GetComponent<TextMeshProUGUI>().text = "Week " + currentWeek.ToString() + " of " + totalWeeks.ToString();
            Debug.Log($"It is currently week {currentWeek}, out of {totalWeeks} total weeks.");
        }
    }

    public Player GetPlayer(int index) { return playerGenerator.GetPlayers()[index]; }

    public List<Player> GetPlayers() { return playerGenerator.GetPlayers(); }

    public List<Player> SortByName(string direction)
    {
        playerGenerator.SortByName(direction);
        return playerGenerator.GetPlayers();
    }

    public List<Player> SortByPosition(string direction)
    {
        playerGenerator.SortByPosition(direction);
        return playerGenerator.GetPlayers();
    }

    public List<Player> SortByRating(string direction)
    {
        playerGenerator.SortByRating(direction);
        return playerGenerator.GetPlayers();
    }

    public List<Player> SortByInterest(string direction, string school)
    {
        playerGenerator.SortByInterest(direction, school);
        return playerGenerator.GetPlayers();
    }
}
