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
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private TutorialPart[] _tutorialParts;
        private int _tutorialPartIndex = 0;

        private void Awake()
        {
            var current = _tutorialParts[_tutorialPartIndex];
            SubscribeAndInitialize(current);
        }

        private void SubscribeAndInitialize(TutorialPart tutorialPart)
        {
            tutorialPart.OnCompleted += OnCurrentPartCompleted;
            tutorialPart.Initialize();
        }

        private TutorialPart GoNextTutorialPart()
        {
            var current = _tutorialParts[_tutorialPartIndex];
            current.OnCompleted -= OnCurrentPartCompleted;
            current.Exit();
            
            if (!IsLastPart())
            {
                _tutorialPartIndex++;
                var next = _tutorialParts[_tutorialPartIndex];
                SubscribeAndInitialize(next);
                return next; 
            }
            throw new Exception("Already reached the last part.");
        }

        private void OnCurrentPartCompleted()
        {
            if (!IsLastPart())
            {
                GoNextTutorialPart(); 
            }
            else
            {
                EndTutorial();
            }
            
        }

        private void EndTutorial()
        {
            Core.Instance.LoadNextStage();
        }

        private bool IsLastPart()
        {
            return _tutorialPartIndex >= _tutorialParts.Length - 1;
        }
        
        
    }
}

