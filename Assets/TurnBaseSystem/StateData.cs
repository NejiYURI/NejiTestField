using UnityEngine;

    public abstract class StateData
    {
        protected MainGameManager gameManager;
        public StateData(MainGameManager _gameManager)
        {
            this.gameManager = _gameManager;
        }
        public virtual void TurnStartFunction()
        {

        }

        public virtual void TurnEndFunction()
        {

        }
    }

