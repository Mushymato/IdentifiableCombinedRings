global using SObject = StardewValley.Object;
using HarmonyLib;
using IdentifiableCombinedRings.AtraStuff;
using IdentifiableCombinedRings.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace IdentifiableCombinedRings;

/// <inheritdoc />
public class ModEntry : Mod
{
    public static IMonitor ModMonitor { get; internal set; } = null!;

    /// <summary>
    /// Gets the config instance for this mod.
    /// </summary>
    internal static ModConfig Config { get; private set; } = null!;

    /// <inheritdoc/>
    public override void Entry(IModHelper helper)
    {
        I18n.Init(this.Helper.Translation);
        ModMonitor = Monitor;

        Config = helper.ReadConfig<ModConfig>();
        this.ApplyPatches(new Harmony(this.ModManifest.UniqueID));

        AssetManager.Initialize(helper.GameContent);
        helper.Events.Content.AssetRequested += static (_, e) => AssetManager.OnAssetRequested(e);
        helper.Events.GameLoop.SaveLoaded += static (_, _) => AssetManager.Load();
    }

    /// <summary>
    /// Applies and logs this mod's harmony patches.
    /// </summary>
    /// <param name="harmony">My harmony instance.</param>
    private void ApplyPatches(Harmony harmony)
    {
        try
        {
            harmony.PatchAll(typeof(ModEntry).Assembly);
        }
        catch (Exception ex)
        {
            this.Monitor.Log(string.Format(ErrorMessageConsts.HARMONYCRASH, ex), LogLevel.Error);
        }
        // harmony.Snitch(this.Monitor, harmony.Id, transpilersOnly: true);
    }

    /// <summary>
    /// Generates the GMCM for this mod by looking at the structure of the config class.
    /// </summary>
    /// <param name="sender">Unknown, expected by SMAPI.</param>
    /// <param name="e">Arguments for event.</param>
    /// <remarks>To add a new setting, add the details to the i18n file. Currently handles: bool.</remarks>
    private void SetUpConfig(object? sender, GameLaunchedEventArgs e)
    {
        // GMCMHelper helper = new(this.Monitor, this.Helper.Translation, this.Helper.ModRegistry, this.ModManifest);
        var GMCM = Helper.ModRegistry.GetApi<Interfaces.IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
        if (GMCM != null)
        {
            GMCM.Register(
                ModManifest,
                reset: static () => Config = new ModConfig(),
                save: () => this.Helper.AsyncWriteConfig(this.Monitor, Config)
            );
            GMCM.AddParagraph(
                ModManifest,
                I18n.Mod_Description
            );
            GMCM.AddBoolOption(
                ModManifest,
                getValue: () => Config.OverrideCombinedRing,
                setValue: (value) => Config.OverrideCombinedRing = value,
                name: I18n.OverrideCombinedRing_Title,
                tooltip: I18n.OverrideCombinedRing_Description
            );
        }
    }
}
