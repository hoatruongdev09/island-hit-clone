using UnityEngine;
using Gameplay.Core;
using System.Collections.Generic;
using deVoid.Utils;
using System;
using Random = UnityEngine.Random;
using System.Collections;
using MoreMountains.NiceVibrations;
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

        public IBackgroundController BackgroundController => backgroundController;

        public uint MaxBallInTime => maxBallInTime;

        public float MaxBallForce => maxForce;

        [SerializeField] private SpawnController spawnController;
        [SerializeField] private BackgroundController backgroundController;
        [SerializeField] private uint maxBallInTime = 5;
        [SerializeField] private float maxForce = 8;
        [SerializeField] private float startBallForce = 4;
        [SerializeField] private uint scoreStep = 5;
        [SerializeField] private float forcePerStep = 2;

        private GameAPI.OnScoreChange onScoreChange;
        private void Awake()
        {
#if !UNITY_EDITOR
            Application.targetFrameRate = 60;
#endif
            LoadGameScript();
            RegisterListeners();
        }
        private void Start()
        {
            onScoreChange = Signals.Get<GameAPI.OnScoreChange>();
            LoadAccountData();
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }
        private void RegisterListeners()
        {
            Signals.Get<GameAPI.OnStartPlay>().AddListener(OnStartPlay);
            Signals.Get<GameAPI.OnReplay>().AddListener(OnReplay);
            Signals.Get<GameAPI.OnBallHitBottomEdge>().AddListener(OnBallHitBottomEdge);
            Signals.Get<GameAPI.OnBallHitDynamicIsland>().AddListener(OnBallHitDynamicIsland);
            Signals.Get<GameAPI.OnBallHitEdge>().AddListener(OnBallHitEdge);
            Signals.Get<GameAPI.OnChangeUseHaptic>().AddListener(OnChangeUseHaptic);
            Signals.Get<GameAPI.OnChangeUseSfx>().AddListener(OnChangeUseSfx);
        }
        private void RemoveListeners()
        {
            Signals.Get<GameAPI.OnStartPlay>().RemoveListener(OnStartPlay);
            Signals.Get<GameAPI.OnReplay>().RemoveListener(OnReplay);
            Signals.Get<GameAPI.OnBallHitBottomEdge>().RemoveListener(OnBallHitBottomEdge);
            Signals.Get<GameAPI.OnBallHitDynamicIsland>().RemoveListener(OnBallHitDynamicIsland);
            Signals.Get<GameAPI.OnBallHitEdge>().RemoveListener(OnBallHitEdge);
            Signals.Get<GameAPI.OnChangeUseHaptic>().RemoveListener(OnChangeUseHaptic);
            Signals.Get<GameAPI.OnChangeUseSfx>().RemoveListener(OnChangeUseSfx);
        }


        private void OnReplay()
        {
            StartCoroutine(DelayToStartGame());
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
            ball.SetCurrentDirection(RandomDirection().normalized);
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
            Vibrate(HapticTypes.SoftImpact);
        }

        private void OnBallHitDynamicIsland(IBall ball)
        {
            GameScript.OnBallHitDynamicIsland(ball);
            Vibrate(HapticTypes.Success);
        }

        private void OnBallHitBottomEdge(IBall ball)
        {
            GameScript.OnBallHitBottomEdge(ball);
            Vibrate(HapticTypes.Warning);
            if (GameScript.IsGameOver())
            {
                Vibrate(HapticTypes.Failure);
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
            ResetGame();
            AddBall();
        }

        public void EndGame()
        {
            if (!GameStarted) { return; }
            GameStarted = false;
            AccountData.lastGameScore = Score;
            if (AccountData.lastGameScore > AccountData.bestGameScore)
            {
                AccountData.isBestScore = true;
                AccountData.bestGameScore = Score;
                SaveAccountData();
            }
            else
            {
                AccountData.isBestScore = false;
            }
            SfxManager.Instance.PlaySfx(SfxManager.SfxName.FailHit);
            AdServerController.Instance?.ShowInterstitialAd();
            DG.Tweening.DOVirtual.DelayedCall(1, () =>
            {
                Signals.Get<GameAPI.OnGameEnd>().Dispatch();
            });
        }
        public Vector2 RandomDirection()
        {
            var x = Random.Range(-0.5f, 0.5f);
            var y = Random.Range(0.5f, 1f) * Mathf.Sign(Random.Range(-1, 1));
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

        public uint GetScoreStep()
        {
            return scoreStep;
        }

        public float GetForceIncreasePerStep()
        {
            return forcePerStep;
        }

        private void LoadAccountData()
        {
            AccountData.bestGameScore = (uint)PlayerPrefs.GetInt(AccountData.BEST_SCORE_KEY, 0);
            AccountData.useHaptic = PlayerPrefs.GetInt(AccountData.USE_HAPTIC, 1) == 1;
            AccountData.useSfx = PlayerPrefs.GetInt(AccountData.USE_SFX, 1) == 1;
        }
        private void SaveAccountData()
        {
            PlayerPrefs.SetInt(AccountData.BEST_SCORE_KEY, (int)AccountData.bestGameScore);
            PlayerPrefs.SetInt(AccountData.USE_HAPTIC, AccountData.useHaptic ? 1 : 0);
            PlayerPrefs.SetInt(AccountData.USE_SFX, AccountData.useSfx ? 1 : 0);
        }

        public void ResetGame()
        {
            SetScore(0);
            BackgroundController.ResetToDefaultColor();
            SetBallsForce(startBallForce);
            AliveBalls.Clear();
        }


        private void OnChangeUseSfx(bool obj)
        {
            SaveAccountData();
        }

        private void OnChangeUseHaptic(bool obj)
        {
            SaveAccountData();
        }

        private void Vibrate(HapticTypes type)
        {
            if (AccountData.useHaptic)
            {
                if (MMVibrationManager.HapticsSupported())
                {
                    MMVibrationManager.Haptic(type);
                }
                else
                {
                    MMVibrationManager.Vibrate();
                }
            }
        }
    }

}