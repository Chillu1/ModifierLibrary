using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace ComboSystem
{
    public class MainMenu : MonoBehaviour
    {

        [SerializeField] private Button playButton;
        [SerializeField] private Button quitButton;

        //Play game
        //Quit 
        void Start()
        {
            playButton.onClick.AddListener(delegate { PlayGame(); });
            quitButton.onClick.AddListener(delegate { SceneController.QuitGame(); });
        }

        void Update()
        {

        }


        private void PlayGame()
        {
            SceneManager.LoadScene("gameplayscene", LoadSceneMode.Single);
        }

        /*private void NewGame()
        {

        }
        private void LoadGame()
        {

        }*/

    }
}