using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreDarts.Content.Projectiles {
	public class BoneDartProjectile : ModProjectile {
		public override void SetDefaults() {
			Projectile.width = 10; // The width of projectile hitbox
			Projectile.height = 10; // The height of projectile hitbox

			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 1200;
		}

		public override void AI() {
			Projectile.ai[0] += 1f;
			Projectile.ai[1] += 1f;
			if (Projectile.ai[0] >= 15f) {
				Projectile.ai[0] = 15f;
				Projectile.velocity.Y += 0.05f;
			}
			// The projectile is rotated to face the direction of travel
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

			// Cap downward velocity
			if (Projectile.velocity.Y > 16f) {
				Projectile.velocity.Y = 16f;
			}
		}
		public override void OnKill(int timeLeft) {
			SoundEngine.PlaySound(SoundID.Dig, Projectile.position); // Plays the basic sound most projectiles make when hitting blocks.
			for (int i = 0; i < 5; i++) {
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Silver);
				dust.noGravity = true;
				dust.velocity *= 1.5f;
				dust.scale *= 0.9f;
			} 
			if (Main.myPlayer == Projectile.owner) {
				for (int i = 0; i < 3; i++){
					Vector2 test = Vector2.One.RotatedByRandom(MathF.Tau);
					float ang = MathF.Atan(test.X/test.Y);
					test.Normalize();
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X+20*MathF.Cos(ang), Projectile.position.Y, test.X*2, test.Y*2, 
						ModContent.ProjectileType<BoneShardProjectile>(), Projectile.damage/2, Projectile.knockBack/2, Main.myPlayer);
				}
			}
		}
	}
}