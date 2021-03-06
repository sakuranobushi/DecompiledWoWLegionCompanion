using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WowStatConstants;
using WowStaticData;

namespace WoWCompanionApp
{
	public class CombatAllyListItem : MonoBehaviour
	{
		public MissionFollowerSlot m_combatAllySlot;

		public Text m_combatAllyLabel;

		public Text m_assignChampionText;

		public Text m_championName;

		public Text m_combatAllyAbilityText;

		public Text m_combatAbilityName;

		public SpellDisplay m_combatAllySupportSpellDisplay;

		public GameObject m_unassignCombatAllyButton;

		public Text m_unassignCombatAllyButtonLabel;

		public CombatAllyDialog m_combatAllyDialog;

		private int m_combatAllyMissionID;

		public CombatAllyListItem()
		{
		}

		private void Awake()
		{
			this.m_combatAllyLabel.font = GeneralHelpers.LoadStandardFont();
			this.m_assignChampionText.font = GeneralHelpers.LoadStandardFont();
			this.m_championName.font = GeneralHelpers.LoadStandardFont();
			this.m_combatAllyAbilityText.font = GeneralHelpers.LoadStandardFont();
			this.m_combatAbilityName.font = GeneralHelpers.LoadStandardFont();
			this.m_unassignCombatAllyButtonLabel.font = GeneralHelpers.LoadStandardFont();
			this.m_combatAllyLabel.text = StaticDB.GetString("COMBAT_ALLY", null);
			this.m_assignChampionText.text = StaticDB.GetString("ORDER_HALL_ZONE_SUPPORT_DESCRIPTION_2", null);
			this.m_combatAllyAbilityText.text = StaticDB.GetString("COMBAT_ALLY_ABILITY", null);
			this.m_unassignCombatAllyButtonLabel.text = StaticDB.GetString("UNASSIGN", null);
			this.UpdateVisuals();
		}

		private void ClearCombatAllyDisplay()
		{
			this.m_combatAllySlot.SetFollower(0);
			this.m_combatAllyLabel.gameObject.SetActive(true);
			this.m_assignChampionText.gameObject.SetActive(true);
			this.m_championName.gameObject.SetActive(false);
			this.m_combatAllyAbilityText.gameObject.SetActive(false);
			this.m_combatAbilityName.gameObject.SetActive(false);
			this.m_combatAllySupportSpellDisplay.gameObject.SetActive(false);
			this.m_unassignCombatAllyButton.SetActive(false);
		}

		public void HandleCompleteMissionResult(int garrMissionID, int result, int missionSuccessChance)
		{
			this.UpdateVisuals();
		}

		public void HandleDataResetFinished()
		{
			this.UpdateVisuals();
		}

		public void HideCombatAllyDialog()
		{
			this.m_combatAllyDialog.gameObject.SetActive(false);
		}

		private void OnDisable()
		{
			Main.instance.GarrisonDataResetFinishedAction -= new Action(this.HandleDataResetFinished);
		}

		private void OnEnable()
		{
			this.ClearCombatAllyDisplay();
			this.UpdateVisuals();
			Main.instance.GarrisonDataResetFinishedAction += new Action(this.HandleDataResetFinished);
		}

		public void ShowCombatAllyDialog()
		{
			this.m_combatAllyDialog.gameObject.SetActive(true);
			this.m_combatAllyDialog.Init();
		}

		public void UnassignCombatAlly()
		{
			Main.instance.CompleteMission(this.m_combatAllyMissionID);
		}

		public void UpdateVisuals()
		{
			CombatAllyMissionState combatAllyMissionState = CombatAllyMissionState.notAvailable;
			foreach (WrapperGarrisonMission value in PersistentMissionData.missionDictionary.Values)
			{
				GarrMissionRec record = StaticDB.garrMissionDB.GetRecord(value.MissionRecID);
				if (record != null)
				{
					if ((record.Flags & 16) == 0)
					{
						continue;
					}
					this.m_combatAllyMissionID = value.MissionRecID;
					combatAllyMissionState = (value.MissionState != 1 ? CombatAllyMissionState.available : CombatAllyMissionState.inProgress);
					break;
				}
			}
			if (combatAllyMissionState != CombatAllyMissionState.inProgress)
			{
				this.ClearCombatAllyDisplay();
			}
			else
			{
				foreach (WrapperGarrisonFollower wrapperGarrisonFollower in PersistentFollowerData.followerDictionary.Values)
				{
					if (wrapperGarrisonFollower.CurrentMissionID != this.m_combatAllyMissionID)
					{
						continue;
					}
					this.m_combatAllySlot.SetFollower(wrapperGarrisonFollower.GarrFollowerID);
					this.m_combatAllyLabel.gameObject.SetActive(false);
					this.m_assignChampionText.gameObject.SetActive(false);
					this.m_championName.gameObject.SetActive(true);
					GarrFollowerRec garrFollowerRec = StaticDB.garrFollowerDB.GetRecord(wrapperGarrisonFollower.GarrFollowerID);
					CreatureRec creatureRec = StaticDB.creatureDB.GetRecord((GarrisonStatus.Faction() != PVP_FACTION.ALLIANCE ? garrFollowerRec.HordeCreatureID : garrFollowerRec.AllianceCreatureID));
					if (wrapperGarrisonFollower.Quality == 6 && garrFollowerRec.TitleName != null && garrFollowerRec.TitleName.Length > 0)
					{
						this.m_championName.text = garrFollowerRec.TitleName;
					}
					else if (garrFollowerRec != null)
					{
						this.m_championName.text = creatureRec.Name;
					}
					this.m_championName.color = GeneralHelpers.GetQualityColor(wrapperGarrisonFollower.Quality);
					this.m_combatAllySupportSpellDisplay.gameObject.SetActive(true);
					this.m_combatAllySupportSpellDisplay.SetSpell(wrapperGarrisonFollower.ZoneSupportSpellID);
					this.m_unassignCombatAllyButton.SetActive(true);
					break;
				}
			}
		}
	}
}