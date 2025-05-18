using System.Collections.Generic;
using GimGim.EventSystem;
using GimGim.Game;
using GimGim.Player;
using UnityEngine;

namespace GimGim.Gotchi {
    public class GotchiBehaviorTree : Tree {
        public GameObject gotchi;
        public BehaviorTreeOptions options;

        private IEventSubscription _playerDeathSub;
        private bool _playerDied;

        private float _lastAttackTime = -100f;
        
        private void OnEnable() {
            _playerDeathSub = NotificationEventSystem.Subscribe(new EventSubscription<PlayerDiedEvent>(OnPlayerDeath));
        }

        private void OnDisable() {
            NotificationEventSystem.Unsubscribe(_playerDeathSub);
        }

        /// <summary>
        /// GotchiBehavior has two branches, one for the peace state and the other for the attack state.
        /// IdleNode is a fallback option.
        /// </summary>
        /// <returns></returns>
        protected override Node SetupTree() {
            Node root = new Selector(new List<Node> {
                SetupPeaceBranch(),
                SetupAttackBranch(),
                new ActionNode(() => NodeState.Running)
            });

            return root;
        }

        #region Attack branch behavior
        private Node SetupAttackBranch() {
            Node attackRoamingSequence = new Sequence(new List<Node> {
                new ConditionNode(IsAttackOnCooldown),
                
            });
            
            Node attackSequence = new Sequence(new List<Node> {
                
            });
            
            Node fightSelector = new Selector(new List<Node> {
                attackRoamingSequence,
                attackSequence
            });
            
            Node root = new Sequence(new List<Node> {
                new ConditionNode(IsInFightState),
                new ConditionNode(IsPlayerDead),
                fightSelector
            });

            return root;
        }
        #endregion

        #region Peace branch behavior
        private Node SetupPeaceBranch() {
            Node root = new Sequence(new List<Node>{
                new ConditionNode(IsInPeaceState),
                
            });
            
            return root;
        }
        #endregion

        #region Callbacks
        private void OnPlayerDeath(PlayerDiedEvent e) {
            _playerDied = true;
        }
        #endregion

        #region Condition Methods

        private bool IsInPeaceState() => GameStateManager.GetGameStateType() == GameStateType.Peace;
        private bool IsInFightState() => GameStateManager.GetGameStateType() == GameStateType.Fight;
        private bool IsPlayerDead() => _playerDied;

        private bool IsAttackOnCooldown() => Time.time > _lastAttackTime + options.attackCooldown;

        #endregion
    }
}