using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerAux : MonoBehaviour
{
    public List<float> elapsedTime = new List<float>();
    public List<bool> isCounting = new List<bool>();

    void Update()
    {
        for (int i = 0; i < isCounting.Count; i++)
        {
            if (isCounting[i])
            {
                elapsedTime[i] += Time.deltaTime;
            }
        }

    }

    public int InitTimer()
    {
        isCounting.Add(false);
        elapsedTime.Add(0.0f);
        return isCounting.Count - 1;
    }

    public void StartTimer(int index)
    {
        isCounting[index] = true;
        elapsedTime[index] = 0f;
    }

    // Restart timer and stops the counting
    public void RestartTimer(int index)
    {
        isCounting[index] = false;
        elapsedTime[index] = 0f;
    }

    public void StopTimer(int index)
    {
        isCounting[index]   = false;
    }

    public void StartAllTimers()
    {
        for (int i = 0; i < isCounting.Count; i++)
        {
            isCounting[i] = true;
            elapsedTime[i] = 0f;
        }
    }

    // Restart all timers but without resetting the time

    public void RestartAllTimers()
    {
        for (int i = 0; i < isCounting.Count; i++)
        {
            isCounting[i] = true;
        }
    }

    public void StopAllTimers()
    {
        for (int i = 0; i < isCounting.Count; i++)
        {
            isCounting[i] = false;
        }
    }
}
