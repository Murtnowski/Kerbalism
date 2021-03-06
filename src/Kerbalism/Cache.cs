using System;
using System.Collections.Generic;
using UnityEngine;


namespace KERBALISM
{
	public static class Cache
	{
		public static void Init()
		{
			vesselObjects = new Dictionary<Guid, Dictionary<string, object>>();
			warp_caches = new Dictionary<Guid, Drive>();
		}


		public static void Clear()
		{
			warp_caches.Clear();
			vesselObjects.Clear();
		}

		/// <summary>
		/// Called whenever a vessel changes and/or should be updated for various reasons.
		/// Purge the anonymous cache and science transmission cache
		/// </summary>
		public static void PurgeVesselCaches(Vessel v)
		{
			var id = Lib.VesselID(v);
			vesselObjects.Remove(id);
			warp_caches.Remove(id);
		}

		/// <summary>
		/// Called whenever a vessel changes and/or should be updated for various reasons.
		/// Purge the anonymous cache and science transmission cache
		/// </summary>
		public static void PurgeVesselCaches(ProtoVessel v)
		{
			var id = Lib.VesselID(v);
			vesselObjects.Remove(id);
			warp_caches.Remove(id);
		}

		/// <summary>
		/// Called when the game state has changed (savegame loads), must reset all non-persisted data that won't be loaded from DB
		/// Purge all anonymous caches, science transmission caches, experiments cached data, and log messages
		/// </summary>
		public static void PurgeAllCaches()
		{
			vesselObjects.Clear();
			warp_caches.Clear();
			Message.all_logs.Clear();
		}

		public static Drive WarpCache(Vessel v)
		{
			Guid id = Lib.VesselID(v);

			// get from the cache, if it exist
			Drive drive;
			if (warp_caches.TryGetValue(id, out drive))
				return drive;
			
			drive = new Drive("warp cache drive", 0, 0);
			warp_caches.Add(id, drive);
			return drive;
		}

		internal static T VesselObjectsCache<T>(Vessel vessel, string key)
		{
			return VesselObjectsCache<T>(Lib.VesselID(vessel), key);
		}

		internal static T VesselObjectsCache<T>(ProtoVessel vessel, string key)
		{
			return VesselObjectsCache<T>(Lib.VesselID(vessel), key);
		}

		private static T VesselObjectsCache<T>(Guid id, string key)
		{
			if (!vesselObjects.ContainsKey(id))
				return default(T);

			var dict = vesselObjects[id];
			if(dict == null)
				return default(T);

			if (!dict.ContainsKey(key))
				return default(T);

			return (T)dict[key];
		}

		internal static void SetVesselObjectsCache<T>(Vessel vessel, string key, T value)
		{
			SetVesselObjectsCache(Lib.VesselID(vessel), key, value);
		}

		internal static void SetVesselObjectsCache<T>(ProtoVessel pv, string key, T value)
		{
			SetVesselObjectsCache(Lib.VesselID(pv), key, value);
		}

		private static void SetVesselObjectsCache<T>(Guid id, string key, T value)
		{
			if (!vesselObjects.ContainsKey(id))
				vesselObjects.Add(id, new Dictionary<string, object>());

			var dict = vesselObjects[id];
			dict.Remove(key);
			dict.Add(key, value);
		}

		internal static bool HasVesselObjectsCache(Vessel vessel, string key)
		{
			return HasVesselObjectsCache(Lib.VesselID(vessel), key);
		}

		internal static bool HasVesselObjectsCache(ProtoVessel pv, string key)
		{
			return HasVesselObjectsCache(Lib.VesselID(pv), key);
		}

		private static bool HasVesselObjectsCache(Guid id, string key)
		{
			if (!vesselObjects.ContainsKey(id))
				return false;

			var dict = vesselObjects[id];
			return dict.ContainsKey(key);
		}

		internal static void RemoveVesselObjectsCache(Vessel vessel, string key)
		{
			RemoveVesselObjectsCache(Lib.VesselID(vessel), key);
		}

		internal static void RemoveVesselObjectsCache<T>(ProtoVessel pv, string key)
		{
			RemoveVesselObjectsCache(Lib.VesselID(pv), key);
		}

		private static void RemoveVesselObjectsCache(Guid id, string key)
		{
			if (!vesselObjects.ContainsKey(id))
				return;
			var dict = vesselObjects[id];
			dict.Remove(key);
		}

		// caches
		private static Dictionary<Guid, Drive> warp_caches;
		private static Dictionary<Guid, Dictionary<string, System.Object>> vesselObjects;
	}


} // KERBALISM
