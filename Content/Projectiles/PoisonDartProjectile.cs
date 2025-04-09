using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreDarts.Content.Projectiles {
	public class PoisonDartProjectile : ModProjectile {
		private NPC HomingTarget {
			get => Projectile.ai[2] == 0 ? null : Main.npc[(int)Projectile.ai[2] - 1];
			set {
				Projectile.ai[2] = value == null ? 0 : value.whoAmI + 1;
			}
		}

		public override void SetDefaults() {
			Projectile.width = 10; // The width of projectile hitbox
			Projectile.height = 10; // The height of projectile hitbox

			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 1200;
			Projectile.penetrate = 2;
			Projectile.maxPenetrate = 2;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
		}

		public override void AI() {
			// The code below was adapted from the ProjAIStyleID.Arrow behavior. Rather than copy an existing aiStyle using Projectile.aiStyle and AIType,
			// like some examples do, this example has custom AI code that is better suited for modifying directly.
			// See https://github.com/tModLoader/tModLoader/wiki/Basic-Projectile#what-is-ai for more information on custom projectile AI.

			float maxDetectRadius = 400f; // The maximum radius at which a projectile can detect a target
			// Apply gravity after a quarter of a second
			Projectile.ai[0] += 1f;
			if (Projectile.ai[0] >= 15f) {
				Projectile.ai[0] = 15f;
				Projectile.velocity.Y += 0.1f;
			}
			// The projectile is rotated to face the direction of travel
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

			// Cap downward velocity
			if (Projectile.velocity.Y > 16f) {
				Projectile.velocity.Y = 16f;
			}
			// First, we find a homing target if we don't have one
			if (HomingTarget == null) {
				HomingTarget = FindClosestNPC(maxDetectRadius);
			}

			// If we have a homing target, make sure it is still valid. If the NPC dies or moves away, we'll want to find a new target
			if (HomingTarget != null && !IsValidTarget(HomingTarget)) {
				HomingTarget = null;
			}

			// If we don't have a target, don't adjust trajectory
			if (HomingTarget == null)
				return;

			// If found, we rotate the projectile velocity in the direction of the target.
			// We only rotate by 3 degrees an update to give it a smooth trajectory. Increase the rotation speed here to make tighter turns
			float length = Projectile.velocity.Length();
			float targetAngle = Projectile.AngleTo(HomingTarget.Center);
			Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(targetAngle, MathHelper.ToRadians(0.25f)).ToRotationVector2() * length;
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
		}
		public NPC FindClosestNPC(float maxDetectDistance) {
			NPC closestNPC = null;

			// Using squared values in distance checks will let us skip square root calculations, drastically improving this method's speed.
			float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

			// Loop through all NPCs
			foreach (var target in Main.ActiveNPCs) {
				// Check if NPC able to be targeted. 
				if (IsValidTarget(target)) {
					// The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
					float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

					// Check if it is within the radius
					if (sqrDistanceToTarget < sqrMaxDetectDistance) {
						sqrMaxDetectDistance = sqrDistanceToTarget;
						closestNPC = target;
					}
				}
			}

			return closestNPC;
		}

		public bool IsValidTarget(NPC target) {
			// This method checks that the NPC is:
			// 1. active (alive)
			// 2. chaseable (e.g. not a cultist archer)
			// 3. max life bigger than 5 (e.g. not a critter)
			// 4. can take damage (e.g. moonlord core after all it's parts are downed)
			// 5. hostile (!friendly)
			// 6. not immortal (e.g. not a target dummy)
			// 7. doesn't have solid tiles blocking a line of sight between the projectile and NPC
			return target.CanBeChasedBy() && Collision.CanHit(Projectile.Center, 1, 1, target.position, target.width, target.height);
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			target.AddBuff(BuffID.Poisoned, 240);
        }
		public override void OnKill(int timeLeft) {
			SoundEngine.PlaySound(SoundID.Dig, Projectile.position); // Plays the basic sound most projectiles make when hitting blocks.
			for (int i = 0; i < 5; i++) // Creates a splash of dust around the position the projectile dies.
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Silver);
				dust.noGravity = true;
				dust.velocity *= 1.5f;
				dust.scale *= 0.9f;
			} 
		}
	}
}