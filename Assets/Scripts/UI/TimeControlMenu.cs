using UnityEngine;
using UnityEngine.UI;

public class TimeControlMenu : MonoBehaviour
{
    [SerializeField] private Button _pauseBtn;
    [SerializeField] private Button _resumeBtn;
    [SerializeField] private Button _2xBtn;

    private void Start()
    {
        _pauseBtn.onClick.AddListener(SwitchPause);
        _resumeBtn.onClick.AddListener(() => Time.timeScale = 1f);
        _2xBtn.onClick.AddListener(() => Time.timeScale = 2f);
    }

    public static void SwitchPause()
    {
        Time.timeScale = Time.timeScale > 0f ? 0f : 1f;
    }
}
