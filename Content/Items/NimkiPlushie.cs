using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreDarts.Content.Items { 
	public class NimkiPlushie : ModItem {
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 99;
		}
		public override void SetDefaults() {
			Item.width = 66;
			Item.height = 72;
			Item.rare = ItemRarityID.Blue; 

			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 1.5f;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.value = Item.sellPrice(silver: 1);
			Item.shoot = ModContent.ProjectileType<Projectiles.NimkiProjectile>(); // The projectile that weapons fire when using this item as ammunition.
			Item.shootSpeed = 3.5f; // The speed of the projectile.
		}
	}
}
