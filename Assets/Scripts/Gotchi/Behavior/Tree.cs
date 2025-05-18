using System;
using UnityEngine;

namespace GimGim.Gotchi {
    public abstract class Tree : MonoBehaviour {
        private Node _root = null;

        private void Start() {
            _root = SetupTree();
        }

        private void Update() {
            _root?.Evaluate();
        }

        protected abstract Node SetupTree();
    }
}