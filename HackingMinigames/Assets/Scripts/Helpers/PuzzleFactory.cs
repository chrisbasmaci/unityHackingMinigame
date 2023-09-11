using UnityEngine;
using System;

using TMPro;
using System.Collections.Generic;
using System.Linq;
using Hack;

public static class PuzzleFactory
{
    public static string getQuestionSolution(List<Card> current_tiles, string question)
    {

        string solution = "";
        string[] splitQuestions = question.Split(new[] { " AND " }, StringSplitOptions.None);
        // Debug.Assert(splitQuestions.Length == wanted_tiles.Count, 
        //     "You need same amount of questions as well as required tiles!");
        int question_index = 0;
        var sortedWanted = current_tiles.Where(card => card.isWanted).OrderBy(card => card.wantedOrder).ToList();
        foreach (var card in sortedWanted)
        {
            string clean_question = splitQuestions[question_index].Remove(splitQuestions[question_index].Length - 4);
            if (question_index != 0)
            {
                solution += " ";
            }
            solution += getPartSolution(card, clean_question);
            Debug.Log("SOl"+solution);
            question_index++;

        }
        return solution;
    }

    public static string getPartSolution(Card tile, string part_question)
    {
        switch (part_question)
        {
            case "Shape":
                return tile._face.getShapeText();
            case "Shape Background Color":
                return tile._face.getShapeBC();
            case "Shape Text":
                return tile._face.getShapePromptText();
            case "Shape Text Background Color":
                return tile._face.getShapePromptBC();
            case "Color Text":
                return tile._face.getColorPromptText();
            case "Color Text Background Color":
                return tile._face.getColorPromptBC();
            default:
                Debug.Log("-"+part_question+"-");
                Debug.Log("something wrong in solution logic");
                return null;
        }
        
    }
}
