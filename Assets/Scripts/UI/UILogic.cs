using UnityEngine;

namespace UI
{
    public class UILogic : MonoBehaviour
    {
        public GameObject panelToControl;
        public GameObject jumpTutorialPanel;
        public bool playing;
        public bool paused;
        public GameObject panelTrigger;
        private bool _doOnce;

        float _timer;
        float _worldActualTime;

        private void Start()
        {
            _timer = 0;//On start _timer starts at zero
            playing = true;//sets the bool to playing 
            DisablePanel();//Disables any of our UI panels
        }

        void Update()
        {
            //If the game is playing.
            //The player can press escape to pause the game.
            //Freezing time and pausing the game.
            if (playing)
            {
                _timer += Time.deltaTime;
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    paused = !paused;
                }

            }

                            //Need to add UI timer in game to figure out when exactly events happen
            if (Time.unscaledTime > 3.5 && jumpTutorialPanel.activeInHierarchy && Input.GetKeyDown(KeyCode.Space))
            {
                DisablePanel();
                paused = false;
                panelTrigger.SetActive(false);
            }

            //If the game is paused set the time to zero freezing the game, if it isn't set it to 1 putting it at a normal scale.
            Time.timeScale = paused ? 0 : 1;

            //Reference for our PanelController method in update().
            PanelController();
        }

        //PanelController() goes through all the actions once so it doesn't loop.
        private void PanelController()
        {
            if (_doOnce) return;
            //if time has reached 1.5 seconds.
            if (_timer > 1.5)
            {
                //Enable the panel and stop time completely.
                EnablePanel();
                Time.timeScale = 0;



                //If any key is pressed disable the panel and time returns to the normal scale.
                if (Time.unscaledTime > 2.5 && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)))
                {
                    DisablePanel();
                    Time.timeScale = 1;
                    _doOnce = true;
                }
            }
        }

        //Disables panels by deactivating the game object
        private void DisablePanel()
        {
            panelToControl.SetActive(false);
            jumpTutorialPanel.SetActive(false);
        }

        //Enables panels by activating the game object
        private void EnablePanel()
        {
            panelToControl.SetActive(true);
            //jumpTutorialPanel.SetActive(false);
        }

        //When the player enters the trigger zone pause time.
        //Also set the jumpTutorialPanel object to true so we can see it in the game.
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Touched");
            if (other.CompareTag("UITrigger"))
            {
                jumpTutorialPanel.SetActive(true);
                paused = true;
                Debug.Log("triggered");
            }
        }
    }
}