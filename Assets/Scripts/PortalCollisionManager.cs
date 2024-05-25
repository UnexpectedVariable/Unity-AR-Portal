using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Collider))]
    internal class PortalCollisionManager : MonoBehaviour
    {
        private bool _isInside = false;

        [SerializeField]
        private GameObject _environmentContainer = null;
        [SerializeField]
        private Material[] _props = null;
        private void Start()
        {
            int count = _environmentContainer.transform.childCount;
            _props = new Material[count];
            for(int i = 0; i < count; i++)
            {
                _props[i] = _environmentContainer.transform.GetChild(i).GetComponent<Renderer>().sharedMaterial;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "MainCamera") return;

            _isInside = !_isInside;
            var compareValue = _isInside ? CompareFunction.NotEqual : CompareFunction.Equal;
            foreach(var prop in _props)
            {
                prop.SetInt("_StencilTest", (int)compareValue);
            }
        }
    }
}
