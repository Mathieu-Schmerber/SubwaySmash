using System;
using UnityEngine;

public class FollowTutorialObject : MonoBehaviour
{
    [SerializeField] private GameObject _tutorialObject;
    [SerializeField] private Vector3 _offset;

    private void Update()
    {
        transform.position = _tutorialObject.transform.position + _offset;
    }
}
