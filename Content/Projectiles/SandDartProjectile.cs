using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreDarts.Content.Projectiles {
	public class SandDartProjectile : ModProjectile {
		public override void SetStaticDefaults() {
			// If this arrow would have strong effects (like Holy Arrow pierce), we can make it fire fewer projectiles from Daedalus Stormbow for game balance considerations like this:
			//ProjectileID.Sets.FiresFewerFromDaedalusStormbow[Type] = true;			
			//ProjectileID.Sets.Explosive[Type] = true;
		}

		public override void SetDefaults() {
			Projectile.width = 10; // The width of projectile hitbox
			Projectile.height = 10; // The height of projectile hitbox

			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 120;
			Projectile.penetrate = -1;
			Projectile.ArmorPenetration = 20;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
		}

		public override void AI() {
			// The code below was adapted from the ProjAIStyleID.Arrow behavior. Rather than copy an existing aiStyle using Projectile.aiStyle and AIType,
			// like some examples do, this example has custom AI code that is better suited for modifying directly.
			// See https://github.com/tModLoader/tModLoader/wiki/Basic-Projectile#what-is-ai for more information on custom projectile AI.

			// Apply gravity after a quarter of a second
			if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3) {
				Projectile.PrepareBombToBlow();
			}
			Projectile.ai[0] += 1f;
			if (Projectile.ai[0] >= 15f) {
				Projectile.ai[0] = 15f;
				Projectile.velocity.Y += 0.1f;
			}
			if (Main.myPlayer == Projectile.owner) {

			}
			// The projectile is rotated to face the direction of travel
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

			// Cap downward velocity
			if (Projectile.velocity.Y > 16f) {
				Projectile.velocity.Y = 16f;
			}
		}
		public override bool OnTileCollide(Microsoft.Xna.Framework.Vector2 oldVelocity) {
			Projectile.timeLeft = 3;
			return false;
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			Projectile.timeLeft = 3;
		}

		public override void PrepareBombToBlow() {
			Projectile.tileCollide = false; // This is important or the explosion will be in the wrong place if the grenade explodes on slopes.
			Projectile.velocity = Vector2.Zero;
			Projectile.alpha = 255;
			Projectile.Resize(256, 256);
			Projectile.damage /= 2;
		} 
		public override void OnKill(int timeLeft) {
			SoundEngine.PlaySound(SoundID.Dig, Projectile.position); // Plays the basic sound most projectiles make when hitting blocks.
			for (int i = 0; i < 5; i++) // Creates a splash of dust around the position the projectile dies.
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Copper);
				dust.noGravity = true;
				dust.velocity *= 1.5f;
				dust.scale *= 0.9f;
			} 
			float x = Projectile.Center.X;

			float y = Projectile.Center.Y;
			for (int j = 0; j < 40; j++) {
				Vector2 pos = new Vector2(x + 60*MathF.Cos(MathF.Tau*j/40), y + 60*MathF.Sin(MathF.Tau*j/40));
				Vector2 vel = new Vector2(10*MathF.Cos(MathF.Tau*j/40), 10*MathF.Sin(MathF.Tau*j/40));
				var fireDust = Dust.NewDustPerfect(pos, DustID.Torch, vel, 25, Color.NavajoWhite, 1.2f);
				fireDust.noGravity = true;
				fireDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 1.0f);
				fireDust.velocity *= 3f;
			}
		}
	}
}