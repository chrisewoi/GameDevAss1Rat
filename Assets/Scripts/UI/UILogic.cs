using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace UI
{
    public class UILogic : MonoBehaviour
    {
        public GameObject panelToControl;
        public GameObject jumpTutorialPanel;
        public bool playing;
        public bool paused;
        public GameObject panelTrigger;
        private bool doOnce = false;
        //[SerializeField] Transform player;

        //public bool isActive;

        float _timer;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            _timer = 0;
            playing = true;

            DisablePanel();
        }


        // Update is called once per frame
        void Update()
        {
            if (playing)
            {
                _timer += Time.deltaTime;
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    paused = !paused;
                }

            }
            if (jumpTutorialPanel.activeInHierarchy && Input.anyKeyDown)
            {
                DisablePanel();
                paused = false;
                //Time.timeScale = 1;
                panelTrigger.SetActive(false);
                //jumpTutorialPanel.SetActive(false);
            }
            
            
            Time.timeScale = paused ? 0 : 1;

            PanelController();

            
        }
        
        public void PanelController()
        {
            if (!doOnce)
            {
                if( _timer > 1.5)
                {
                    
                    EnablePanel();
                    Time.timeScale = 0;
                    if (Input.anyKeyDown)
                    {
                        DisablePanel();
                        Time.timeScale = 1;
                        doOnce = true;
                    }
                } 
            }
            
        }

        private void DisablePanel()
        {
            panelToControl.SetActive(false);
            jumpTutorialPanel.SetActive(false);
        }

        private void EnablePanel()
        {
            panelToControl.SetActive(true);
            //jumpTutorialPanel.SetActive(false);
            /*while (panelToControl == isActiveAndEnabled)
            {
                jumpTutorialPanel.SetActive(false);
            }
            jumpTutorialPanel.SetActive(true);*/
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Touched");
            if (other.CompareTag("UITrigger"))
            {
                jumpTutorialPanel.SetActive(true);
                paused = true;
                Debug.Log("triggered");
                /*if (Input.anyKeyDown)
                {
                    DisablePanel();
                    Time.timeScale = 1;
                    jumpTutorialPanel.SetActive(false);
                    //jumpTutorialPanel.SetActive(false);
                }*/
            }
            
        }

        /*private void OnTriggerExit(Collider other)
        {
            panelTrigger.SetActive(false);
        }*/
    }

}