using MoreDarts.Content.Items.Ammo;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;

namespace MoreDarts{
	// ModPlayer classes provide a way to attach data to Players and act on that data. ExamplePlayer has a lot of functionality related to 
	// several effects and items in MoreDarts{See SimpleModPlayer for a very simple example of how ModPlayer classes work.
	public class BoboGrenadinization : ModPlayer {
        bool isTransform = false;

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource) {
            if (damageSource.SourcePlayerIndex == Main.myPlayer &&
            damageSource.SourceProjectileType == ProjectileID.Grenade){
                isTransform = true;
            }
            return true;
        }
        public override void OnRespawn() {
            if (!isTransform){
                return;
            }
            Player you = Main.LocalPlayer; 
            for (int i = 0; i < 50; i++){
                if (you.inventory[i].type == ItemID.Grenade){
                    int temp = you.inventory[i].stack;
                    you.inventory[i].TurnToAir();
                    Player.QuickSpawnItem(you.GetSource_FromThis(), ModContent.ItemType<BoboGrenade>(), temp);
                }
            }
            isTransform = false;
        }
    }
}