using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextHelper {

    public static string WrapText(int maxLineLength, string text2Wrap)
    {
        string newText = string.Empty;
        int newLineCounter = 0;
        for (int i = 0; i < text2Wrap.Length; i++)
        {
            if (newLineCounter > maxLineLength && text2Wrap[i] == ' ')
            {
                newText += '\n';
                newLineCounter = 0;
            }
            else
            {
                newText += text2Wrap[i];
                newLineCounter++;
            }
        }


        return newText;
    }

    public static string FormatText(string heading, string text, int lineMaxLength) {
       return "<b><size=20>" + heading + "\n\n</size></b>"
           + TextHelper.WrapText(lineMaxLength, text);
    }
}
