using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ShufflingExtention {

    // not my code!!!!!
    // got it here: http://stackoverflow.com/questions/273313/randomize-a-listt/1262619#1262619 
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }
}
