using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    //bio
    private int id;
    private string firstName;
    private string lastName;
    private int age;
    private int weight;
    private int height;
    private string city;
    private string state;
    private int rating;
    private string position;
    private int imageIndex;

    //traits
    private List<string> traits;

    //ratings
    private int projectedOffensiveRating;
    private int actualOffensiveRating;
    private int projectedDefensiveRating;
    private int actualDefensiveRating;
    private int projectedPhysicalRating;
    private int actualPhysicalRating;

    //recruiting
    private string committedSchool = "";
    private IDictionary<string, int> recruitingInterest;

    //scouting
    private IDictionary<string, List<string>> unscouted;
    private List<string> scoutedThisWeek = new List<string>();
    private List<string> recruitedThisWeek = new List<string>();
    private List<string> hasVisited = new List<string>();

    public Player(int id, string firstName, string lastName, int age, int weight, int height, string city, string state, string position, int rating, int imageIndex, List<string> traits, Tuple<int, int ,int> projectedAttributes, Tuple<int, int, int> actualAttributes)
    {
        this.id = id;
        this.firstName = firstName;
        this.lastName = lastName;
        this.age = age;
        this.weight = weight;
        this.height = height;
        this.city = city;
        this.state = state;
        this.position = position;
        this.imageIndex = imageIndex;
        this.rating = rating;
        this.traits = traits;
        this.projectedOffensiveRating = projectedAttributes.Item1;
        this.actualOffensiveRating = actualAttributes.Item1;
        this.projectedDefensiveRating = projectedAttributes.Item2;
        this.actualDefensiveRating = actualAttributes.Item2;
        this.projectedPhysicalRating = projectedAttributes.Item3;
        this.actualPhysicalRating = actualAttributes.Item3;
        this.committedSchool = "";
        this.recruitingInterest = new Dictionary<string, int>();
        unscouted = new Dictionary<string, List<string>>();
    }

    override
    public string ToString()
    {
        return $"{{\n\tPlayer {this.id}:\n\tName: {this.firstName} {this.lastName}\n\tAge: {this.age}\n\tWeight: {this.weight}\n\tHeight: {this.height/12}-{this.height % 12}\n\tTraits: {this.GetPrintedTraits()}\n\tHome Town: {this.city}, {this.state}\n\tPosition: {this.position}\n\tRating: {this.rating}\n\tProjected Offensive Rating: {this.GetProjectedOffensiveRating()}\n\tProjected Defensive Rating: {this.GetProjectedDefensiveRating()}\n\tProjected Physical Rating: {this.GetProjectedPhysicalRating()}\n\tActual Offensive Rating: {this.actualOffensiveRating}\n\tActual Defensive Rating: {this.actualDefensiveRating}\n\tActual Physical Rating: {this.actualPhysicalRating}\n}}";
    }

    public void SetRecruitingInterest(IDictionary<string, int> recruitingInterest)
    {
        this.recruitingInterest = recruitingInterest;
        foreach(string key in recruitingInterest.Keys )
        {
            this.unscouted[key] = new List<string>() {"Offense", "Defense", "Physical" };
        }
    }

    public int GetRecruitingInterest(string school)
    {
        if (recruitingInterest.ContainsKey(school))
        {
            return recruitingInterest[school];
        }
        else
        {
            return 0;
        }
    }

    public string GetProjectedOffensiveRating() { return ConvertRatingToString(this.projectedOffensiveRating); }
    public string GetProjectedDefensiveRating() { return ConvertRatingToString(this.projectedDefensiveRating); }
    public string GetProjectedPhysicalRating() { return ConvertRatingToString(this.projectedPhysicalRating); }

    public string GetPrintedTraits()
    {
        string s = "";
        for(int i = 0; i< this.traits.Count; i++)
        {
            s += this.traits[i] + ", ";
        }
        s.Trim();
        return s ;
    }

    private string ConvertRatingToString(int attribute)
    {
        if (attribute < 55) { return "F"; }
        else if (attribute < 60) { return "D"; }
        else if (attribute < 70) { return "C"; }
        else if (attribute < 80) { return "B"; }
        else if (attribute < 100) { return "A"; }
        else { return "F"; }
    }

    public int GetImageIndex() { return imageIndex; }

    public int GetId() { return id; }
    public string GetName() { return firstName + " " + lastName; }

    public string GetHomeTown() { return city + ", " + state; }

    public string GetAge() { return age.ToString(); }

    public string GetWeight() { return weight.ToString(); }

    public string GetHeight() { return (this.height / 12).ToString() + "-" + (this.height % 12).ToString(); }

    public string GetPosition() { return position; }

    public int GetRating() { return rating; }
    public string GetProjectedOffense() { return ConvertRatingToString(projectedOffensiveRating); }
    public string GetProjectedDefense() { return ConvertRatingToString(projectedDefensiveRating); }
    public string GetProjectedPhysical() { return ConvertRatingToString(projectedPhysicalRating); }

    public int GetActualOffense() { return actualOffensiveRating; }
    public int GetActualDefense() { return actualDefensiveRating; }

    public int GetActualPhysical() { return actualPhysicalRating; }

    public string GetCommittedSchool() { return committedSchool; }

    public void ResetScouting()
    {
        scoutedThisWeek = new List<string>();
    }

    public void RecruitedThisWeek(string school)
    {
        recruitedThisWeek.Add(school);
    }
    public void ResetRecruiting()
    {
        recruitedThisWeek = new List<string>();
    }

    public void Scout(string school)
    {
        if(unscouted.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, unscouted[school].Count);
            unscouted[school].Remove(unscouted[school][index]);
            scoutedThisWeek.Add(school);
        }
    }

    public bool CanScout(string school)
    {
        return (unscouted[school].Count > 0 && !scoutedThisWeek.Contains(school) && !Committed());
    }

    public bool StatScouted(string stat, string school)
    {
        if (unscouted[school].Contains(stat)) return false;
        return true;
    }

    public bool CanRecruit(string school)
    {
        return committedSchool.Length == 0 && !recruitedThisWeek.Contains(school);
    }

    public bool CanVisit(string school)
    {
        return recruitingInterest[school] >= 45 && !hasVisited.Contains(school);
    }

    public void Visit(string school)
    {
        hasVisited.Add(school);
    }

    public void IncreaseRecruitingInterest(string school, int recruitingValue)
    {
        if (recruitingInterest.ContainsKey(school))
        {
            int currentRecruiting = recruitingInterest[school];
            int newRecruiting = currentRecruiting+ recruitingValue;
            newRecruiting = Mathf.Clamp(newRecruiting, 0, 100);
            recruitingInterest[school] = newRecruiting;
        }
        else
        {
            recruitingInterest.Add(school, recruitingValue);
        }
        Debug.Log($"{this.GetName()} has gained {recruitingValue.ToString()} towards {school}. Current Interest: {recruitingInterest[school]}");
    }

    public bool Committed()
    {
        return committedSchool.Length > 0;
    }

    public bool DidCommit()
    {
        List<string> committedSchools = new List<string>();
        int interestedSchools = recruitingInterest.Keys.Count;

        //check if interest is 100%, if yes then commit
        //if two schools have 100% then randomly select between them I suppose lol.
        foreach (string key in recruitingInterest.Keys){
            if (recruitingInterest[key] == 100)
            {
                Debug.Log($"Player {this.id} reached 100.");
                committedSchools.Add(key);
            }
        }

        if(committedSchools.Count > 0)
        {
            if(committedSchools.Count > 1)
            {
                int index = UnityEngine.Random.Range(0, committedSchools.Count);
                committedSchool= committedSchools[index];
                Debug.Log($"Player {this.id} committed to {committedSchool}.");
                return true;
            }
            else
            {
                committedSchool = committedSchools[0];
                Debug.Log($"Player {this.id} committed to {committedSchool}.");
                return true;
            }
        }

        // go through all schools and randomly generate a number if above 40, if it falls within the schools recruiting percent, then commit.
        // if two schools have RNG commits, then no commit happens.
        foreach (string key in recruitingInterest.Keys)
        {
            if (recruitingInterest[key] > 65)
            {
                int randomNumber = UnityEngine.Random.Range(1, 100);
                if (randomNumber <= recruitingInterest[key])
                {
                    committedSchools.Add(key);
                }
            }
        }

        if (committedSchools.Count > 0)
        {
            if (committedSchools.Count > 1)
            {
                return false;
            }
            else
            {
                committedSchool = committedSchools[0];
                Debug.Log($"Player {this.id} RANDOMLY committed to {committedSchool}.");
                return true;
            }
        }

        return false;
    }

    public List<Tuple<string, int>> GetTopSchools()
    {
        List<Tuple<string, int>> recruiting = new List<Tuple<string, int>>();
        foreach(string school in recruitingInterest.Keys)
        {
            recruiting.Add(new Tuple<string, int>(school, recruitingInterest[school]));
        }
        recruiting.Sort((school1, school2) => school2.Item2.CompareTo(school1.Item2));
        return recruiting;
    }
}
