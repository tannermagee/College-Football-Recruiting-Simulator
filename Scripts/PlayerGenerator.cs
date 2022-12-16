using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Globalization;

public class PlayerGenerator
{
    private int totalPlayers = 500;
    private List<Player> playerList = new List<Player>();

    public PlayerGenerator(List<string> schools) {
        GeneratePlayers(schools);
    }

    private void GeneratePlayers(List<string> schools)
    {
        List<string> firstNames = GetNames("Assets/Data/FirstNames.csv");
        List<string> lastNames = GetNames("Assets/Data/LastNames.csv");
        List<Tuple<string, string>> locations = GetLocations("Assets/Data/Cities.csv");

        for (int i = 0; i < totalPlayers; i++)
        {
            Tuple<string, string> name = GeneratePlayerName(firstNames, lastNames);
            Tuple<string, string> homeTown = GenerateHomeTown(locations);
            string position = GeneratePosition();
            int rating = GenerateRating();
            int image = GenerateImage();
            Tuple<int, int> physicals = GeneratePhysicalAttributes(position);
            List<string> traits = GenerateTraits();
            Tuple<int, int, int> projectedAttributes = GenerateProjectedAttributes(position, rating);
            Tuple<int, int, int> actualAttributes = GenerateActualAttributes(traits, projectedAttributes);
            Player player = new Player(i, name.Item1, name.Item2, UnityEngine.Random.Range(17, 20), physicals.Item1, physicals.Item2, homeTown.Item1, homeTown.Item2, position, rating, image, traits, projectedAttributes, actualAttributes);
            Debug.Log(player.ToString());

            player.SetRecruitingInterest(GenerateInitialInterest(schools));

            playerList.Add(player);
        }
    }

    public List<Player> GetPlayers() { return playerList; }

    private List<string> GetNames(string path)
    {
        List<string> names = new List<string>();
        using (var nameReader = new StreamReader(path))
        {
            while (!nameReader.EndOfStream)
            {
                var line = nameReader.ReadLine();
                var values = line.Split(',');
                names.Add(values[0]);
            }
        }
        return names;
    }

    private List<Tuple<string, string>> GetLocations(string path)
    {
        List<Tuple<string, string>> locations = new List<Tuple<string, string>>();
        using (var locationReader = new StreamReader(path))
        {
            while (!locationReader.EndOfStream)
            {
                var line = locationReader.ReadLine();
                var values = line.Split(',');
                Tuple<string, string> location = new Tuple<string, string>(values[0], values[1]);
                locations.Add(location);
            }
        }
        return locations;
    }

    private Tuple<string, string> GeneratePlayerName(List<string> firstNames, List<string> lastNames)
    {
        int totalFirstNames = firstNames.Count;
        int totalLastNames = lastNames.Count;
        int fIndex = UnityEngine.Random.Range(0, totalFirstNames);
        int lIndex = UnityEngine.Random.Range(0, totalLastNames);

        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;


        return new Tuple<string, string>(firstNames[fIndex], textInfo.ToTitleCase(lastNames[lIndex].ToLower()));
    }

    private Tuple<string, string> GenerateHomeTown(List<Tuple<string, string>> locations)
    {
        int totalLocations = locations.Count;
        int lIndex = UnityEngine.Random.Range(0, totalLocations);

        return locations[lIndex];
    }

    private string GeneratePosition()
    {
        string position = "";
        int percentile = UnityEngine.Random.Range(0, 100);

        if (percentile <= 3) {
            position = "K";
        }
        else if (percentile <= 6)
        {
            position = "P";
        }
        else if (percentile <= 14)
        {
            position = "T";
        }
        else if (percentile <= 22)
        {
            position = "G";
        }
        else if (percentile <= 25)
        {
            position = "C";
        }
        else if (percentile <= 33)
        {
            position = "TE";
        }
        else if (percentile <= 36)
        {
            position = "QB";
        }
        else if (percentile <= 39)
        {
            position = "RB";
        }
        else if (percentile <= 47)
        {
            position = "WR";
        }
        else if (percentile <= 55)
        {
            position = "DE";
        }
        else if (percentile <= 63)
        {
            position = "DT";
        }
        else if (percentile <= 75)
        {
            position = "LB";
        }
        else if (percentile <= 82)
        {
            position = "CB";
        }
        else if (percentile <= 90)
        {
            position = "S";
        }
        else
        {
            position = "ATH";
        }
        return position;
    }

    private IDictionary<string, int> GenerateInitialInterest(List<string> schools)
    {
        IDictionary<string, int> recruitingInterest = new Dictionary<string, int>();
        foreach(string school in schools)
        {
            recruitingInterest.Add(school, UnityEngine.Random.Range(0, 15));
        }
        return recruitingInterest;
    }

    private int GenerateRating()
    {
        int percentile = UnityEngine.Random.Range(0, 100);

        if (percentile <= 3) {
            return 5;
        }
        else if(percentile <= 13)
        {
            return 4;
        }
        else if(percentile <= 33)
        {
            return 3;
        }
        else if(percentile <= 63)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }

    private int GenerateImage()
    {
        int number = UnityEngine.Random.Range(0, 19);
        return number;
    }
    private Tuple<int, int> GeneratePhysicalAttributes(string position)
    {
        int lowerWeight = 0;
        int upperWeight = 0;
        int lowerHeight = 0;
        int upperHeight = 0;
        if (position == "K" || position == "P")
        {
            lowerWeight = 160;
            upperWeight = 200;
            lowerHeight = 67;
            upperHeight = 75;
        }
        else if (position == "T" || position == "G" || position == "C" || position == "DT")
        {
            lowerWeight = 250;
            upperWeight = 400;
            lowerHeight = 73;
            upperHeight = 80;
        }
        else if (position == "TE")
        {
            lowerWeight = 220;
            upperWeight = 260;
            lowerHeight = 75;
            upperHeight = 80;
        }
        else if (position == "QB")
        {
            lowerWeight = 180;
            upperWeight = 220;
            lowerHeight = 70;
            upperHeight = 78;
        }
        else if (position == "RB")
        {
            lowerWeight = 190;
            upperWeight = 230;
            lowerHeight = 70;
            upperHeight = 77;
        }
        else if (position == "WR")
        {
            lowerWeight = 170;
            upperWeight = 220;
            lowerHeight = 70;
            upperHeight = 80;
        }
        else if (position == "DE")
        {
            lowerWeight = 220;
            upperWeight = 280;
            lowerHeight = 75;
            upperHeight = 79;
        }
        else if (position == "LB")
        {
            lowerWeight = 220;
            upperWeight = 260;
            lowerHeight = 72;
            upperHeight = 75;
        }
        else if (position == "CB")
        {
            lowerWeight = 180;
            upperWeight = 210;
            lowerHeight = 70;
            upperHeight = 76;
        }
        else if (position == "S")
        {
            lowerWeight = 200;
            upperWeight = 230;
            lowerHeight = 70;
            upperHeight = 77;
        }
        else if (position == "ATH")
        {
            lowerWeight = 180;
            upperWeight = 240;
            lowerHeight = 70;
            upperHeight = 78;
        }

        return new Tuple<int, int>(UnityEngine.Random.Range(lowerWeight, upperWeight + 1), UnityEngine.Random.Range(lowerHeight, upperHeight + 1));
    }

    private List<string> GenerateTraits()
    {
        List<string> traits = new List<string>();
        int percentile = UnityEngine.Random.Range(0, 100);
        if (percentile <= 3)
        {
            traits.Add("GEM");
        }
        else if (percentile <= 7)
        {
            traits.Add("BUST");
        }

        return traits;
    }
    private Tuple<int, int, int> GenerateProjectedAttributes(string position, int rating)
    {
        int projectedOffensiveRating = 0;
        int projectedDefensiveRating = 0;
        int projectedPhysicalRating = 0;

        if (position == "K" || position == "P")
        {
            projectedOffensiveRating = 20;
            projectedDefensiveRating = 20;
            projectedPhysicalRating = 40;
        }
        else if (position == "T" || position == "G" || position == "C" || position == "TE" || position == "QB" || position == "RB" || position == "WR")
        {
            projectedOffensiveRating = 50;
            projectedDefensiveRating = 10;
            projectedPhysicalRating = 50;
        }
        else if (position == "DE" || position == "DT" || position == "LB" || position == "CB" || position == "S")
        {
            projectedOffensiveRating = 10;
            projectedDefensiveRating = 50;
            projectedPhysicalRating = 50;
        }
        else
        {
            //ATH
            projectedOffensiveRating = 50;
            projectedDefensiveRating = 50;
            projectedPhysicalRating = 50;
        }

        int minAttribute = 0;
        int maxAttribute = 0;
        switch(rating)
        {
            case 1:
                minAttribute = 1;
                maxAttribute = 10;
                break;
            case 2:
                minAttribute = 5;
                maxAttribute = 15;
                break;
            case 3:
                minAttribute = 10;
                maxAttribute = 20;
                break;
            case 4:
                minAttribute = 15;
                maxAttribute = 25;
                break;
            case 5:
                minAttribute = 20;
                maxAttribute = 30;
                break;
        }

        projectedOffensiveRating += UnityEngine.Random.Range(minAttribute, maxAttribute);
        projectedDefensiveRating += UnityEngine.Random.Range(minAttribute, maxAttribute);
        projectedPhysicalRating += UnityEngine.Random.Range(minAttribute, maxAttribute);
        return new Tuple<int, int, int>(projectedOffensiveRating, projectedDefensiveRating, projectedPhysicalRating);
    }

    private Tuple<int, int, int> GenerateActualAttributes(List<string> traits, Tuple<int, int, int> projectedAttributes)
    {
        int minAttribute = -5;
        int maxAttribute = 5;
        if (traits.Contains("GEM"))
        {
            minAttribute = 5;
            maxAttribute = 15;
        }
        else if (traits.Contains("BUST"))
        {
            minAttribute = -15;
            maxAttribute = -5;
        }

        int actualOffensiveRating = projectedAttributes.Item1 + UnityEngine.Random.Range(minAttribute, maxAttribute);
        int actualDefensiveRating = projectedAttributes.Item2 + UnityEngine.Random.Range(minAttribute, maxAttribute);
        int actualPhysicalRating = projectedAttributes.Item3 + UnityEngine.Random.Range(minAttribute, maxAttribute);

        return new Tuple<int, int, int>(actualOffensiveRating, actualDefensiveRating, actualPhysicalRating);
    }

    public void SortDefault()
    {
        this.playerList.Sort((p1, p2) => p1.GetId().CompareTo(p2.GetId()));
    }

    public void SortByName(string direction)
    {
        if(direction == "Ascending")
        {
            this.playerList.Sort((p1, p2) => p1.GetName().CompareTo(p2.GetName()));
        }
        else
        {
            this.playerList.Sort((p1, p2) => p2.GetName().CompareTo(p1.GetName()));
        }
    }

    public void SortByPosition(string direction)
    {
        if (direction == "Ascending")
        {
            this.playerList.Sort((p1, p2) => p1.GetPosition().CompareTo(p2.GetPosition()));
        }
        else
        {
            this.playerList.Sort((p1, p2) => p2.GetPosition().CompareTo(p1.GetPosition()));
        }
    }

    public void SortByRating(string direction)
    {
        if (direction == "Ascending")
        {
            this.playerList.Sort((p1, p2) => p1.GetRating().CompareTo(p2.GetRating()));
        }
        else
        {
            this.playerList.Sort((p1, p2) => p2.GetRating().CompareTo(p1.GetRating()));
        }
    }

    public void SortByInterest(string direction, string school)
    {
        if (direction == "Ascending")
        {
            this.playerList.Sort((p1, p2) => p1.GetRecruitingInterest(school).CompareTo(p2.GetRecruitingInterest(school)));
        }
        else
        {
            this.playerList.Sort((p1, p2) => p2.GetRecruitingInterest(school).CompareTo(p1.GetRecruitingInterest(school)));
        }
    }
}
