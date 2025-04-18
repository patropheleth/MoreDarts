using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreDarts.Content.Projectiles {
	public class HellDartExplosionProjectile : ModProjectile {
		
		public override void SetStaticDefaults() {
			
			// This set handles some things for us already:
			// Sets the timeLeft to 3 and the projectile direction when colliding with an NPC or player in PVP (so the explosive can detonate).
			// Explosives also bounce off the top of Shimmer, detonate with no blast damage when touching the bottom or sides of Shimmer, and damage other players in For the Worthy worlds.
			//ProjectileID.Sets.Explosive[Type] = true;
		}
		public override void SetDefaults() {
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.friendly = true;
			Projectile.penetrate = -1; // Infinite penetration so that the blast can hit all enemies within its radius.
			Projectile.DamageType = DamageClass.Ranged;
			// usesLocalNPCImmunity and localNPCHitCooldown of -1 mean the projectile can only hit the same target once.
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;

			Projectile.timeLeft = 3;
		}
		public override void AI() {
			// If timeLeft is <= 3, then explode the grenade.
			if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3) {
				Projectile.PrepareBombToBlow();
			}
			else {
				// Spawn a smoke dust.
				var smokeDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100);
				smokeDust.scale *= 1f + Main.rand.Next(10) * 0.1f;
				smokeDust.velocity *= 0.2f;
				smokeDust.noGravity = true;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity) {
			// Bounce off of tiles.
			if (Projectile.velocity.X != oldVelocity.X) {
				Projectile.velocity.X = oldVelocity.X * -0.4f;
			}

			if (Projectile.velocity.Y != oldVelocity.Y && oldVelocity.Y > 0.7f) {
				Projectile.velocity.Y = oldVelocity.Y * -0.4f;
			}

			// Return false so the projectile doesn't get killed. If you do want your projectile to explode on contact with tiles, do not return true here.
			// If you return true, the projectile will die without being resized (no blast radius).
			// Instead, set `Projectile.timeLeft = 3;` like the Example Rocket Projectile.
			return false;
		}

		public override void PrepareBombToBlow() {
			Projectile.tileCollide = false; // This is important or the explosion will be in the wrong place if the grenade explodes on slopes.
			Projectile.alpha = 255; // Make the grenade invisible.

            Projectile.hostile = false;
			// Resize the hitbox of the projectile for the blast "radius".
			// Rocket I: 128, Rocket III: 200, Mini Nuke Rocket: 250
			// Measurements are in pixels, so 128 / 16 = 8 tiles.
			Projectile.Resize(80, 80);
			// Set the knockback of the blast.
			// Rocket I: 8f, Rocket III: 10f, Mini Nuke Rocket: 12f
			Projectile.knockBack = 8f;
		}

		public override void OnKill(int timeLeft) {
			// Play an exploding sound.
			//SoundEngine.PlaySound(SoundID.Item74, Projectile.position);

			// Resize the projectile again so the explosion dust and gore spawn from the middle.
			// Rocket I: 22, Rocket III: 80, Mini Nuke Rocket: 50

			// Spawn a bunch of smoke dusts.
			for (int i = 0; i < 5; i++) {
				var smoke = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1.5f);
				smoke.velocity *= 1.4f;
			}

			// Spawn a bunch of fire dusts.
			for (int j = 0; j < 10; j++) {
				var fireDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 3.5f);
				fireDust.noGravity = true;
				fireDust.velocity *= 7f;
				fireDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 1.5f);
				fireDust.velocity *= 3f;
			}
            
			Projectile.Resize(0, 0);
			// To make the explosion destroy tiles, take a look at the commented out code in Example Rocket Projectile.
		}
	}
}