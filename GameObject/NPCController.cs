using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Data;
using Managers;
using Models;
using SkillBridge.Message;

public class NPCController : MonoBehaviour {

	public int npcID;

	SkinnedMeshRenderer renderer;

	Animator anim;

	Color orignColor;

	private bool inInteractive = false;

	NpcDefine npc;

	NpcQuestStatus questStatus;

	

	// Use this for initialization
	void Start () {
		renderer = this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
		anim = this.gameObject.GetComponentInChildren<Animator>();
		orignColor = renderer.sharedMaterial.color;
		npc = NPCManager.Instance.GetNpcDefine(this.npcID);
		this.StartCoroutine(Actions());
		RefreshNpcStatus();
		QuestManager.Instance.onQuestStatusChanged += OnQuestStatusChanged;
		
	}

	void OnQuestStatusChanged(Quest quest)
    {
		this.RefreshNpcStatus();

	}
	void RefreshNpcStatus()
    {
		questStatus = QuestManager.Instance.GetQuestStatusByNpc(this.npcID);
		UIWorldElementManager.Instance.AddNpcQuestStatus(this.transform, questStatus);
    }

	void OnDestroy()
    {
		QuestManager.Instance.onQuestStatusChanged -= OnQuestStatusChanged;
		if (UIWorldElementManager.Instance != null)
			UIWorldElementManager.Instance.RemoveNpcQuestStatus(this.transform);
    }

	IEnumerator Actions()
    {
        while (true)
        {
			if (inInteractive)
				yield return new WaitForSeconds(2f);
			else
				yield return new WaitForSeconds(Random.Range(5f, 10f));

			this.Relax();
        }
    }

	// Update is called once per frame
	void Update () {
		
	}

	void Relax()
    {
		anim.SetTrigger("Relax");
    }

	void Interactive()
    {
		if (!inInteractive)
        {
			inInteractive = true;
			StartCoroutine(DoInteractive());
        }
    }

	IEnumerator DoInteractive()
    {
		yield return FaceToPlayer();
		if (NPCManager.Instance.Interactive(npc))
        {
			anim.SetTrigger("Talk");
        }
		yield return new WaitForSeconds(3f);
		inInteractive = false;
    }

	IEnumerator FaceToPlayer()
    {
		Vector3 faceTo = (User.Instance.CurrentCharacterObject.transform.position - this.transform.position).normalized;
		while (Mathf.Abs(Vector3.Angle(this.gameObject.transform.forward, faceTo)) > 5)
        {
			this.gameObject.transform.forward = Vector3.Lerp(this.gameObject.transform.forward, faceTo, Time.deltaTime * 5f);
			yield return null;
        }
    }

	void OnMouseDown()
    {
		Interactive();
    }

	private void OnMouseOver()
    {
		Highlight(true);
    }

	private void OnMouseEnter()
    {
		Highlight(true);
    }

	public void OnMouseExit()
    {
		Highlight(false);
    }

	void Highlight(bool hightlight)
    {
		if (hightlight)
        {
			if (renderer.sharedMaterial.color != Color.white)
				renderer.sharedMaterial.color = Color.white;
        }
        else
        {
			if (renderer.sharedMaterial.color != orignColor)
				renderer.sharedMaterial.color = orignColor;
        }
    }
}
