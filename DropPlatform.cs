using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;

namespace DropPlatform
{
	public class DropPlatform : Mod
	{
		[Label("Quick Drop Platform")]
		public class newConfig : ModConfig
		{
			public override ConfigScope Mode => ConfigScope.ServerSide;
			public static newConfig get => ModContent.GetInstance<newConfig>();

			[Header("Settings")]

			[Label("Quick Drop Liquid walk")]
			[Tooltip("Make water walk do the same thing\n its kinda pointless cuz no-one use water for arena :/")]
			[DefaultValue(false)]
			public bool ThereIsOnly1ConfigInThisMod;
		}
		public class myPlayer : ModPlayer
		{
			public List<Vector2> platform = new List<Vector2>();

			/*
			public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
				ModPacket packet = null;
				foreach (var item in platform)
				{
					packet = mod.GetPacket();
					packet.Write(item.X);
					packet.Write(item.Y);
					packet.Send(toWho, fromWho);
				}
			}
			*/

			public override void ResetEffects() {
				foreach (var pos in platform) {
					Tile tile = Framing.GetTileSafely((int)(pos.X / 16), (int)(pos.Y / 16));
					tile.inActive(false);
				}
				platform.Clear();
			}
			public override void PostUpdateMiscEffects() {
				if (player.controlDown) {
					for (int i = -2; i <= 2; i++)
					{
						Vector2 pos = player.Center;
						pos.X += i * 16;
						pos.Y += player.height / 2;
						if (player.mount.Active)
							pos.Y += player.mount.HeightBoost;
						pos.Y += 8;

						Tile tile = Framing.GetTileSafely((int)(pos.X / 16), (int)(pos.Y / 16));
						if (!tile.inActive() && (tile.type == TileID.Platforms || tile.type == TileID.PlanterBox)) {
							tile.inActive(true);
							platform.Add(pos);
						}
					}
					if (newConfig.get.ThereIsOnly1ConfigInThisMod) {
						player.waterWalk = false;
						player.waterWalk2 = false;
					}
				}
			}
		}	
	}
}