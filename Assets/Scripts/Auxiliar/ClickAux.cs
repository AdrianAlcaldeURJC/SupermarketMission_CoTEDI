using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickAux
{
    private List<string> pairs = new List<string>();
    public TimerAux timerAux;
    public int timerIndex;

    public void SaveClickOrTouchInfo()
    {
        string clickInfo;
        if (Input.GetMouseButtonDown(0))
        {
            clickInfo = $"({timerAux.elapsedTime[timerIndex]}, {Input.mousePosition})";
            pairs.Add(clickInfo);

        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            clickInfo = $"({timerAux.elapsedTime[timerIndex]}, {Input.GetTouch(0).position})";
            pairs.Add(clickInfo);

        }

    }

    public string GetClickInfo()
    {
        return "[" + string.Join(", ", pairs) + "]";
    }
}
