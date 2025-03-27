using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace UI
{
    public class UILogic : MonoBehaviour
    {
        public GameObject panelToControl;
        public bool playing;
        public bool paused;
        public GameObject panelTrigger;

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
            
            Time.timeScale = paused ? 0 : 1;
        }

        private void DisablePanel()
        {
            
             panelToControl.SetActive(false);
        }

        private void EnablePanel()
        {
            panelToControl.SetActive(true);
        }

        public void OnTriggerEnter(Collider trigger)
        {
            if (trigger.transform.CompareTag("UITrigger"))
            {
                EnablePanel();
                playing = false;
            }
        }
    }
    
}