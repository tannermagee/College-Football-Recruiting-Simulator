using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject quickPlayerViewer;
    public List<Sprite> sprites;
    public GameObject content;
    public GameObject playerPrefab;
    public GameManager gameManager;
    private List<GameObject> spawnedPlayers = new List<GameObject>();
    private List<Player> currentPlayerList = new List<Player>();
    private string direction = "Ascending";


    public RecruitingManager recruitingManager;
    public CoachManager coachManager;

    public GameObject scoutButton;
    public GameObject letterButton;
    public GameObject visitButton;

    public void LoadPlayers(List<Player> players)
    {
        currentPlayerList = players;
        for (int i = 0; i < players.Count; i++)
        {
            GameObject player = GameObject.Instantiate(playerPrefab, content.transform);
            player.GetComponent<PlayerComponent>().uiManager = this;
            player.GetComponent<PlayerComponent>().index = i;
            player.GetComponent<PlayerComponent>().player = players[i];
            player.name = players[i].GetName();

            player.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = players[i].GetName();
            player.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = players[i].GetPosition();

            Transform rating = player.transform.GetChild(2);
            for (int j = 0; j < players[i].GetRating(); j++)
            {
                rating.GetChild(j).GetComponent<Image>().color = Color.yellow;
            }

            player.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = players[i].GetProjectedOffensiveRating();
            player.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = players[i].GetProjectedDefensiveRating();
            player.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = players[i].GetProjectedPhysicalRating();
            player.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = players[i].GetRecruitingInterest(coachManager.GetSchool()).ToString() + " %";

            spawnedPlayers.Add(player);
        }
    }

    public void ReloadPlayers()
    {
        foreach (GameObject player in spawnedPlayers)
        {
            Player playerObj = player.GetComponent<PlayerComponent>().player;

            player.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = TextColor(playerObj);
            player.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = TextColor(playerObj);

            if (playerObj.StatScouted("Offense", coachManager.GetSchool()))
            {
                player.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = playerObj.GetActualOffense().ToString();
                player.transform.GetChild(3).GetComponent<TextMeshProUGUI>().color = TextColor(playerObj);
            }
            else {
                player.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = playerObj.GetProjectedOffensiveRating();
                player.transform.GetChild(3).GetComponent<TextMeshProUGUI>().color = TextColor(playerObj);
            }
            if (playerObj.StatScouted("Defense", coachManager.GetSchool()))
            {
                player.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = playerObj.GetActualDefense().ToString();
                player.transform.GetChild(4).GetComponent<TextMeshProUGUI>().color = TextColor(playerObj);
            }
            else
            {
                player.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = playerObj.GetProjectedDefensiveRating();
                player.transform.GetChild(4).GetComponent<TextMeshProUGUI>().color = TextColor(playerObj);
            }
            if (playerObj.StatScouted("Physical", coachManager.GetSchool()))
            {
                player.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = playerObj.GetActualPhysical().ToString();
                player.transform.GetChild(5).GetComponent<TextMeshProUGUI>().color = TextColor(playerObj);
            }
            else
            {
                player.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = playerObj.GetProjectedPhysicalRating();
                player.transform.GetChild(5).GetComponent<TextMeshProUGUI>().color = TextColor(playerObj);
            }

            player.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = playerObj.GetRecruitingInterest(coachManager.GetSchool()).ToString() + " %";
            player.transform.GetChild(6).GetComponent<TextMeshProUGUI>().color = TextColor(playerObj);
        }
    }

    private Color TextColor(Player player)
    {
        if (player.GetCommittedSchool().Equals(coachManager.GetSchool()))
        {
            return Color.green;
        }
        else if(player.GetCommittedSchool().Length > 0)
        {
            return Color.red;
        }
        else
        {
            return Color.white;
        }
    }

    public void UpdateQuickPlayerViewer(int index)
    {
        UpdateQuickPlayerViewer(gameManager.GetPlayer(index));
    }

    public void UpdateQuickPlayerViewer(Player player)
    {
        Debug.Log("Updating player view.");

        Image playerImage = quickPlayerViewer.transform.GetChild(0).GetComponent<Image>();
        playerImage.sprite = sprites[player.GetImageIndex()];

        Transform bio = quickPlayerViewer.transform.GetChild(1);
        UpdateBio(bio, player);

        Transform stats = quickPlayerViewer.transform.GetChild(2);
        UpdateStats(stats, player);

        Transform recruiting = quickPlayerViewer.transform.GetChild(3);
        UpdateRecruiting(recruiting, player);

        if (player.CanScout(coachManager.GetSchool()) && coachManager.CanPerformAction(recruitingManager.GetScoutCost()))
        {
            scoutButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            scoutButton.GetComponent<Button>().interactable = false;
        }

        if (player.CanRecruit(coachManager.GetSchool()) && coachManager.CanPerformAction(recruitingManager.GetLetterCost()))
        {
            letterButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            Debug.Log("letter set false");
            letterButton.GetComponent<Button>().interactable = false;
        }

        if (player.CanRecruit(coachManager.GetSchool()) && player.CanVisit(coachManager.GetSchool()) && coachManager.CanPerformAction(recruitingManager.GetScoutCost()))
        {
            visitButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            visitButton.GetComponent<Button>().interactable = false;
        }


        recruitingManager.UpdateCurrentPlayer(player);
    }

    private void UpdateBio(Transform bio, Player player)
    {
        TextMeshProUGUI name = bio.GetChild(0).GetComponent<TextMeshProUGUI>();
        name.text = "Name: " + player.GetName();

        TextMeshProUGUI homeTown = bio.GetChild(1).GetComponent<TextMeshProUGUI>();
        homeTown.text = "Home Town: " + player.GetHomeTown();

        TextMeshProUGUI age = bio.GetChild(2).GetComponent<TextMeshProUGUI>();
        age.text = "Age: " + player.GetAge();

        TextMeshProUGUI weight = bio.GetChild(3).GetComponent<TextMeshProUGUI>();
        weight.text = "Weight: " + player.GetWeight();

        TextMeshProUGUI height = bio.GetChild(4).GetComponent<TextMeshProUGUI>();
        height.text = "Height: " + player.GetHeight();
    }

    private void UpdateStats(Transform stats, Player player)
    {
        TextMeshProUGUI position = stats.GetChild(0).GetComponent<TextMeshProUGUI>();
        position.text = "Position: " + player.GetPosition();

        Transform rating = stats.GetChild(1);
        for (int i = 0; i < player.GetRating(); i++)
        {
            rating.GetChild(i).GetComponent<Image>().color = Color.yellow;
        }

        TextMeshProUGUI offense = stats.GetChild(2).GetComponent<TextMeshProUGUI>();
        if (player.StatScouted("Offense", coachManager.GetSchool()))
        {
            offense.text = "Offense: " + player.GetActualOffense().ToString();
        }
        else
        {
            offense.text = "Offense: " + player.GetProjectedOffense();
        }

        TextMeshProUGUI defense = stats.GetChild(3).GetComponent<TextMeshProUGUI>();
        if (player.StatScouted("Defense", coachManager.GetSchool()))
        {
            defense.text = "Defense: " + player.GetActualDefense().ToString();
        }
        else
        {
            defense.text = "Defense: " + player.GetProjectedDefense();
        }

        TextMeshProUGUI physical = stats.GetChild(4).GetComponent<TextMeshProUGUI>();
        if (player.StatScouted("Physical", coachManager.GetSchool()))
        {
            physical.text = "Physical: " + player.GetActualPhysical().ToString();
        }
        else
        {
            physical.text = "Physical: " + player.GetProjectedPhysical();
        }
    }

    private void UpdateRecruiting(Transform recruiting, Player player)
    {
        List<Tuple<string, int>> topSchools = player.GetTopSchools();

        recruiting.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = topSchools[0].Item1;
        SchoolColor(recruiting.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>(), topSchools[0].Item1, player);
        recruiting.GetChild(1).GetChild(1).GetComponent<Slider>().value = topSchools[0].Item2;

        recruiting.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = topSchools[1].Item1;
        SchoolColor(recruiting.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>(), topSchools[1].Item1, player);
        recruiting.GetChild(2).GetChild(1).GetComponent<Slider>().value = topSchools[1].Item2;

        recruiting.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = topSchools[2].Item1;
        SchoolColor(recruiting.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>(), topSchools[2].Item1, player);
        recruiting.GetChild(3).GetChild(1).GetComponent<Slider>().value = topSchools[2].Item2;
    }

    private void SchoolColor(TextMeshProUGUI schoolText, string school, Player player){
        if(school == coachManager.GetSchool())
        {
            schoolText.color = Color.yellow;

            if (player.GetCommittedSchool().Equals(coachManager.GetSchool())){
                schoolText.color = Color.green;
            }
        }
        else
        {
            schoolText.color = Color.white;
            if (player.GetCommittedSchool().Equals(school)){
                schoolText.color = Color.red;
            }
        }
    }
    public void SortByName()
    {
        SwitchDirection();
        currentPlayerList = gameManager.SortByName(direction);
        
        for (int i = 0; i < currentPlayerList.Count; i++)
        {
            foreach(GameObject g in spawnedPlayers)
            {
                if(currentPlayerList[i].GetId() == g.GetComponent<PlayerComponent>().player.GetId())
                {
                    Debug.Log($"Match. Moving to index: {i}");
                    g.transform.SetSiblingIndex(i);
                }
            }
        }
    }
    public void SortByPosition()
    {
        SwitchDirection();
        currentPlayerList = gameManager.SortByPosition(direction);

        for (int i = 0; i < currentPlayerList.Count; i++)
        {
            foreach (GameObject g in spawnedPlayers)
            {
                if (currentPlayerList[i].GetId() == g.GetComponent<PlayerComponent>().player.GetId())
                {
                    g.transform.SetSiblingIndex(i);
                }
            }
        }
    }
    public void SortByRating()
    {
        SwitchDirection();
        currentPlayerList = gameManager.SortByRating(direction);

        for (int i = 0; i < currentPlayerList.Count; i++)
        {
            foreach (GameObject g in spawnedPlayers)
            {
                if (currentPlayerList[i].GetId() == g.GetComponent<PlayerComponent>().player.GetId())
                {
                    g.transform.SetSiblingIndex(i);
                }
            }
        }
    }
    public void SortByOffense(List<Player> players)
    {
        players.Sort((p1, p2) => p1.GetProjectedOffense().CompareTo(p2.GetProjectedOffense()));
    }
    public void SortByDefense(List<Player> players)
    {
        players.Sort((p1, p2) => p1.GetProjectedDefense().CompareTo(p2.GetProjectedDefense()));
    }
    public void SortByPhysical(List<Player> players)
    {
        players.Sort((p1, p2) => p1.GetProjectedDefense().CompareTo(p2.GetProjectedDefense()));
    }
    public void SortByInterest()
    {
        SwitchDirection();
        currentPlayerList = gameManager.SortByInterest(direction, coachManager.GetSchool());

        for (int i = 0; i < currentPlayerList.Count; i++)
        {
            foreach (GameObject g in spawnedPlayers)
            {
                if (currentPlayerList[i].GetId() == g.GetComponent<PlayerComponent>().player.GetId())
                {
                    Debug.Log($"Match. Moving to index: {i}");
                    g.transform.SetSiblingIndex(i);
                }
            }
        }
    }

    private void SwitchDirection()
    {
        if (direction.Equals("Ascending"))
        {
            direction = "Descending";
        }
        else
        {
            direction = "Ascending";
        }
    }
}
