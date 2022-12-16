using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position
{
    private string position;
    private Tuple<int, int> weightRange;
    private Tuple<int, int> heightRange;
    private List<string> primaryAttributes = new List<string>();
    public Position(string position, Tuple<int, int> weightRange, Tuple<int, int> heightRange, List<string> primaryAttributes)
    {
        this.position = position;
        this.weightRange = weightRange;
        this.heightRange = heightRange;
        this.primaryAttributes = primaryAttributes;
    }

    public string getPosition() { return position; }

    public Tuple<int, int> getWeightRange() { return weightRange; }

    public Tuple<int, int> getHeightRange() { return heightRange; }

    public List<string> getPrimaryAttributes() { return primaryAttributes; }
}
