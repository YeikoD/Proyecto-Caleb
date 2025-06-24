using UnityEngine;

public class RayCast : MonoBehaviour
{
	LayerMask mask;
	public float distancia = 1.5f;

	public Texture2D puntero;
	public GameObject TextDetect;
	GameObject ultimoReconocido = null;

	void Start()
	{
		mask = LayerMask.GetMask("Raycast");
		TextDetect.SetActive(false);
	}


	void Update()
	{
		RaycastHit hit;

		if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distancia, mask))
		{
			Deselect();
			SelectedObject(hit.transform);

			if (hit.collider.tag == "NPC")
			{
				if (Input.GetKeyDown(KeyCode.E))
				{
					hit.collider.transform.GetComponent<NPCDeciciones>().IniciarDialogo();
				}

				var inventario = hit.collider.transform.GetComponent<Inventario>();
				EventManager.TriggerEvent("EntregarRecursoRepartidor", inventario);
				hit.collider.transform.GetComponent<Inventario>().MostrarInventario();
			}
			else if (hit.collider.tag == "MESA")
			{
				hit.collider.transform.GetComponent<Inventario>().MostrarInventario();
			}

			Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * distancia, Color.green);
		}
		else
		{
			Deselect();
		}
	}

	void SelectedObject(Transform transform)
	{
		transform.GetComponent<MeshRenderer>().material.color = Color.green;
		ultimoReconocido = transform.gameObject;
	}

	void Deselect()
	{
		if (ultimoReconocido)
		{
			ultimoReconocido.GetComponent<Renderer>().material.color = Color.white;
			ultimoReconocido = null;
		}
	}

	void OnGUI()
	{
		Rect rect = new Rect(Screen.width / 2, Screen.height / 2, puntero.width, puntero.height);
		GUI.DrawTexture(rect, puntero);

		if (ultimoReconocido)
		{
			TextDetect.SetActive(true);
		}
		else
		{
			TextDetect.SetActive(false);
		}
	}
}
