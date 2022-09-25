using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Core;

namespace Gameplay.Main
{
    public class HandleBar : MonoBehaviour, IHandleBar
    {
        [field: SerializeField] public float CurrentSpeed { get; private set; }
        private float horizontal;
        private void Update()
        {
            horizontal = Input.GetAxisRaw("Horizontal");
        }
        private void LateUpdate()
        {
            Move(horizontal);
        }

        public void Move(float direction)
        {
            transform.Translate(Vector3.right * direction * CurrentSpeed * Time.deltaTime);
        }

        public void SetSpeed(float newSpeed)
        {
            CurrentSpeed = newSpeed;
        }
    }
}