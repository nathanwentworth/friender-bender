using UnityEngine;
using System.Collections;
using System;

public class PlayerList : IComparable<PlayerList>
{
    public int playerNum;
    public string readableName;

    public PlayerList(int newNum, string newName)
    {
        playerNum = newNum;
        readableName = newName;
    }

    public int CompareTo(PlayerList other)
    {
        if (other == null)
        {
            return 1;
        }

        return 0;
    }

}
