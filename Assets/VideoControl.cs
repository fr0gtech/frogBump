using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class VideoControl : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public TextMeshProUGUI playbackSpeedText;
    public TextMeshProUGUI volumeText;
    private bool isMuted = false;
    private float originalPlaybackSpeed;
    private float playbackSpeedIncrement = 0.1f;
    private float displayTimer = 3f;
    private bool isDisplayingUI = true;
    private float volumeIncrement = 0.1f;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        originalPlaybackSpeed = videoPlayer.playbackSpeed;
        float newVolume = Mathf.Clamp01(videoPlayer.GetDirectAudioVolume(0) + -0.9f);
        videoPlayer.SetDirectAudioVolume(0, newVolume);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMute();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            ChangePlaybackSpeed(playbackSpeedIncrement);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangePlaybackSpeed(-playbackSpeedIncrement);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            AdjustVolume(volumeIncrement);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            AdjustVolume(-volumeIncrement);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        UpdateUI();

        if (isDisplayingUI)
        {
            displayTimer -= Time.deltaTime;
            if (displayTimer <= 0f)
            {
                HideUI();
            }
        }
    }

    void ToggleMute()
    {
        isMuted = !isMuted;
        videoPlayer.SetDirectAudioMute(0, isMuted);
        displayTimer = 3f;
        ShowUI();
    }

    void ChangePlaybackSpeed(float speedChange)
    {
        originalPlaybackSpeed = originalPlaybackSpeed + speedChange;
        float newSpeed = Mathf.Clamp(originalPlaybackSpeed, 0.1f, 10f);
        videoPlayer.playbackSpeed = newSpeed;
        displayTimer = 3f;
        ShowUI();
    }

    void AdjustVolume(float volumeChange)
    {
        float newVolume = Mathf.Clamp01(videoPlayer.GetDirectAudioVolume(0) + volumeChange);
        videoPlayer.SetDirectAudioVolume(0, newVolume);
        displayTimer = 3f;
        ShowUI();
    }

    void UpdateUI()
    {
        playbackSpeedText.text = "Playback Speed: " + originalPlaybackSpeed.ToString("F2");
        volumeText.text = "Volume: " + (isMuted ? "Muted" : videoPlayer.GetDirectAudioVolume(0).ToString("F2"));
    }

    void ShowUI()
    {
        isDisplayingUI = true;
        playbackSpeedText.gameObject.SetActive(true);
        volumeText.gameObject.SetActive(true);
    }

    void HideUI()
    {
        isDisplayingUI = false;
        playbackSpeedText.gameObject.SetActive(false);
        volumeText.gameObject.SetActive(false);
    }
}