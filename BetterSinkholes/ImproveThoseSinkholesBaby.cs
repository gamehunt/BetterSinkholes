using Mirror;
using UnityEngine;
using HarmonyLib;
using RemoteAdmin;
using Exiled.API.Features;

namespace BetterSinkholes
{
    [HarmonyPatch(typeof(SinkholeEnvironmentalHazard), nameof(SinkholeEnvironmentalHazard.DistanceChanged))]
    public class ImproveThoseSinkholesBaby
    {
        public static bool Prefix(SinkholeEnvironmentalHazard __instance, ReferenceHub player)
        {

            // Check if player has a connection to the server.
            if (!NetworkServer.active) return false;

            PlayerEffectsController playerEffectsController = player.playerEffectsController;
            if (playerEffectsController == null) return false;

            // Check if the player walking into a sinkhole is an SCP or not.
            if (__instance.SCPImmune)
            {
                CharacterClassManager component = player.GetComponent<CharacterClassManager>();
                if (component == null || component.IsAnyScp()) return false;
            }

            // Check if player is in god mode.
            Player ply = Player.Get(player);
            if (ply.IsGodModeEnabled) return false;

            // If a player is out of a sinkhole's range.
            if (Vector3.Distance(player.transform.position, __instance.transform.position) > (double)__instance.DistanceToBeAffected * BetterSinkholes.config.SlowDistance)
            {
                // If player doesn't have a sinkhole effect don't remove it.
                if (player.TryGetComponent(out PlayerEffectsController pec))
                {
                    CustomPlayerEffects.SinkHole SinkholeEffect = pec.GetEffect<CustomPlayerEffects.SinkHole>();

                    // // If the player has the sinkhole effect, remove it.
                    if (SinkholeEffect != null && SinkholeEffect.IsEnabled)
                        playerEffectsController.DisableEffect<CustomPlayerEffects.SinkHole>();

                    return false;
                }

                return false;
            }

            // Check distance from the sinkhole's center.
            if (Vector3.Distance(player.transform.position, __instance.transform.position) < (double)__instance.DistanceToBeAffected * BetterSinkholes.config.TeleportDistance)
            {
                // Remove Sinkhole effect once falling into a sinkhole.
                playerEffectsController.DisableEffect<CustomPlayerEffects.SinkHole>();

                // Teleport player once walking too close to the center of a sinkhole.
                player.playerMovementSync.OverridePosition(Vector3.down * 1998.5f, 0f, true);

                // Apply corrosion effect.
                playerEffectsController.EnableEffect<CustomPlayerEffects.Corroding>(0f, false);

                // Send player a broadcast specified in the configs. Default: "" for 0U duration.
                QueryProcessor.Localplayer.GetComponent<Broadcast>().TargetAddElement(player.gameObject.GetComponent<NetworkIdentity>().connectionToClient, BetterSinkholes.config.TeleportMessage, BetterSinkholes.config.TeleportMessageDuration, Broadcast.BroadcastFlags.Normal);
                return false;
            }

            playerEffectsController.EnableEffect<CustomPlayerEffects.SinkHole>(0f, false);
            return false;
        }
    }
}
