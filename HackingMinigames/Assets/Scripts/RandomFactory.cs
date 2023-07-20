using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Assertions;

public class RandomFactory : MonoBehaviour
{

    // Start is called before the first frame update
    public static int getRandomColor(){
        int random = UnityEngine.Random.Range(0, ColorHex.color_total);
        return random;
    }
    public static int getRandomShape(){
        int total_shapes = Enum.GetValues(typeof(ShapeBundle)).Length;
        int random = UnityEngine.Random.Range(0, total_shapes);
        return random;
    }

    public static string getColorText(int colorNo)
    {
        //can add checks if needed
        var colorText = ColorHex.colorMap[colorNo].colorName;
        return colorText;
    }
    public static string getShapeText(int shapeNo)
    {
        //can add checks if needed
        var shapeText = (ShapeBundle)shapeNo;
        return shapeText.ToString();
    }
    public static (string shapeText, int shapePos, int backgroundColorNo) GetShapeTuple()
    {
        var shapeNo = getRandomShape();
        var shapeText = getShapeText(shapeNo);
        var backgroundColorNo = getRandomColor();
        var shapePair = (shapeText, shapeNo, backgroundColorNo);
        return shapePair;
    }
    public static (string colorText,int colorPos, int backgroundColorNo) GetcolorPromptTuple()
    {
        var colorNo = getRandomColor();
        var colorText = getColorText(colorNo);
        var backgroundColorNo = getRandomColor();
        var colorPair = (colorText, colorNo, backgroundColorNo);
        Debug.Log("xCheck Text: " + colorText + " Color: " + ColorHex.colorMap[colorNo] + " Background: " + ColorHex.colorMap[backgroundColorNo].colorName);
        return colorPair;    
    }
    public static string getRandomQuestion(int question_total, int tile_total, ref List<int> wanted_tiles){
        string q1 = "Shape";
        string q2 = "Shape Background Color";
        string q3 = "Shape Text";
        string q4 = "Shape Text Background Color";
        string q5 = "Color Text";
        string q6 = "Color Text Background Color";
        string[] questions = {q1,q2,q3,q4,q5,q6};
        Debug.Log(tile_total);
        string full_question = "";
        var orderList = GetOrderList(tile_total);
        
        for (int i = 0; i < question_total; i++){
            int random = UnityEngine.Random.Range(0, questions.Length);
            full_question += questions[random];
            wanted_tiles.Add(orderList[i]);
            full_question += " (" + orderList[i] + ")";
            if(i!= question_total-1){
                full_question += " AND ";
            }
        }

        return full_question;
    }
    public static List<int> GetOrderList(int numbers_wanted)
    {
        List<int> tmpList = Enumerable.Range(1, numbers_wanted).ToList();
        System.Random random = new System.Random();
        tmpList = tmpList.OrderBy(x => random.Next()).ToList();
        return tmpList;
    }


}
