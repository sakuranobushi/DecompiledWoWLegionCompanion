using System;
using UnityEngine;

namespace WoWCompanionApp
{
	public class MissionDialog : MonoBehaviour
	{
		public MissionDetailView m_missionDetailView;

		public FollowerListView m_followerListView;

		public MissionDialog()
		{
		}

		private void HandleMissionSelected(int garrMissionID)
		{
			if (garrMissionID != 0)
			{
				this.SetMission(garrMissionID);
			}
			else
			{
				this.m_missionDetailView.gameObject.SetActive(false);
			}
		}

		private void OnDisable()
		{
			AdventureMapPanel.instance.MissionSelectedFromListAction -= new Action<int>(this.HandleMissionSelected);
		}

		private void OnEnable()
		{
			AdventureMapPanel.instance.MissionSelectedFromListAction += new Action<int>(this.HandleMissionSelected);
		}

		public void SetMission(int garrMissionID)
		{
			this.m_missionDetailView.gameObject.SetActive(true);
			this.m_missionDetailView.HandleMissionSelected(garrMissionID);
			this.m_followerListView.m_missionDetailView = this.m_missionDetailView;
			this.m_followerListView.InitFollowerList();
			this.m_followerListView.HandleMissionChanged(garrMissionID);
		}
	}
}