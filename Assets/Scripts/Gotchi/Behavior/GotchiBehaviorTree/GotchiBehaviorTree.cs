using System;
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
        private bool _playerDied = false;
        
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
                new IdleNode()
            });

            return root;
        }

        private static Node SetupAttackBranch() {
            Node root = new Selector();

            return root;
        }

        private static Node SetupPeaceBranch() {
            Node root = new Selector();

            return root;
        }

        private void OnPlayerDeath(PlayerDiedEvent e) {
            _playerDied = true;
        }
    }
}