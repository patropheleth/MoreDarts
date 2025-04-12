using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreDarts.Content.Items.Ammo { 
	public class SteelDart : ModItem {
		// The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.MoreDarts.hjson' file.
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 99;
		}
		public override void SetDefaults() {
			Item.width = 14;
			Item.height = 36;

			Item.damage = 6; 
			Item.DamageType = DamageClass.Ranged;

			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.knockBack = 1.5f;
			Item.value = Item.sellPrice(copper: 16);
			Item.shoot = ModContent.ProjectileType<Projectiles.EctoDartProjectile>(); // The projectile that weapons fire when using this item as ammunition.
			Item.shootSpeed = 3f; // The speed of the projectile.
			Item.ammo = AmmoID.Dart; 
		}

		public override void AddRecipes()		{
			Recipe recipe = CreateRecipe(50);
			recipe.AddIngredient(ItemID.LeadBar, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
			Recipe recipe2 = CreateRecipe(50);
			recipe2.AddIngredient(ItemID.IronBar, 1);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}
