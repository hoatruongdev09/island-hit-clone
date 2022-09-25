using UnityEngine;
using Gameplay.Core;
using System.Collections.Generic;
using deVoid.Utils;
using System;
using Random = UnityEngine.Random;
using System.Collections;

namespace Gameplay.Main
{
    public class GameController : MonoBehaviour, IGameController
    {
        public List<IBall> AliveBalls { get; private set; } = new List<IBall>();

        public uint Score { get; private set; }

        public bool GameStarted { get; private set; }

        public ISpawnController SpawnController => spawnController;

        public float CurrentBallsForce { get; private set; }

        public IGameScript GameScript { get; private set; }

        [SerializeField] private SpawnController spawnController;
        [SerializeField] private float startBallForce = 200f;

        private GameAPI.OnScoreChange onScoreChange;
        private void Awake()
        {
            LoadGameScript();
            RegisterListeners();
        }
        private void Start()
        {
            onScoreChange = Signals.Get<GameAPI.OnScoreChange>();
        }
        private void OnDestroy()
        {
            RemoveListeners();
        }
        private void RegisterListeners()
        {
            Signals.Get<GameAPI.OnStartPlay>().AddListener(OnStartPlay);
            Signals.Get<GameAPI.OnBallHitBottomEdge>().AddListener(OnBallHitBottomEdge);
            Signals.Get<GameAPI.OnBallHitDynamicIsland>().AddListener(OnBallHitDynamicIsland);
            Signals.Get<GameAPI.OnBallHitEdge>().AddListener(OnBallHitEdge);
        }
        private void RemoveListeners()
        {
            Signals.Get<GameAPI.OnStartPlay>().RemoveListener(OnStartPlay);
            Signals.Get<GameAPI.OnBallHitBottomEdge>().RemoveListener(OnBallHitBottomEdge);
            Signals.Get<GameAPI.OnBallHitDynamicIsland>().RemoveListener(OnBallHitDynamicIsland);
            Signals.Get<GameAPI.OnBallHitEdge>().RemoveListener(OnBallHitEdge);
        }

        private void OnStartPlay()
        {
            StartCoroutine(DelayToStartGame());
        }
        private IEnumerator DelayToStartGame(float time = 1f)
        {
            yield return new WaitForSeconds(time);
            StartGame();
        }

        public void AddBall()
        {
            var ball = SpawnController.CreateBall();
            ball.SetCurrentDirection(RandomDirection());
            ball.SetCurrentForce(CurrentBallsForce);
            AliveBalls.Add(ball);
        }

        private void Update()
        {
            OnUpdate(Time.deltaTime);
        }
        private void FixedUpdate()
        {
            OnFixUpdate(Time.fixedDeltaTime);
        }

        public void OnFixUpdate(float fixDelta)
        {
            GameScript.OnFixUpdate(fixDelta);
        }

        public void OnUpdate(float delta)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartGame();
            }
            GameScript.OnUpdate(delta);
        }

        private void OnBallHitEdge(IBall ball, Collision2D collision)
        {
            GameScript.OnBallHitEdge(ball, collision);
        }

        private void OnBallHitDynamicIsland(IBall ball)
        {
            GameScript.OnBallHitDynamicIsland(ball);
        }

        private void OnBallHitBottomEdge(IBall ball)
        {
            GameScript.OnBallHitBottomEdge(ball);

            if (GameScript.IsGameOver())
            {
                EndGame();
            }
        }

        public void SetScore(uint newScore)
        {
            onScoreChange.Dispatch(Score, newScore);
            Score = newScore;
        }

        public void StartGame()
        {
            if (GameStarted) { return; }
            GameStarted = true;
            SetScore(0);
            SetBallsForce(startBallForce);
            AddBall();
        }

        public void EndGame()
        {
            if (!GameStarted) { return; }
            GameStarted = false;
        }
        public Vector2 RandomDirection()
        {
            var x = Random.Range(-1f, 1f);
            var y = Random.Range(-1f, 1f);
            return new Vector2(x, y);
        }

        public void SetBallsForce(float newForce)
        {
            CurrentBallsForce = newForce;
            foreach (var ball in AliveBalls)
            {
                if (ball.IsDead) { continue; }
                ball.SetCurrentForce(CurrentBallsForce);
            }
        }

        public void LoadGameScript()
        {
            GameScript = new NormalGameScript(this);
        }
    }
}