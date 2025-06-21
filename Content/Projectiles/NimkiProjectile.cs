using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreDarts.Content.Projectiles {
	public class NimkiProjectile : ModProjectile {
		
		public override void SetStaticDefaults() {
			ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Type] = true; // Damage dealt to players does not scale with difficulty in vanilla.

			// This set handles some things for us already:
			// Sets the timeLeft to 3 and the projectile direction when colliding with an NPC or player in PVP (so the explosive can detonate).
			// Explosives also bounce off the top of Shimmer, detonate with no blast damage when touching the bottom or sides of Shimmer, and damage other players in For the Worthy worlds.
			ProjectileID.Sets.Explosive[Type] = true;
		}
		public override void SetDefaults() {
			Projectile.width = 60;
			Projectile.height = 60;
			Projectile.friendly = true;
			Projectile.penetrate = -1; // Infinite penetration so that the blast can hit all enemies within its radius.
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;

			Projectile.timeLeft = 18000;
		}
		public override bool PreAI() {
			DrawOriginOffsetY = 5;
			DrawOffsetX = -3;
			return true;
		}
		public override void AI() {
			// If timeLeft is <= 3, then explode the grenade.
			if (Projectile.owner == Main.myPlayer && Projectile.timeLeft == 17950) {
				Explode();
			}
			if (Projectile.ai[0] == 0.0f){
				Projectile.velocity.Normalize();
				Projectile.velocity *= 10;
			}
			Projectile.ai[0] += 1f;
			// Wait 15 ticks until applying friction and gravity.
			if (Projectile.ai[0] > 10f) {
				// Slow down if on the ground.
				if (Projectile.velocity.Y == 0f) {
					Projectile.velocity.X *= 0.95f;
				}
				// Fall down. Remember, positive Y is down.
				if (Projectile.ai[1] == 0.0f){
					Projectile.velocity.Y += 0.2f;
				}
			}
			
			// Rotate the grenade in the direction it is moving.
			Projectile.rotation += MathF.Sign(Projectile.velocity.X) * MathF.Sqrt(Math.Abs(Projectile.velocity.X * 0.2f));
		}

		public override bool OnTileCollide(Vector2 oldVelocity) {
			Projectile.velocity.X = 0.0f;
			Projectile.velocity.Y = 0.0f;
			Projectile.ai[1] = 1.0f;
			return false;
		}

		public void Explode() {
			SoundEngine.PlaySound(SoundID.LucyTheAxeTalk, Projectile.position);
			float x = Projectile.Center.X;
			float y = Projectile.Center.Y;
			for (int j = 0; j < 40; j++) {
				Vector2 pos = new(x + 20*MathF.Cos(MathF.Tau*j/40), y + 20*MathF.Sin(MathF.Tau*j/40));
				Vector2 vel = new(15*MathF.Cos(MathF.Tau*j/40), 15*MathF.Sin(MathF.Tau*j/40));
				Color greenb = new(0.0f, 1.0f, 0.75f);
				var fireDust = Dust.NewDustPerfect(pos, DustID.BubbleBlock, vel.RotatedBy(MathF.Tau/80), 25, greenb, 2.0f);
				var fireDust2 = Dust.NewDustPerfect(pos, DustID.BubbleBlock, vel, 25, new Color(0.0f, 1.0f, 1.0f), 2.0f);
				fireDust.noGravity = true;
				fireDust2.noGravity = true;
				fireDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BubbleBlock, 0f, 0f, 100, greenb, 2.0f);
				fireDust.velocity *= 3f;
			}
		}
	}
}