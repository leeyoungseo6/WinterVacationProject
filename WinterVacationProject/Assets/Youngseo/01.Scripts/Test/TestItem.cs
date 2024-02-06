using UnityEngine;

namespace YS
{
    public class TestItem : MonoBehaviour, ISelectable
    {
        private Material _outlineMat;
        private readonly int _outlineThickness = Shader.PropertyToID("_OutlineThickness");

        private void Awake()
        {
            _outlineMat = GetComponent<MeshRenderer>().materials[^1];
        }

        public void MouseEnter()
        {
            _outlineMat.SetFloat(_outlineThickness, 0.03f);
        }
        
        public void MouseExit()
        {
            _outlineMat.SetFloat(_outlineThickness, 0);
        }

        public void OnSelect()
        {
            
        }

        public Transform GetTransform() => transform;
    }
}