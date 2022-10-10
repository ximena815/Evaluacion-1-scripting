using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ClickableTile : MonoBehaviour {

	public BoxCollider collider;
	public int Xtile;
	public int Ytile;
	public TileMap Mapa;

	private void Start()
	{
		collider = GetComponent<BoxCollider>();
	}

	void OnMouseUp() {
		Debug.Log ("Click");

		if(EventSystem.current.IsPointerOverGameObject())
			return;

		Mapa.GeneratePathTo(Xtile, Ytile);
	}

	public void ColliderArriba()
    {
		collider.center = new Vector3(0, 0, -1);
		StartCoroutine(ColliderAbajo());
    }

	IEnumerator ColliderAbajo()
    {
		yield return new WaitForSeconds(0.2f);
		collider.center = new Vector3(0, 0, 0);
	}
}
