using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ChangeVideoOnPlatform : MonoBehaviour
{
    private int Platform = -1;
    [SerializeField] private VideoClip videoAndroid;
    [SerializeField] private VideoClip videoPC;
    [SerializeField] private VideoPlayer videoPlayer;

    void Awake()
    {
        Platform = (int)Environment.OSVersion.Platform;
        videoPlayer.clip = Platform == 4 ? videoAndroid : videoPC;
    }
}
