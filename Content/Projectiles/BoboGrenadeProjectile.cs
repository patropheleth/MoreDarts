using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreDarts.Content.Projectiles {
	public class BoboGrenadeProjectile : ModProjectile {
		
		public override void SetStaticDefaults() {
			ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Type] = true; // Damage dealt to players does not scale with difficulty in vanilla.

			// This set handles some things for us already:
			// Sets the timeLeft to 3 and the projectile direction when colliding with an NPC or player in PVP (so the explosive can detonate).
			// Explosives also bounce off the top of Shimmer, detonate with no blast damage when touching the bottom or sides of Shimmer, and damage other players in For the Worthy worlds.
			ProjectileID.Sets.Explosive[Type] = true;
		}
		public override void SetDefaults() {
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.penetrate = -1; // Infinite penetration so that the blast can hit all enemies within its radius.
			Projectile.DamageType = DamageClass.Ranged;
			// usesLocalNPCImmunity and localNPCHitCooldown of -1 mean the projectile can only hit the same target once.
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;

			Projectile.timeLeft = 180;
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
			if (Projectile.ai[0] == 0.0f){
				Projectile.velocity.Normalize();
				Projectile.velocity *= 10;
			}
			Projectile.ai[0] += 1f;
			// Wait 15 ticks until applying friction and gravity.
			if (Projectile.ai[0] > 15f) {
				// Slow down if on the ground.
				if (Projectile.velocity.Y == 0f) {
					Projectile.velocity.X *= 0.95f;
				}

				// Fall down. Remember, positive Y is down.
				Projectile.velocity.Y += 0.2f;
				if (Projectile.owner == Main.myPlayer){
					Vector2 vel = Main.LocalPlayer.position-Projectile.position;
					vel.Normalize();
					Projectile.velocity += vel * 0.2f;
				}
			}
			
			// Rotate the grenade in the direction it is moving.
			Projectile.rotation += Projectile.velocity.X * 0.1f;
		}

		public override bool OnTileCollide(Vector2 oldVelocity) {
			if (Projectile.velocity.X != oldVelocity.X) {
				Projectile.velocity.X = oldVelocity.X * -0.4f;
			}

			if (Projectile.velocity.Y != oldVelocity.Y && oldVelocity.Y > 0.7f) {
				Projectile.velocity.Y = oldVelocity.Y * -0.4f;
			}
			return false;
		}

		public override void PrepareBombToBlow() {
			Projectile.tileCollide = false; // This is important or the explosion will be in the wrong place if the grenade explodes on slopes.
			Projectile.alpha = 255; // Make the grenade invisible.

			Projectile.Resize(128, 128);
			Projectile.knockBack = 8f;
		}

		public override void OnKill(int timeLeft) {
			SoundEngine.PlaySound(SoundID.Item62, Projectile.position);
			Projectile.Resize(22, 22);

			for (int i = 0; i < 30; i++) {
				var smoke = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1.5f);
				smoke.velocity *= 1.4f;
			}

			// Spawn a bunch of fire dusts.
			for (int j = 0; j < 20; j++) {
				var fireDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 3.5f);
				fireDust.noGravity = true;
				fireDust.velocity *= 7f;
				fireDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 1.5f);
				fireDust.velocity *= 3f;
			}

			// Spawn a bunch of smoke gores.
			for (int k = 0; k < 2; k++) {
				float speedMulti = 0.4f;
				if (k == 1) {
					speedMulti = 0.8f;
				}

				var smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity += Vector2.One;
				smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity.X -= 1f;
				smokeGore.velocity.Y += 1f;
				smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity.X += 1f;
				smokeGore.velocity.Y -= 1f;
				smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
				smokeGore.velocity *= speedMulti;
				smokeGore.velocity -= Vector2.One;
			}
		}
	}
}