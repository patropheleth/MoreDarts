using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreDarts.Content.Items.Ammo { 
	public class BoboGrenade : ModItem {
		// The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.MoreDarts.hjson' file.
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 99;
		}
		public override void SetDefaults() {
			Item.width = 14;
			Item.height = 36;
			Item.rare = ItemRarityID.Blue; 

			Item.damage = 70; 
			Item.DamageType = DamageClass.Ranged;

			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 1.5f;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.value = Item.sellPrice(silver: 1);
			Item.shoot = ModContent.ProjectileType<Projectiles.BoboGrenadeProjectile>(); // The projectile that weapons fire when using this item as ammunition.
			Item.shootSpeed = 3.5f; // The speed of the projectile.
		}
	}
}
