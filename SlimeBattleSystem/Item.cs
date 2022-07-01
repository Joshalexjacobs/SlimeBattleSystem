namespace SlimeBattleSystem
{

    public class Item
    {

        public string name;

        public Item()
        {
            
        }

        public Item(string name)
        {
            this.name = name;
        }

        public virtual void UseItem(Participant target)
        {
            // target can be the caster, a friendly npc, or an enemy
        }
        
    }

    public class DroppableItem
    {

        public Item itemToDrop;

        public int chanceToDrop = 1;

        public DroppableItem()
        {
        }

        public DroppableItem(Item itemToDrop, int chanceToDrop)
        {
            this.itemToDrop = itemToDrop;
            this.chanceToDrop = chanceToDrop;
        }
    }
    
}
