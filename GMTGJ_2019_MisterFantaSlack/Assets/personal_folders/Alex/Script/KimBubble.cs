using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum KimMessageType
{
    Failure,
    Restart,
    Move,
    Fire,
    NewLevel,
    LevelCompleted
}

public class KimBubble : MonoBehaviour
{
    public Sprite normalBubble;
    public Sprite angryBubble;

    public Image image;
    public TextMeshProUGUI text;

    public bool sayingSomething = false;

    public void Say(KimMessageType message, bool isAngry, float duration)
    {
        List<string> textToSay = new List<string>();

        switch (message)
        {
            case KimMessageType.Failure:
                textToSay.Add("Failure!");
                break;
            case KimMessageType.Restart:
                textToSay.Add("Again.");
                break;
            case KimMessageType.Move:
                textToSay.Add("Move!");
                break;
            case KimMessageType.Fire:
                textToSay.Add("Fire!");
                break;
            case KimMessageType.NewLevel:
                textToSay.Add("Looks Easy.");
                break;
            case KimMessageType.LevelCompleted:
                textToSay.Add("Nice!");
                break;
            default:
                textToSay.Add("");
                break;
        }

        if (sayingSomething)
            return;

        sayingSomething = true;

        if (isAngry)
        {
            image.sprite = angryBubble;
            text.color = Color.red;
            // Trigger kim facher
        }
        else
        {
            image.sprite = normalBubble;
            text.color = Color.black;
        }

        text.text = textToSay[Random.Range(0, textToSay.Count-1)];

        image.gameObject.SetActive(true);

        this.DelayedCall(duration, delegate () {
            image.gameObject.SetActive(false);
            sayingSomething = false;
        });
    }
}
