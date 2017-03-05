using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Steamworks;

public class TrackTimePlayed : MonoBehaviour {

    public static bool lookAtCredits { private get; set; } // done
    public static bool sorry { private get; set; } // done
    public static bool everyCar { private get; set; }
    public static bool jansportShrineDiscovered { private get; set; } // done
    public static bool didntRefund { private get; set; } // done
    public static bool devTagUsed { private get; set; } // done
    public static bool largeObjectInAir { private get; set; } // done
    public static bool airBoost { private get; set; } // done
    public static bool allPowerupsAtOnce { private get; set; }
    public static bool plexusParkFallTenTimes { private get; set; }


  private enum Achievement : int {
    lookAtCredits,
    sorry,
    everyCar,
    jansportShrineDiscovered,
    didntRefund,
    devTagUsed,
    largeObjectInAir,
    airBoost,
    allPowerupsAtOnce,
    plexusParkFallTenTimes,
  };

  private Achievement_t[] m_Achievements = new Achievement_t[] {
    new Achievement_t(Achievement.lookAtCredits, "Thanks!", "Appreciating the creators"),
    new Achievement_t(Achievement.sorry, "Sorry", ":^/"),
    new Achievement_t(Achievement.everyCar, "Car Connoisseur", "Sampled the whole selection"),
    new Achievement_t(Achievement.jansportShrineDiscovered, "Find The Shrine", "You found it!"),
    new Achievement_t(Achievement.didntRefund, "Thanks for playing!", "You didn't refund our game!"),
    new Achievement_t(Achievement.devTagUsed, "Homage", "You're in the know, right?"),
    new Achievement_t(Achievement.largeObjectInAir, "Pull!", "Shot out of the air"),
    new Achievement_t(Achievement.airBoost, "Flying High", "This isn't Rocket League"),
    new Achievement_t(Achievement.allPowerupsAtOnce, "Clusterheck", "Sorry to whoever's on the receiving end of this one"),
    new Achievement_t(Achievement.plexusParkFallTenTimes, "Off The Edge", "Don't worry, you'll get back up"),
  };

  private CGameID m_GameID;

  // Did we get the stats from Steam?
  private bool m_bRequestedStats;
  private bool m_bStatsValid;

  // Should we store stats this frame?
  private bool m_bStoreStats;

  // Current Stat details
  private float m_flGameFeetTraveled;
  private float m_ulTickCountGameStart;
  private double m_flGameDurationSeconds;

  // Persisted Stat details

  private bool stat_lookAtCredits;
  private bool stat_sorry;
  private bool stat_everyCar;
  private bool stat_jansportShrineDiscovered;
  private float stat_didntRefund;
  private bool stat_devTagUsed;
  private bool stat_largeObjectInAir;
  private bool stat_airBoost;
  private bool stat_allPowerupsAtOnce;
  private bool stat_plexusParkFallTenTimes;


  protected Callback<UserStatsReceived_t> m_UserStatsReceived;
  protected Callback<UserStatsStored_t> m_UserStatsStored;
  protected Callback<UserAchievementStored_t> m_UserAchievementStored;

  private void Start() {

    m_GameID = new CGameID(SteamUtils.GetAppID());

    if (!PlayerPrefs.HasKey("CKYiscwBXR6VhzWh")) {
      PlayerPrefs.SetFloat("CKYiscwBXR6VhzWh", DataManager.TotalTimePlayed);
      Debug.Log("Created playerprefs for total time played");
    } else {
      Debug.Log("total time played on start: " + PlayerPrefs.GetFloat("CKYiscwBXR6VhzWh"));
    }

    if(SteamManager.Initialized) {
      string name = SteamFriends.GetPersonaName();
      Debug.Log(name);

    }
  }

  private void OnEnable() {
    m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
    m_UserStatsStored = Callback<UserStatsStored_t>.Create(OnUserStatsStored);
    m_UserAchievementStored = Callback<UserAchievementStored_t>.Create(OnAchievementStored);
  }

  private void Update() {
    if (PlayerPrefs.HasKey("CKYiscwBXR6VhzWh")) {
      DataManager.TotalTimePlayed += Time.unscaledDeltaTime;

      if ( DataManager.TotalTimePlayed > 7230f ) {
        DataManager.didntRefund = true;
      }
    }

    if (!m_bRequestedStats) {
      // Is Steam Loaded? if no, can't get stats, done
      if (!SteamManager.Initialized) {
        m_bRequestedStats = true;
        return;
      }
      
      // If yes, request our stats
      bool bSuccess = SteamUserStats.RequestCurrentStats();

      // This function should only return false if we weren't logged in, and we already checked that.
      // But handle it being false again anyway, just ask again later.
      m_bRequestedStats = bSuccess;
    }

    if (!m_bStatsValid) {
      return;
    }


    // check achievements here!!

    for (int i = 0; i < m_Achievements.Length; i++) {


      if (m_Achievements[i].m_bAchieved)
        continue;

      switch (m_Achievements[i].m_eAchievementID) {
        case Achievement.lookAtCredits:
          if (DataManager.lookAtCredits == true) {
            Debug.Log("looked at credits");
            UnlockAchievement(m_Achievements[i]);
          }
          break;
        case Achievement.sorry:
          if (DataManager.sorry == true) {
            UnlockAchievement(m_Achievements[i]);
          }
          break;
        case Achievement.everyCar:
          if (DataManager.everyCar == true) {
            UnlockAchievement(m_Achievements[i]);
          }
          break;
        case Achievement.jansportShrineDiscovered:
          if (DataManager.jansportShrineDiscovered == true) {
            UnlockAchievement(m_Achievements[i]);
          }
          break;
        case Achievement.didntRefund:
          if (DataManager.didntRefund == true) {
            UnlockAchievement(m_Achievements[i]);
          }
          break;
        case Achievement.devTagUsed:
          if (DataManager.devTagUsed == true) {
            UnlockAchievement(m_Achievements[i]);
          }
          break;
        case Achievement.largeObjectInAir:
          if (DataManager.largeObjectInAir == true) {
            UnlockAchievement(m_Achievements[i]);
          }
          break;
        case Achievement.airBoost:
          if (DataManager.airBoost == true) {
            UnlockAchievement(m_Achievements[i]);
          }
          break;
        case Achievement.allPowerupsAtOnce:
          if (DataManager.allPowerupsAtOnce == true) {
            UnlockAchievement(m_Achievements[i]);
          }
          break;
        case Achievement.plexusParkFallTenTimes:
          if (DataManager.plexusParkFallTenTimes == true) {
            UnlockAchievement(m_Achievements[i]);
          }
          break;
      }
    }

    if (m_bStoreStats) {
      // already set any achievements in UnlockAchievement

      // set stats
      SteamUserStats.SetStat("stat_lookAtCredits", BoolToInt(stat_lookAtCredits));
      SteamUserStats.SetStat("stat_sorry", BoolToInt(stat_sorry));
      SteamUserStats.SetStat("stat_everyCar", BoolToInt(stat_everyCar));
      SteamUserStats.SetStat("stat_jansportShrineDiscovered", BoolToInt(stat_jansportShrineDiscovered));
      SteamUserStats.SetStat("stat_didntRefund", Mathf.Round(stat_didntRefund));
      SteamUserStats.SetStat("stat_devTagUsed", BoolToInt(stat_devTagUsed));
      SteamUserStats.SetStat("stat_largeObjectInAir", BoolToInt(stat_largeObjectInAir));
      SteamUserStats.SetStat("stat_airBoost", BoolToInt(stat_airBoost));
      SteamUserStats.SetStat("stat_allPowerupsAtOnce", BoolToInt(stat_allPowerupsAtOnce));
      SteamUserStats.SetStat("stat_plexusParkFallTenTimes", BoolToInt(stat_plexusParkFallTenTimes));

      bool bSuccess = SteamUserStats.StoreStats();
      // If this failed, we never sent anything to the server, try
      // again later.
      m_bStoreStats = !bSuccess;
    }

  }

  private void UnlockAchievement(Achievement_t achievement) {
    achievement.m_bAchieved = true;

    // the icon may change once it's unlocked
    //achievement.m_iIconImage = 0;

    // mark it down
    SteamUserStats.SetAchievement(achievement.m_eAchievementID.ToString());

    // Store stats end of frame
    m_bStoreStats = true;

    Debug.Log("Stored new Achievement: " + achievement.m_eAchievementID.ToString());
  }

  private void OnUserStatsReceived(UserStatsReceived_t pCallback) {

    if (!SteamManager.Initialized)
      return;

    // we may get callbacks for other games' stats arriving, ignore them
    if ((ulong)m_GameID == pCallback.m_nGameID) {
      if (EResult.k_EResultOK == pCallback.m_eResult) {
        Debug.Log("Received stats and achievements from Steam\n");

        m_bStatsValid = true;

        // load achievements
        foreach (Achievement_t ach in m_Achievements) {
          bool ret = SteamUserStats.GetAchievement(ach.m_eAchievementID.ToString(), out ach.m_bAchieved);
          if (ret) {
            ach.m_strName = SteamUserStats.GetAchievementDisplayAttribute(ach.m_eAchievementID.ToString(), "name");
            ach.m_strDescription = SteamUserStats.GetAchievementDisplayAttribute(ach.m_eAchievementID.ToString(), "desc");
          }
          else {
            Debug.LogWarning("SteamUserStats.GetAchievement failed for Achievement " + ach.m_eAchievementID + "\nIs it registered in the Steam Partner site?");
          }
        }

        int int_lookAtCredits;
        int int_sorry;
        int int_everyCar;
        int int_jansportShrineDiscovered;
        int int_didntRefund;
        int int_devTagUsed;
        int int_largeObjectInAir;
        int int_airBoost;
        int int_allPowerupsAtOnce;
        int int_plexusParkFallTenTimes;

        // load stats
        SteamUserStats.GetStat("stat_lookAtCredits", out int_lookAtCredits);
        SteamUserStats.GetStat("stat_sorry", out int_sorry);
        SteamUserStats.GetStat("stat_everyCar", out int_everyCar);
        SteamUserStats.GetStat("stat_jansportShrineDiscovered", out int_jansportShrineDiscovered);
        SteamUserStats.GetStat("stat_didntRefund", out int_didntRefund);
        SteamUserStats.GetStat("stat_devTagUsed", out int_devTagUsed);
        SteamUserStats.GetStat("stat_largeObjectInAir", out int_largeObjectInAir);
        SteamUserStats.GetStat("stat_airBoost", out int_airBoost);
        SteamUserStats.GetStat("stat_allPowerupsAtOnce", out int_allPowerupsAtOnce);
        SteamUserStats.GetStat("stat_plexusParkFallTenTimes", out int_plexusParkFallTenTimes);

        stat_lookAtCredits = IntToBool(int_lookAtCredits);
        stat_sorry = IntToBool(int_sorry);
        stat_everyCar = IntToBool(int_everyCar);
        stat_jansportShrineDiscovered = IntToBool(int_jansportShrineDiscovered);
        stat_didntRefund = (float)int_didntRefund;
        stat_devTagUsed = IntToBool(int_devTagUsed);
        stat_largeObjectInAir = IntToBool(int_largeObjectInAir);
        stat_airBoost = IntToBool(int_airBoost);
        stat_allPowerupsAtOnce = IntToBool(int_allPowerupsAtOnce);
        stat_plexusParkFallTenTimes = IntToBool(int_plexusParkFallTenTimes);

      }
      else {
        Debug.Log("RequestStats - failed, " + pCallback.m_eResult);
      }
    }
  }

  private void OnUserStatsStored(UserStatsStored_t pCallback) {
    // we may get callbacks for other games' stats arriving, ignore them
    if ((ulong)m_GameID == pCallback.m_nGameID) {
      if (EResult.k_EResultOK == pCallback.m_eResult) {
        Debug.Log("StoreStats - success");
      }
      else if (EResult.k_EResultInvalidParam == pCallback.m_eResult) {
        // One or more stats we set broke a constraint. They've been reverted,
        // and we should re-iterate the values now to keep in sync.
        Debug.Log("StoreStats - some failed to validate");
        // Fake up a callback here so that we re-load the values.
        UserStatsReceived_t callback = new UserStatsReceived_t();
        callback.m_eResult = EResult.k_EResultOK;
        callback.m_nGameID = (ulong)m_GameID;
        OnUserStatsReceived(callback);
      }
      else {
        Debug.Log("StoreStats - failed, " + pCallback.m_eResult);
      }
    }
  }

  private void OnAchievementStored(UserAchievementStored_t pCallback) {
    // We may get callbacks for other games' stats arriving, ignore them
    if ((ulong)m_GameID == pCallback.m_nGameID) {
      if (0 == pCallback.m_nMaxProgress) {
        Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' unlocked!");
      }
      else {
        Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' progress callback, (" + pCallback.m_nCurProgress + "," + pCallback.m_nMaxProgress + ")");
      }
    }
  }

  private int BoolToInt(bool b) {
    int a;
    return a = (b) ? 1 : 0;
  }

  private bool IntToBool(int i) {
    bool b;
    return b = (i == 1) ? true : false;
  }

  private class Achievement_t {
    public Achievement m_eAchievementID;
    public string m_strName;
    public string m_strDescription;
    public bool m_bAchieved;

    /// <summary>
    /// Creates an Achievement. You must also mirror the data provided here in https://partner.steamgames.com/apps/achievements/yourappid
    /// </summary>
    /// <param name="achievement">The "API Name Progress Stat" used to uniquely identify the achievement.</param>
    /// <param name="name">The "Display Name" that will be shown to players in game and on the Steam Community.</param>
    /// <param name="desc">The "Description" that will be shown to players in game and on the Steam Community.</param>
    public Achievement_t(Achievement achievementID, string name, string desc) {
      m_eAchievementID = achievementID;
      m_strName = name;
      m_strDescription = desc;
      m_bAchieved = false;
    }
  }

  private void OnDestroy() {
    PlayerPrefs.SetFloat("CKYiscwBXR6VhzWh", DataManager.TotalTimePlayed);
  }
  private void OnApplicationQuit() {
    PlayerPrefs.SetFloat("CKYiscwBXR6VhzWh", DataManager.TotalTimePlayed);
  }
}
