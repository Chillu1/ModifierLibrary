using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CustomGameNamespace
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private Text timeText;//Unused for now

        private GameObject pausePanel;

        private Button pauseResumeButton;
        private Button mainMenuButton;
        private Button quitButton;

        private GameObject canvas;

        private GameObject pausePanelPrefab;


        public void Start()
        {
            pausePanelPrefab = Resources.Load<GameObject>("Prefabs/UI/PausePanel");
            canvas = FindObjectOfType<Canvas>().gameObject;

            pausePanel = Instantiate(pausePanelPrefab, Vector3.zero, Quaternion.identity);
            pausePanel.transform.SetParent(canvas.transform, false);
            pauseResumeButton = pausePanel.transform.Find("ResumeButton").GetComponent<Button>();
            mainMenuButton = pausePanel.transform.Find("MainMenuButton").GetComponent<Button>();
            quitButton = pausePanel.transform.Find("QuitButton").GetComponent<Button>();

            pauseResumeButton.onClick.AddListener(delegate { GameController.TimeController.PauseOrResume(true); });
            mainMenuButton.onClick.AddListener(delegate { SceneManager.LoadScene("mainmenu", LoadSceneMode.Single); GameController.TimeController.PauseOrResume(true); RemoveAllListeners(); });
            quitButton.onClick.AddListener(delegate { SceneController.QuitGame(); });
        }

        public void UpdateTimeText(int seconds)
        {
            timeText.text = seconds.ToString("D2");
        }

        public void OnPauseOrResume()
        {
            if (pausePanel.activeSelf == TimeController.Paused)
            {
                Debug.LogError("Pause menu is on/off while the game itself is/isnt paused");
                return;
            }
            pausePanel.SetActive(TimeController.Paused);
        }

        public void RemoveAllListeners()
        {
            pauseResumeButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.RemoveAllListeners();
            quitButton.onClick.RemoveAllListeners();
        }
    }
}