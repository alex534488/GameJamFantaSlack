using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KimBubble : MonoBehaviour
{
    public Sprite normalBubble;
    public Sprite angryBubble;

    public Image image;
    public TextMeshProUGUI text;

    public bool sayingSomething = false;

    public void Say(string message, bool isAngry, float duration)
    {
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

        text.text = message;

        image.gameObject.SetActive(true);

        this.DelayedCall(duration, delegate () {
            image.gameObject.SetActive(false);
            sayingSomething = false;
        });
    }
}
