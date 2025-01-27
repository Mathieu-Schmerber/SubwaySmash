using System.Collections.Generic;
using System.Linq;
using Databases;
using UnityEngine;

namespace Game.Entities.GPE.BBQ
{
    public class Ignitable : MonoBehaviour
    {
        private static readonly int Progress = Shader.PropertyToID("_Progress");
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
        private Shader _textureShader;
        private Shader _baseColorShader;
        
        [SerializeField, Range(0, 1)] private float _carbonatedProgress;
        private GameObject _onFireFX;

        private ParticleSystem _fx;
        private float _burnTime;
        private bool _isBurning;
        private List<Renderer> _renderers;

        public float BurnTime => _burnTime;
        public float BurnProgress => _carbonatedProgress;

        private void Awake()
        {
            _onFireFX = RuntimeDatabase.Data.FireFx;
            _textureShader = Shader.Find("Shader Graphs/Carbonated");
            _baseColorShader = Shader.Find("Shader Graphs/CarbonatedSimple");
            _carbonatedProgress = 0;
            _renderers = GetComponentsInChildren<Renderer>().ToList();
            _renderers.RemoveAll(x => x is ParticleSystemRenderer or SpriteRenderer || x.tag.Equals("FX"));
        }

        public void StartIgnite(float burnTime, float _initialProgress = 0)
        {
            if (_isBurning) return;
            SetupMaterials();
            var instance = Instantiate(_onFireFX, transform.position, transform.rotation, transform);
            _fx = instance.GetComponent<ParticleSystem>();
            _burnTime = burnTime;
            _isBurning = true;
            _carbonatedProgress = _initialProgress;
        }

        private void Update()
        {
            if (!_isBurning) return;

            // Increment progress over time
            _carbonatedProgress += Time.deltaTime / _burnTime;
            _carbonatedProgress = Mathf.Clamp01(_carbonatedProgress);

            // Apply the progress to all materials
            ApplyProgressToMaterials();

            // Stop burning when progress reaches 1
            if (_carbonatedProgress >= 1)
            {
                _isBurning = false;
                _fx.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }
        
        private void ApplyProgressToMaterials()
        {
            foreach (var renderer in _renderers)
            {
                // Apply progress to each material
                foreach (var material in renderer.materials)
                {
                    if (material.HasProperty(Progress))
                        material.SetFloat(Progress, _carbonatedProgress);
                }
            }
        }

        private void SetupMaterials()
        {
            // Find the shaders
            _textureShader = Shader.Find("Shader Graphs/Carbonated");
            _baseColorShader = Shader.Find("Shader Graphs/CarbonatedSimple");

            if (_textureShader == null || _baseColorShader == null)
            {
                Debug.LogError("Shaders not found: Ensure Shader Graphs/Carbonated and Shader Graphs/CarbonatedSimple exist.");
                return;
            }

            // Replace materials for all renderers
            foreach (var render in _renderers)
            {
                // Process each material
                var materials = render.sharedMaterials;
                for (int i = 0; i < materials.Length; i++)
                {
                    Material material = materials[i];
                    Material newMaterial;

                    if (material.shader == _textureShader || material.shader == _baseColorShader)
                        continue;
                    
                    // Check if the material has a texture
                    if (material.HasTexture(MainTex) && material.GetTexture(MainTex) != null)
                    {
                        // Create a material using the texture shader
                        newMaterial = new Material(_textureShader);
                        newMaterial.SetTexture(MainTex, material.GetTexture(MainTex));
                    }
                    else
                    {
                        // Create a material using the base color shader
                        newMaterial = new Material(_baseColorShader);
                        if (material.HasProperty(BaseColor))
                        {
                            // Copy the base color from the original material if it exists
                            newMaterial.SetColor(BaseColor, material.GetColor(BaseColor));
                        }
                        else
                        {
                            // Default base color if not present in the original material
                            newMaterial.SetColor(BaseColor, Color.white);
                        }
                    }

                    newMaterial.SetFloat(Progress, _carbonatedProgress);
                    // Replace the material
                    materials[i] = newMaterial;
                }

                // Apply the updated materials to the renderer
                render.sharedMaterials = materials;
            }
        }
    }
}