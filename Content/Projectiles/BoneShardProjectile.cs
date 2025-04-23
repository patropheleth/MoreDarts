using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreDarts.Content.Projectiles {
	public class BoneShardProjectile : ModProjectile {
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
			Projectile.velocity.Y += 0.3f;
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
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Asphalt);
				dust.noGravity = true;
				dust.velocity *= 1.5f;
				dust.scale *= 0.9f;
			}
		}
		/*public override bool OnTileCollide(Vector2 oldVelocity) {
			Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

			// If the projectile hits the left or right side of the tile, reverse the X velocity
			if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon) {
				Projectile.velocity.X = -oldVelocity.X;
			}

			
			// If the projectile hits the top or bottom side of the tile, reverse the Y velocity
			if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon) {
				return true;
			}
			return false;
		}*/

	}
}