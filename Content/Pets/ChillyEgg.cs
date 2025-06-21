using MoreDarts.Content.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreDarts.Content.Pets {
	public class ChillyEgg : ModItem {
		// Names and descriptions of all ExamplePetX classes are defined using .hjson files in the Localization folder
		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.GolemPetItem);
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(silver: 56);
			Item.shoot = ModContent.ProjectileType<HamisProjectile>(); // "Shoot" your pet projectile.
			Item.buffType = ModContent.BuffType<HamisBuff>(); // Apply buff upon usage of the Item.
		}

        public override bool? UseItem(Player player) {
			if (player.whoAmI == Main.myPlayer) {
				player.AddBuff(Item.buffType, 3600);
			}
   			return true;
		}

		public override void AddRecipes() {
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SpiderEgg, 1);
			recipe.AddIngredient(ItemID.IceBlock, 5);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
			recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.FriedEgg, 1);
			recipe.AddIngredient(ItemID.IceBlock, 5);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
			recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.RottenEgg, 1);
			recipe.AddIngredient(ItemID.IceBlock, 5);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
			recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.BlueEgg, 1);
			recipe.AddIngredient(ItemID.IceBlock, 5);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
			recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.LizardEgg, 1);
			recipe.AddIngredient(ItemID.IceBlock, 5);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
