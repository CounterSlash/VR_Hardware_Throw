using System;
using UnityEngine;

namespace VRThrow.Gameplay.ServerClasses
{
    [Serializable]
    public struct ComponentPosition
    {
        [SerializeField] private Vector3 localPosition;
        [SerializeField] private Vector3 localEulerAngles;

        public Vector3 LocalPosition => localPosition;
        public Quaternion LocalRotation => Quaternion.Euler(localEulerAngles);
    }
}
