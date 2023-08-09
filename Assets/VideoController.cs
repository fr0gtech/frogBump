using UnityEngine;
using UnityEngine.Video;
using System.IO;
using System.Collections.Generic;

public class VideoController : MonoBehaviour
{
    public Camera displayCamera;
    public VideoPlayer videoPlayer;

    public string videosFolderPath;
    private List<string> videoFilePaths = new List<string>();
    private int currentVideoIndex = 0;
    private int shuffledIndex = 0; 
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoEnd;

        LoadVideoFiles();
    }

    void LoadVideoFiles()
    {
        if (!string.IsNullOrEmpty(videosFolderPath) && Directory.Exists(videosFolderPath))
        {
            videoFilePaths.AddRange(GetVideoFilesInFolder(new DirectoryInfo(videosFolderPath)));
            videoFilePaths.Shuffle();
            PlayNextVideo();
        }
        else
        {
            Debug.Log("Videos folder path does not exist: " + videosFolderPath);
        }
    }

    List<string> GetVideoFilesInFolder(DirectoryInfo directory)
    {
        List<string> videoList = new List<string>();

        foreach (FileInfo file in directory.GetFiles("*.mp4"))
        {
            videoList.Add(file.FullName);
        }

        foreach (DirectoryInfo subDirectory in directory.GetDirectories())
        {
            videoList.AddRange(GetVideoFilesInFolder(subDirectory));
        }

        return videoList;
    }

    void PlayNextVideo()
    {
        if (videoFilePaths.Count > 0 && shuffledIndex < videoFilePaths.Count)
        {
            videoPlayer.url = "file://" + videoFilePaths[shuffledIndex];
            videoPlayer.Play();

            shuffledIndex++;
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        PlayNextVideo();

        if (shuffledIndex >= videoFilePaths.Count)
        {
            Debug.Log("Reshuffling video list...");
            videoFilePaths.Shuffle();
            shuffledIndex = 0;
        }
    }
}
