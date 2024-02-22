using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SimonSays.Presenter
{
    // This class is responsible for the SimonButton prefab
    public class SimonButton : MonoBehaviour
    {
        private int _buttonId;
        [SerializeField] private Image _buttonImage;
        [SerializeField] private Button _button;
        [SerializeField] private AudioClip[] _audioClips;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private GameObject _buttonLock;

        public event Action<int> Click;
        
        private readonly WaitForSeconds _computerClickDelay = new WaitForSeconds(0.25f);
        private readonly PointerEventData _pointerEventData = new PointerEventData(EventSystem.current);

        public void SetButtonId(int buttonId, Color buttonColor)
        {
            _buttonId = buttonId;
            _buttonImage.color = buttonColor;
            _audioSource.clip = _audioClips[buttonId];
        }

        public void OnClick()
        {
            Click?.Invoke(_buttonId);
            PlaySound();
        }

        public IEnumerator ComputerMove()
        {
            PlaySound();
            _button.OnPointerDown(_pointerEventData);
            yield return _computerClickDelay;
            _button.OnPointerUp(_pointerEventData);
        }

        private void PlaySound()
        {
            _audioSource.PlayOneShot(_audioClips[_buttonId]);
        }

        public int GetButtonId()
        {
            return _buttonId;
        }

        public void DisableButton()
        {
            _buttonLock.SetActive(true);
        }

        public void EnableButton()
        {
            _buttonLock.SetActive(false);
        }
    }
}