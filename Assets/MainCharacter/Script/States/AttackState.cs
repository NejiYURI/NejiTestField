using UnityEngine;
    public class AttackState : CharacterState
    {
        private IF_GameCharacter AttackTarget;
        public AttackState(MainCharacterScript characterScript, IF_GameCharacter if_character) : base(characterScript)
        {
            this.AttackTarget = if_character;
        }

        public override void StartFunction()
        {
            if (this.AttackTarget != null) this.AttackTarget.GetDamage(3f);
            characterScript.SetState(new SelectState(characterScript));
        }
    }


