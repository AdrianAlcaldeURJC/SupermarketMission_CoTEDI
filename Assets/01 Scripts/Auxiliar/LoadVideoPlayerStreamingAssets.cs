using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;

public class LoadVideoPlayerAssets : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] string videoName = "";

    void Awake()
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, videoName);
        videoPlayer.url = filePath;
    }
}
