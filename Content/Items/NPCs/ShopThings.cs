using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreDarts.Content.NPCs {
	public class WDDarts : GlobalNPC {
		
		public override void ModifyNPCLoot (NPC npc, NPCLoot npcLoot ) {
			if (npc.type == NPCID.BlackRecluse || npc.type == NPCID.WallCreeper ||
				npc.type == NPCID.BlackRecluseWall || npc.type == NPCID.WallCreeperWall) {
					npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Pets.ChillyEgg>(), 50));
				}
        }
		
		public override void ModifyShop(NPCShop shop)
		{
			if (shop.NpcType == NPCID.WitchDoctor)
			{
				shop.Add<Items.Ammo.SteelDart>();
			}
			if (shop.NpcType == NPCID.Demolitionist)
			{
				shop.Add<Items.Ammo.BoboGrenade>(Condition.PlayerCarriesItem(ModContent.ItemType<Items.Ammo.BoboGrenade>()));
			}
			if (shop.NpcType == NPCID.PartyGirl)
			{
				shop.Add<Items.NimkiPlushie>();
			}
		}
	}
}