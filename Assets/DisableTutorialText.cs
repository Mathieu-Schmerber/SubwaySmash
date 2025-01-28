using System;
using UnityEngine;
using Game.Systems.Push;

public class DisableTutorialText : MonoBehaviour
{
    private Pushable _pushable;
    [SerializeField] FollowTutorialObject[] _textObjects;
    
    private void Awake()
    {
        _pushable = GetComponent<Pushable>();
    }

    private void OnEnable()
    {
            _pushable.OnPushed += RemoveText;
    }

    void RemoveText()
    {
        foreach (var textObject in _textObjects)
        {
            textObject.gameObject.SetActive(false);
        }
    }
}
