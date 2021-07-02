using UnityEngine;
using UnityEngine.UI;


namespace Tools
{
	public class PlayerController : MonoBehaviour
	{
		[SerializeField]
		private float _raycastMaxDistance = 5;
		[SerializeField]
		private Camera _playerCamera;
		[SerializeField]
		private Slider _hitMarker;
		[SerializeField]
		private Image _hitMarkerImage;
		private GameObject _gazedObject = null;
		private float _markerFillingSpeed = 100;


		void Update()
		{
			RaycastHit hit;

			if (Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out hit, _raycastMaxDistance))
			{
				if (hit.collider.gameObject != _gazedObject)
				{
					SetNewObjectAsGazed(hit);
				}
				if (IsCanBeUsed(hit))
				{
					FillHitMarkerOnGazingSameObject();
				}
				if (hit.collider.tag == "TeleportPosition")
				{
					_hitMarkerImage.color = Color.green;
					TeleportOnFullMarkerValue();
				}
				if (hit.collider.tag == "Enemies")
				{
					_hitMarkerImage.color = Color.red;
					DestroyEnemyOnFullMarkerValue();
				}
			}
			else
			{
				ResetOnTargetLose();
			}
		}


		private bool IsCanBeUsed(RaycastHit hit)
		{
			return hit.collider.tag == "TeleportPosition" | hit.collider.tag == "Enemies";
		}


		private void ResetOnTargetLose()
		{
			_gazedObject = null;
			_hitMarker.value = 0;
		}


		private void SetNewObjectAsGazed(RaycastHit hit)
		{
			_gazedObject = hit.collider.gameObject;
			_hitMarker.value = 0;
		}


		private void FillHitMarkerOnGazingSameObject()
		{
			_hitMarker.value += _markerFillingSpeed * Time.deltaTime;
		}


		private void TeleportOnFullMarkerValue()
		{
			if (_hitMarker.value == 100)
			{
				transform.position = _gazedObject.transform.position;
			}
		}


		private void DestroyEnemyOnFullMarkerValue()
		{
			if (_hitMarker.value == 100)
			{
				Destroy(_gazedObject);
			}
		}
	}
}
