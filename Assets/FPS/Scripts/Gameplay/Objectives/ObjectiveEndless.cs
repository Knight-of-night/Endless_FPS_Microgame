using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class ObjectiveEndless : Objective
    {
        [Tooltip("How many enemies in the senene")]
        public int KillsToCompleteObjective = 5;

        [Tooltip("Start sending notification about remaining enemies when this amount of enemies is left")]
        public int NotificationEnemiesRemainingThreshold = 3;

        [Tooltip("Break time between rounds")]
        public int BreakTime = 3;

        public GameObject[] Weapons;

        public GameObject Enemy_HoverBot;
        public GameObject Enemy_Turret;
        
        public GameObject[] SpawnPosition;
        public GameObject[] TurretSpawnPosition;

        int m_KillTotal;
        int roundnum = 1;

        protected override void Start()
        {
            base.Start();

            EventManager.AddListener<EnemyKillEvent>(OnEnemyKilled);

            // set a title and description specific for this type of objective, if it hasn't one
            if (string.IsNullOrEmpty(Title))
                Title = "Eliminate all the enemies";

            if (string.IsNullOrEmpty(Description))
                Description = GetUpdatedCounterAmount();

            DisplayMessageEvent displayMessage = Events.DisplayMessageEvent;
            displayMessage.Message = "Round 1";
            displayMessage.DelayBeforeDisplay = 0.0f;
            EventManager.Broadcast(displayMessage);
        }

        void OnEnemyKilled(EnemyKillEvent evt)
        {
            if (IsCompleted)
                return;

            m_KillTotal++;

            KillsToCompleteObjective = evt.RemainingEnemyCount + m_KillTotal;

            int targetRemaining = evt.RemainingEnemyCount;

            // update the objective text according to how many enemies remain to kill
            if (targetRemaining <= 0)
            {
                UpdateObjective(string.Empty, GetUpdatedCounterAmount(), string.Empty);

                DisplayMessageEvent displayMessage = Events.DisplayMessageEvent;
                displayMessage.Message = "Stage Clear";
                displayMessage.DelayBeforeDisplay = 0.0f;
                EventManager.Broadcast(displayMessage);

                targetRemaining = (roundnum + 1) / 10 + 4;
                Invoke("NextRound", BreakTime);
            }
            else if (targetRemaining == 1)
            {
                string notificationText = NotificationEnemiesRemainingThreshold >= targetRemaining
                    ? "One enemy left"
                    : string.Empty;
                UpdateObjective(string.Empty, GetUpdatedCounterAmount(), notificationText);
            }
            else
            {
                // create a notification text if needed, if it stays empty, the notification will not be created
                string notificationText = NotificationEnemiesRemainingThreshold >= targetRemaining
                    ? targetRemaining + " enemies to kill left"
                    : string.Empty;

                UpdateObjective(string.Empty, GetUpdatedCounterAmount(), notificationText);
            }
        }

        string GetUpdatedCounterAmount()
        {
            return m_KillTotal + " / " + KillsToCompleteObjective;
        }

        void OnDestroy()
        {
            EventManager.RemoveListener<EnemyKillEvent>(OnEnemyKilled);
        }

        void NextRound()
        {
            roundnum += 1;
            string notificationText = "Round " + roundnum;
            DisplayMessageEvent displayMessage = Events.DisplayMessageEvent;
            displayMessage.Message = notificationText;
            displayMessage.DelayBeforeDisplay = 0.0f;
            EventManager.Broadcast(displayMessage);

            for (var i = 0; i < (roundnum / 10 + 4); i++)
            {
                GameObject enemy = Instantiate(Enemy_HoverBot);
                enemy.transform.position = SpawnPosition[Random.Range(0, SpawnPosition.Length)].transform.position;
            }
            
            GameObject enemy_t = Instantiate(Enemy_Turret);
            enemy_t.transform.position = TurretSpawnPosition[Random.Range(0, TurretSpawnPosition.Length)].transform.position;

            if (roundnum == 2)
            {
                GameObject weapon = Instantiate(Weapons[0]);
                weapon.transform.position = new Vector3(0.75f, 5.0f, -39f);
                
                DisplayMessageEvent message = Events.DisplayMessageEvent;
                message.Message = "New Weapon Unlocked Somewhere";
                message.DelayBeforeDisplay = 0.0f;
                EventManager.Broadcast(message);
            }

            if (roundnum == 5)
            {
                GameObject weapon = Instantiate(Weapons[1]);
                weapon.transform.position = new Vector3(0.75f, 5.0f, -39f);
                
                DisplayMessageEvent message = Events.DisplayMessageEvent;
                message.Message = "New Weapon Unlocked Somewhere";
                message.DelayBeforeDisplay = 0.0f;
                EventManager.Broadcast(message);
            }

            if (roundnum == 10)
            {
                GameObject weapon = Instantiate(Weapons[2]);
                weapon.transform.position = new Vector3(0.75f, 5.0f, -39f);
                
                DisplayMessageEvent message = Events.DisplayMessageEvent;
                message.Message = "New Weapon Unlocked Somewhere";
                message.DelayBeforeDisplay = 0.0f;
                EventManager.Broadcast(message);
            }
        }
    }
}