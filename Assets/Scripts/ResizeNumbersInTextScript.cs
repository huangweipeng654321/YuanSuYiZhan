using UnityEngine;
using UnityEngine.UI;

public class ResizeNumbersInTextScript : MonoBehaviour
{
    public void ResizeText(Text targetText, int normalSize, int numberSize)
    {
        string originalText = targetText.text;
        string processedText = ProcessText(originalText, normalSize, numberSize);
        targetText.supportRichText = true;  // 确保支持富文本
        targetText.text = processedText;
    }

    string ProcessText(string input, int normalFontSize, int numberFontSize)
    {
        // 使用正则表达式找到所有的数字，并为其添加不同的字体大小标签
        string pattern = @"(\d+)";
        string replacement = $"<size={numberFontSize}>$1</size><size={normalFontSize}>";
        string result = System.Text.RegularExpressions.Regex.Replace(input, pattern, replacement);

        // 添加富文本的结尾标签以确保格式正确
        result += $"</size>";
        return result;
    }
}
