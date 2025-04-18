using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ExampleMod.Content {
    public class ExampleRecipes : ModSystem {
        public override void PostAddRecipes() {
			for (int i = 0; i < Recipe.numRecipes; i++) {
				Recipe recipe = Main.recipe[i];
                //remove old poison dart recipe
				if (recipe.HasResult(ItemID.PoisonDart)) {
					recipe.DisableRecipe();
				}
			}
		}
    }
}