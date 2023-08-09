using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.IO;
using System.Collections.Generic;
using TMPro;

public class VideoPathManager : MonoBehaviour
{
    public TMP_InputField videoPathInput;
    public Button savePathButton;
    public Canvas canvas;

    private const string VideoPathKey = "VideoPath";
    private bool isInitialSetupDone = false;

    void Start()
    {
        PlayerPrefs.DeleteKey(VideoPathKey);
        string savedVideoPath = PlayerPrefs.GetString(VideoPathKey);
        if (!string.IsNullOrEmpty(savedVideoPath))
        {
            isInitialSetupDone = true;
            videoPathInput.text = savedVideoPath;
            EnableVideoController(savedVideoPath);
            videoPathInput.text = GetDefaultVideoPath();
        }

        savePathButton.onClick.AddListener(SaveVideoPath);
    }

    void SaveVideoPath()
    {
        string path = videoPathInput.text.Trim();
        if (Directory.Exists(path))
        {
            canvas.gameObject.SetActive(false);
            PlayerPrefs.SetString(VideoPathKey, path);
            EnableVideoController(path);
            Debug.Log("Video path saved: " + path);
        }
        else
        {
            canvas.gameObject.SetActive(true);
            Debug.Log("Invalid video path");
        }
    }

    void EnableVideoController(string path)
    {
        VideoController videoController = FindObjectOfType<VideoController>();
        if (videoController != null)
        {
            videoController.videosFolderPath = path;
            videoController.enabled = true;
            videoController.SendMessage("LoadVideoFiles");
            canvas.gameObject.SetActive(false);
        }
    }
    string GetDefaultVideoPath()
    {
        string defaultVideoPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyVideos);
        return defaultVideoPath;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            canvas.gameObject.SetActive(true);
        }
    }
}