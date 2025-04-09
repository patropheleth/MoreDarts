using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreDarts.Content.Projectiles {
	public class EctoDartProjectile : ModProjectile {
		public override void SetDefaults() {
			Projectile.width = 10; // The width of projectile hitbox
			Projectile.height = 10; // The height of projectile hitbox

			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 1200;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 1;
		}

		public override void AI() {
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity.RotatedByRandom(MathF.Tau)/10, 
				ProjectileID.SpectreWrath, Projectile.damage/2, 0, Main.myPlayer);
        }

		public override void OnKill(int timeLeft) {
			SoundEngine.PlaySound(SoundID.Dig, Projectile.position); // Plays the basic sound most projectiles make when hitting blocks.
			for (int i = 0; i < 5; i++) {
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Silver);
				dust.noGravity = true;
				dust.velocity *= 1.5f;
				dust.scale *= 0.9f;
			} 
		}
	}
}