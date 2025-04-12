using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MoreDarts.Content.NPCs {
	public class WDDarts : GlobalNPC {
        public override void ModifyShop(NPCShop shop) {
			if (shop.NpcType == NPCID.WitchDoctor) {
				shop.Add<Items.Ammo.SteelDart>();
            }
        }
	}
}