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
        if (sayingSomething)
            return;

        Lottery<string> lottery = new Lottery<string>();
        switch (message)
        {
            case KimMessageType.Failure:
                lottery.Add("Arrgg!", 1);
                lottery.Add("", 1);
                lottery.Add("Hmmm..", 1);
                break;

            case KimMessageType.Restart:
                lottery.Add("Again.", 1);
                break;

            case KimMessageType.Move:
                //lottery.Add("Move!", 1);
                lottery.Add("", 3);
                break;

            case KimMessageType.Fire:
                lottery.Add("Fire!", 1);
                lottery.Add("Shoot!", 1);
                break;

            case KimMessageType.NewLevel:
                lottery.Add("Looks Easy.", 1);
                break;

            case KimMessageType.LevelCompleted:
                lottery.Add("Nice!", 3);
                lottery.Add("Haha!", 3);
                lottery.Add("GGEZ!", 1);
                break;

            default:
                lottery.Add("", 1);
                break;
        }

        string result = lottery.Pick();

        if (result.IsNullOrEmpty())
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

        text.text = result;

        image.gameObject.SetActive(true);

        this.DelayedCall(duration, delegate ()
        {
            image.gameObject.SetActive(false);
            sayingSomething = false;
        });
    }
}
