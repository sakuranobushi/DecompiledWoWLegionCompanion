using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WowJamMessages;
using WowStatConstants;
using WowStaticData;

public class CombatAllyDialog : MonoBehaviour
{
	public FollowerInventoryListItem m_combatAllyChampionListItemPrefab;

	public GameObject m_combatAllyListContent;

	public Text m_combatAllyCost;

	public Image m_combatAllyCostResourceIcon;

	public Text m_titleText;

	public CombatAllyDialog()
	{
	}

	private void CreateCombatAllyItems(int combatAllyMissionID, int combatAllyMissionCost)
	{
		foreach (JamGarrisonFollower value in PersistentFollowerData.followerDictionary.Values)
		{
			FollowerStatus followerStatus = GeneralHelpers.GetFollowerStatus(value);
			if (value.ZoneSupportSpellID <= 0 || followerStatus != FollowerStatus.available && followerStatus != FollowerStatus.onMission)
			{
				continue;
			}
			FollowerInventoryListItem followerInventoryListItem = UnityEngine.Object.Instantiate<FollowerInventoryListItem>(this.m_combatAllyChampionListItemPrefab);
			followerInventoryListItem.transform.SetParent(this.m_combatAllyListContent.transform, false);
			followerInventoryListItem.SetCombatAllyChampion(value, combatAllyMissionID, combatAllyMissionCost);
		}
	}

	public void Init()
	{
		FollowerInventoryListItem[] componentsInChildren = this.m_combatAllyListContent.GetComponentsInChildren<FollowerInventoryListItem>(true);
		for (int i = 0; i < (int)componentsInChildren.Length; i++)
		{
			UnityEngine.Object.DestroyImmediate(componentsInChildren[i].gameObject);
		}
		int missionCost = 0;
		IEnumerator enumerator = PersistentMissionData.missionDictionary.Values.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				JamGarrisonMobileMission current = (JamGarrisonMobileMission)enumerator.Current;
				GarrMissionRec record = StaticDB.garrMissionDB.GetRecord(current.MissionRecID);
				if (record != null)
				{
					if ((record.Flags & 16) == 0)
					{
						continue;
					}
					this.CreateCombatAllyItems(current.MissionRecID, (int)record.MissionCost);
					missionCost = (int)record.MissionCost;
					break;
				}
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			IDisposable disposable1 = disposable;
			if (disposable != null)
			{
				disposable1.Dispose();
			}
		}
		if (missionCost > GarrisonStatus.Resources())
		{
			this.m_combatAllyCost.text = string.Concat(new object[] { StaticDB.GetString("COST2", "Cost:"), " <color=#ff0000ff>", missionCost, "</color>" });
		}
		else
		{
			this.m_combatAllyCost.text = string.Concat(new object[] { StaticDB.GetString("COST2", "Cost:"), " <color=#ffffffff>", missionCost, "</color>" });
		}
		Sprite sprite = GeneralHelpers.LoadCurrencyIcon(1220);
		if (sprite != null)
		{
			this.m_combatAllyCostResourceIcon.sprite = sprite;
		}
	}

	private void OnDisable()
	{
		Main.instance.m_canvasBlurManager.RemoveBlurRef_MainCanvas();
		Main.instance.m_backButtonManager.PopBackAction();
	}

	public void OnEnable()
	{
		Main.instance.m_UISound.Play_ShowGenericTooltip();
		Main.instance.m_canvasBlurManager.AddBlurRef_MainCanvas();
		Main.instance.m_backButtonManager.PushBackAction(BackAction.hideAllPopups, null);
	}

	public void Start()
	{
		this.m_combatAllyCost.font = GeneralHelpers.LoadStandardFont();
		this.m_titleText.text = StaticDB.GetString("COMBAT_ALLY", null);
	}
}