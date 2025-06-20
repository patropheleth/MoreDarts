using MoreDarts.Content.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreDarts.Content.Pets {
	public class ChillyEgg : ModItem {
		// Names and descriptions of all ExamplePetX classes are defined using .hjson files in the Localization folder
		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.GolemPetItem);

			Item.shoot = ModContent.ProjectileType<HamisProjectile>(); // "Shoot" your pet projectile.
			Item.buffType = ModContent.BuffType<HamisBuff>(); // Apply buff upon usage of the Item.
		}

        public override bool? UseItem(Player player) {
			if (player.whoAmI == Main.myPlayer) {
				player.AddBuff(Item.buffType, 3600);
			}
   			return true;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			
		}
	}
}
