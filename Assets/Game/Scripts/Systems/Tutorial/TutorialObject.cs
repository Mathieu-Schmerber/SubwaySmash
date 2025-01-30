using System;
using Game.Systems.Push;
using UnityEngine;
using LemonInc.Core.Utilities;
using UnityEditor;
using UnityEngine.Serialization;


namespace Game.Systems.Tutorial
{
    public class TutorialObject : PushTriggerBase
    {
        
        private TutorialManager _manager;
        private readonly Timer _timer = new();
        private readonly Timer _timer2 = new();
        public bool _useAsTrigger;
        [Space(20)]
        [SerializeField] private float _timeBeforeChange = 3f;
        [SerializeField] private float _timeBeforeText = 2f;
        private Pushable _pushable;
        
        [Space(20)]
        public bool _isPushable;
        public bool _hasText;
        [HideInInspector] public bool _pushTriggersSteps;
        [HideInInspector]public FollowTutorialObject[] _textObjects;

        private void Awake()
        {
            _manager = FindFirstObjectByType<TutorialManager>();
            if(_isPushable)
                _pushable = GetComponent<Pushable>();
        }

        public override void Trigger(Transform actor)
        {
            if (_useAsTrigger)
                _timer.Start(_timeBeforeChange, false, _manager.ChangeStage);
        }

        private void OnEnable()
        {
            if (_isPushable)
            {
                _pushable.OnPushed += RemoveText;
                if (_pushTriggersSteps)
                {
                    _timer.Start(_timeBeforeChange, false, _manager.ChangeStage);
                }
            }
        }

        void RemoveText()
        {
            foreach (var textObject in _textObjects)
            {
                textObject.gameObject.SetActive(false);
            }
            _timer2.Start(_timeBeforeText, false, RepopText);
        }
        void RepopText()
        {
            foreach (var textObject in _textObjects)
            {
                textObject.gameObject.SetActive(true);
            }
        }

        [CustomEditor(typeof(TutorialObject))]
        public class TutorialObjectEditor: Editor
        {
            SerializedProperty _isPushable;
            SerializedProperty _hasText;
            SerializedProperty _pushTriggersSteps;
            SerializedProperty _textObjects;

            private void OnEnable () 
            {
                // hook up the serialized properties
                _isPushable = serializedObject.FindProperty(nameof(TutorialObject._isPushable));
                _hasText = serializedObject.FindProperty(nameof(TutorialObject._hasText));
                _pushTriggersSteps = serializedObject.FindProperty(nameof(TutorialObject._pushTriggersSteps));
                _textObjects = serializedObject.FindProperty(nameof(TutorialObject._textObjects));
            }
            public override void OnInspectorGUI() 
            {
                // Call normal GUI (displaying "a" and any other variables you might have)
                DrawDefaultInspector();
                // update the current values into the serialized object and propreties
                serializedObject.Update();

                // if the first bool is true
                if (_isPushable.boolValue)
                {
                    // draw the second bool field
                    EditorGUILayout.PropertyField(_pushTriggersSteps);
                }
                if (_hasText.boolValue)
                {
                    EditorGUILayout.PropertyField(_textObjects);
                }

                // Write back changed values
                // This also handles all marking dirty, saving, undo/redo etc
                serializedObject.ApplyModifiedProperties();
                }
            }
        }
    }


