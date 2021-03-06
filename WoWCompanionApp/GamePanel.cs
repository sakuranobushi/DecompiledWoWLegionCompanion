using System;
using UnityEngine;

namespace WoWCompanionApp
{
	public class GamePanel : MonoBehaviour
	{
		public MiniMissionListPanel m_missionListPanel;

		public AdventureMapPanel m_mapPanel;

		public OrderHallFollowersPanel m_followersPanel;

		public TroopsPanel m_troopsPanel;

		public TalentTreePanel m_talentTreePanel;

		private GameObject m_currentPanel;

		public Action<OrderHallNavButton> OrderHallNavButtonSelectedAction;

		public GameObject m_navBarLayout;

		public GameObject m_birdEagleThings;

		public GamePanel()
		{
		}

		private void OnEnable()
		{
			this.m_mapPanel.CenterAndZoomOut();
		}

		public void SelectOrderHallNavButton(OrderHallNavButton navButton)
		{
			if (this.OrderHallNavButtonSelectedAction != null)
			{
				this.OrderHallNavButtonSelectedAction(navButton);
			}
		}

		public void ShowPanel(GameObject panel)
		{
			if (this.m_currentPanel == panel)
			{
				return;
			}
			if (this.m_currentPanel != null)
			{
				this.m_currentPanel.SetActive(false);
			}
			this.m_currentPanel = panel;
			this.m_currentPanel.SetActive(true);
		}

		private void Start()
		{
			if (this.m_missionListPanel != null && this.m_missionListPanel.gameObject.activeSelf)
			{
				this.m_currentPanel = this.m_missionListPanel.gameObject;
			}
			else if (this.m_mapPanel != null && this.m_mapPanel.gameObject.activeSelf)
			{
				this.m_currentPanel = this.m_mapPanel.gameObject;
			}
			else if (this.m_followersPanel != null && this.m_followersPanel.gameObject.activeSelf)
			{
				this.m_currentPanel = this.m_followersPanel.gameObject;
			}
			else if (this.m_troopsPanel != null && this.m_troopsPanel.gameObject.activeSelf)
			{
				this.m_currentPanel = this.m_troopsPanel.gameObject;
			}
			else if (this.m_talentTreePanel != null && this.m_talentTreePanel.gameObject.activeSelf)
			{
				this.m_currentPanel = this.m_troopsPanel.gameObject;
			}
			if (this.m_birdEagleThings != null)
			{
				this.m_birdEagleThings.SetActive(!Main.instance.IsNarrowScreen());
			}
			if (this.m_mapPanel != null)
			{
				AdventureMapPanel component = this.m_mapPanel.GetComponent<AdventureMapPanel>();
				if (component != null)
				{
					component.SetStartingMapByFaction();
				}
			}
		}
	}
}