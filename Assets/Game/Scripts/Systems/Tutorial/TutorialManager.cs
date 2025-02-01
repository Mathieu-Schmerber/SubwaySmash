using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Entities;
using Game.Entities.Player;
using Game.Systems.Waypoint;
using LemonInc.Core.Utilities;
using Pixelplacement;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMPro;

namespace Game.Systems.Tutorial
{
    
    [Serializable]
    public class TutorialPart
    {
        public Transform CameraPositions;
        public Transform PlayerPositions;
        public Animator[] LinkedDoors;
        public GameObject CurrentPart;
        
    }
    public  class TutorialManager : MonoBehaviour
    {
        
        [SerializeField] private Transform _camera;
        [SerializeField] private GameObject _player;
        private int _index = 0;
        private int _indexChild = 0;
        private readonly Timer _timer = new();
        public Animator _animatorPlayer;
        [TableList]
        public TutorialPart[] _tutorialParts;
        
        private bool _move;
        private float elapsedTime;
        private float elapsedTimep;
        [SerializeField] private float _moveduration = 3f;
        private Vector3 _currentPlayerPos;

        private void Awake()
        {
        }

        private void UpdatePositions()
        {
            Debug.Log(_index);
            if (_tutorialParts.Length >= _index && _tutorialParts[_index].PlayerPositions != null)
            {
                if(_tutorialParts[_index+1]!= null)
                    _tutorialParts[_index+1].CurrentPart.SetActive(true);
                Core.Instance._levelExists = FindObjectsByType<Exit>(FindObjectsSortMode.None);
                Core.AlertSystem.ResetAlert();
                _player.GetComponent<PlayerInputProvider>().enabled = false;
                _player.GetComponent<Controller>().enabled = false;
                _player.GetComponent<PlayerStateMachine>().enabled = false;
                _animatorPlayer.SetFloat("Speed",.5f);
                _timer.Start(_moveduration, false, ResetInputs);
                _currentPlayerPos = _player.transform.position;
                foreach (Animator animator in _tutorialParts[_index].LinkedDoors)
                    animator.SetTrigger("Open");
                
                Tween.Position(_camera.transform, _tutorialParts[_index].CameraPositions.transform.position, _moveduration, 0, Tween.EaseInOut);
                _move = true;
            }
            else
            {
                EndTutorial();
            }
        }

        private void Update()
        {
            if (_move)
            {
                elapsedTime += Time.deltaTime;
                elapsedTimep += Time.deltaTime;
                var count = _tutorialParts[_index].PlayerPositions.transform.childCount;
                var percentageCompletedplayer = elapsedTimep * count  / _moveduration;
                var percentageCompletedcam = elapsedTime / _moveduration;
                

                
                if (percentageCompletedplayer >= 1 && _indexChild < count-1)
                {
                    elapsedTimep = 0;
                    percentageCompletedplayer--;
                    _indexChild++;
                    Debug.Log(count);
                    _currentPlayerPos = _player.transform.position;
                }
                
                var target = _tutorialParts[_index].PlayerPositions.transform.GetChild(_indexChild).transform.position;
                _player.transform.GetChild(0).transform.LookAt(target);
                Debug.Log(target);
                _player.transform.position = Vector3.Lerp(_currentPlayerPos, target, percentageCompletedplayer);
            }
        }

        void ResetInputs()
        {
            
            _move = false;
            elapsedTime = 0;
            elapsedTimep = 0;
            _player.GetComponent<PlayerInputProvider>().enabled = true;
            _player.GetComponent<Controller>().enabled = true;
            _player.GetComponent<PlayerStateMachine>().enabled = true;
            foreach (Animator animator in _tutorialParts[_index].LinkedDoors)
                animator.SetTrigger("Close");
            _tutorialParts[_index].CurrentPart.SetActive(false);
            _index++;
        }
        // Update is called once per frame
        public void ChangeStage()
        {
            UpdatePositions();
            
        }

        public void EndTutorial()
        {
            //end tutorial
        }

    }
}

