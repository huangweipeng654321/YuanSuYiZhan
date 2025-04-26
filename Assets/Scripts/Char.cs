using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class TextNumberResizer : MonoBehaviour
{
    public Text textComponent; // 引用你想要调整的文本组件
    public int smallerSize = 20; // 较小字号的值
    public void ResizeNumbers()
    {
        string originalText = textComponent.text;
        string newText = "";
        foreach (char c in originalText)
        {
            if (char.IsDigit(c))
            {
                newText += $"<size={smallerSize}>{c}</size>"; // 使用富文本标记来设置较小字号
            }
            else
            {
                newText += c;
            }
        }

        textComponent.text = newText;
    }
}