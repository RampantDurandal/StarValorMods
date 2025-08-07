# Durandal's Star Valor Mods
If you haven't already, you'll need to install BepInEx 5.4+, x86 version. See here for its installation guide: https://docs.bepinex.dev/articles/user_guide/installation/index.html

Once that's done, place the mod files in your \SteamLibrary\steamapps\common\Star Valor\BepInEx\plugins\ folder.

## Legendary Upgrade Kits
Allows you to craft upgrade kits to bring gear up to legendary (gold) quality. To do so, you'll need to scrap looted legendary gear to acquire a legendary component. Each kit takes 8 of these components to craft by default.

You can configure how many components scrapping a piece of gear gives (a random number from minimum to maximum), as well as how many are required to craft a kit. After running the game with the mod once, edit the durandal.LegendaryUpgradeKits.cfg file in your BepInEx/config folder.

Compatability notes:
- Works with Components From Scrapping; both mods will give components when scrapping a legendary.
- Works with Mod Any Item; however, if you change the config for my mod, you'll need to delete the generated .itemmod files or they will overwrite your config changes.

Big thanks to MartinC (https://github.com/MPC88) and LunaLycan (https://github.com/LunaLycan287/StarValorMods) whose code I modeled much of mine off of!
