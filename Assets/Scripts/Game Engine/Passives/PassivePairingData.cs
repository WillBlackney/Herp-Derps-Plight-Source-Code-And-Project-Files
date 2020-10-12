using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

[Serializable]
public class PassivePairingData
{
    public Passive passiveData;
    public int passiveStacks;

    public override string ToString()
    {
        string stringReturned = passiveData.ToString();

        StringBuilder builder = new StringBuilder();
        foreach (char c in stringReturned)
        {
            if (Char.IsUpper(c) && builder.Length > 0) builder.Append(' ');
            builder.Append(c);
        }

        stringReturned = builder.ToString();

        return stringReturned;
    }
}
