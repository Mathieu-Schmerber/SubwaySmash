using System;
using UnityEngine;
using Game.Entities;
using Game.Entities.Player;
using LemonInc.Core.Utilities;
namespace Game.Systems.Tutorial
{
    public  class TutorialManager : MonoBehaviour
    {
        [SerializeField] private GameObject _cameraPosParent;
        [SerializeField] private GameObject _playerPosParent;
        [SerializeField] private Camera _camera;
        [SerializeField] private GameObject _player;
        private int _index = 0;
        private readonly Timer _timer = new();
        public Animator _animator;
        private bool _move;
        private float elapsedTime;
        [SerializeField] private float _moveduration = 3f;
        private Vector3 _currentPlayerPos;
        private Vector3 _currentCamPos;

        private void Awake()
        {
        }

        private void UpdatePositions()
        {
            Debug.Log(_index);
            if (_cameraPosParent.transform.GetChild(_index) != null && _playerPosParent.transform.GetChild(_index) != null)
            {
                _player.GetComponent<PlayerInputProvider>().enabled = false;
                _player.GetComponent<Controller>().enabled = false;
                _player.GetComponent<PlayerStateMachine>().enabled = false;
                _animator.SetFloat("Speed",.5f);
                _timer.Start(_moveduration, false, ResetInputs);
                _currentPlayerPos = _player.transform.position;
                _currentCamPos = _camera.transform.position;
                _player.transform.GetChild(0).transform.LookAt(_playerPosParent.transform.GetChild(_index).transform.position);
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
                var percentageCompleted = elapsedTime / _moveduration;
                _player.transform.position = Vector3.Lerp(_currentPlayerPos, _playerPosParent.transform.GetChild(_index).transform.position, percentageCompleted);
                _camera.transform.position = Vector3.Lerp(_currentCamPos, _cameraPosParent.transform.GetChild(_index).transform.position, percentageCompleted);
            }
        }

        void ResetInputs()
        {
            _move = false;
            elapsedTime = 0;
            _player.GetComponent<PlayerInputProvider>().enabled = true;
            _player.GetComponent<Controller>().enabled = true;
            _player.GetComponent<PlayerStateMachine>().enabled = true;
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

