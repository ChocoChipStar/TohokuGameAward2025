using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfData : MonoBehaviour
{
    public string TeamName { get; private set; } = "";
    public int Number { get; private set; } = 0;

    public void SetNumber(int selfNum)
    {
        Number = selfNum;
    }

    public void SetTeamName(string selfTeamName)
    {
        TeamName = selfTeamName;
    }
}
