using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreDarts.Content.Items.Ammo { 
	public class VenomDart : ModItem {
		// The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.MoreDarts.hjson' file.
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 99;
		}
		public override void SetDefaults() {
			Item.width = 14;
			Item.height = 36;

			Item.damage = 16; // Keep in mind that the arrow's final damage is combined with the bow weapon damage.
			Item.DamageType = DamageClass.Ranged;

			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.knockBack = 1.5f;
			Item.value = Item.sellPrice(copper: 16);
			Item.shoot = ModContent.ProjectileType<Projectiles.VenomDartProjectile>(); // The projectile that weapons fire when using this item as ammunition.
			Item.shootSpeed = 3f; // The speed of the projectile.
			Item.ammo = AmmoID.Dart; // The ammo class this ammo belongs to.
		}

		public override void AddRecipes() {
			Recipe recipe = CreateRecipe(50);
			recipe.AddIngredient(ModContent.ItemType<SteelDart>(), 50);
			recipe.AddIngredient(ItemID.VialofVenom, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
