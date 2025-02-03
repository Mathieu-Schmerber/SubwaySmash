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
            _tutorialParts[_tutorialPartIndex].Initialize();
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
                next.OnCompleted += OnCurrentPartCompleted;
                next.Initialize();
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
            //Core.Instance.
        }

        private bool IsLastPart()
        {
            return _tutorialPartIndex >= _tutorialParts.Length - 1;
        }
        
        
    }
}

