using System;
using System.Collections;
using UnityEngine;

namespace WowStaticData
{
	public class SpellEffectDB
	{
		private Hashtable m_records;

		public SpellEffectDB()
		{
		}

		public void EnumRecords(Predicate<SpellEffectRec> callback)
		{
			IEnumerator enumerator = this.m_records.Values.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					if (callback((SpellEffectRec)enumerator.Current))
					{
						continue;
					}
					break;
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
		}

		public void EnumRecordsByParentID(int parentID, Predicate<SpellEffectRec> callback)
		{
			IEnumerator enumerator = this.m_records.Values.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					SpellEffectRec current = (SpellEffectRec)enumerator.Current;
					if (current.SpellID != parentID || callback(current))
					{
						continue;
					}
					break;
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
		}

		public SpellEffectRec GetRecord(int id)
		{
			return (SpellEffectRec)this.m_records[id];
		}

		public bool Load(string path, AssetBundle nonLocalizedBundle, AssetBundle localizedBundle, string locale)
		{
			string str = string.Concat(path, "NonLocalized/SpellEffect.txt");
			if (this.m_records != null)
			{
				Debug.Log(string.Concat("Already loaded static db ", str));
				return false;
			}
			TextAsset textAsset = nonLocalizedBundle.LoadAsset<TextAsset>(str);
			if (textAsset == null)
			{
				Debug.Log(string.Concat("Unable to load static db ", str));
				return false;
			}
			string str1 = textAsset.ToString();
			this.m_records = new Hashtable();
			int num = 0;
			int num1 = 0;
			do
			{
				num = str1.IndexOf('\n', num1);
				if (num < 0)
				{
					continue;
				}
				string str2 = str1.Substring(num1, num - num1 + 1).Trim();
				SpellEffectRec spellEffectRec = new SpellEffectRec();
				spellEffectRec.Deserialize(str2);
				this.m_records.Add(spellEffectRec.ID, spellEffectRec);
				num1 = num + 1;
			}
			while (num > 0);
			return true;
		}
	}
}