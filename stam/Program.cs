using System;

public class Example
{
    public static void Main()
    {
        string[] words = { "Tuesday", "Salı", "Вторник", "Mardi", 
            "Τρίτη", "Martes", "יום שלישי", 
            "الثلاثاء", "วันอังคาร" };
        // Display array in unsorted order.
        foreach (string word in words)
            Console.WriteLine(word);
        Console.WriteLine();

        // Create parallel array of words by calling ToLowerInvariant.
        string[] lowerWords = new string[words.Length];
        for (int ctr = words.GetLowerBound(0); ctr <= words.GetUpperBound(0); ctr++)
            lowerWords[ctr] = words[ctr].ToLowerInvariant();
      
        // Display the sorted array.
        foreach (string word in lowerWords)
            Console.WriteLine(word);
    }
}