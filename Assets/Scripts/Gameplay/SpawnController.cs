using UnityEngine;
using Gameplay.Core;

namespace Gameplay.Main
{
    public class SpawnController : MonoBehaviour, ISpawnController
    {
        [SerializeField] private Ball ballPrefab;
        [SerializeField] private Transform ballsHolder;
        [SerializeField] private Transform ballStartTransform;


        public IBall CreateBall()
        {
            var ball = Instantiate(ballPrefab, ballStartTransform.position, Quaternion.identity, ballsHolder);
            return ball;
        }
    }
}