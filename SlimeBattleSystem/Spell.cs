using System;

namespace SlimeBattleSystem
{
    
    [Serializable]
    public class Spell
    {

        public int magicPointsCost = 1;

        public virtual void CastSpell(Participant target)
        {
            // target can be the caster, a friendly npc, or an enemy
        }

    }

}