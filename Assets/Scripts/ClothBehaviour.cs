

using Unity.VisualScripting;
using UnityEngine;

namespace RevisedParticle
{
    // Implementation of the Mass spring model for cloth 
    public class ClothBehaviour : MonoBehaviour
    {
        public SimulationValues simStats;
        public ParticleManager ParticleManager { get; private set; }
        public SpringManager SpringManager { get; private set; }
        
        private Mesh _mesh;
        private Transform _cachedTransform;
        
        private int _frameCounter = 0;
        private const int _gizmoUpdateFrequency = 1;
        private Vector3[] _meshVertices;

        void Start()
        {
            _cachedTransform = transform;

            // Creates the Particle and Spring Manager classes with initial simulation data
            ParticleManager = new ParticleManager(simStats);
            SpringManager = new SpringManager(simStats);

            // Get the MeshFilter component
            MeshFilter meshFilter = GetComponent<MeshFilter>();

            // Sets up the particles and springs
            ParticleManager.SetupParticles(_cachedTransform);
            SpringManager.SpawnSprings(ParticleManager.particleArr);

            _mesh = new Mesh();
            _mesh.name = "GeneratedClothMesh";
            meshFilter.mesh = _mesh;

            GenerateClothMesh(ParticleManager.rows, ParticleManager.columns, simStats.spacing);

            // Ensure meshVertices array is correctly sized to fit the mesh's vertices
            _meshVertices = new Vector3[_mesh.vertexCount];

            // Initial population of meshVertices from particle positions (world to local space)
            for (int i = 0; i < simStats.rows; i++)
            {
                for (int j = 0; j < simStats.columns; j++)
                {
                    int oneDIndex = (i * simStats.columns) + j;
                    // Check bounds just in case, though it should match now
                    if (oneDIndex < _meshVertices.Length)
                    {
                        // Convert world-space particle position to local space of this GameObject
                        _meshVertices[oneDIndex] =
                            _cachedTransform.InverseTransformPoint(ParticleManager.particleArr[i, j].pos);
                    }
                }
            }

            // Apply initial vertices to the mesh
            _mesh.vertices = _meshVertices;
            _mesh.RecalculateNormals();

            Debug.Log("The mesh vertex count is " + _mesh.vertexCount);
        }
        

        private void GenerateClothMesh(int rows, int columns, float spacing)
        {
            // Calculate total number of vertices (each particle is a vertex)
            int vertexCount = rows * columns;
            Vector3[] vertices = new Vector3[vertexCount];
            Vector2[] uvs = new Vector2[vertexCount]; // For texture mapping

            // Populate initial vertex positions and UVs
            
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    int index = (r * columns) + c;
                    // Initial local position. Adjust based on how your particles are spawned relative to origin.
                    // This creates a flat grid.
                    vertices[index] = new Vector3(c * spacing, -r * spacing, 0); // Assuming your ParticleManager spawns (x,y) with x increasing horizontally, y decreasing vertically

                    // UVs for texture mapping (0 to 1 range)
                    uvs[index] = new Vector2((float)c / (columns - 1), (float)r / (rows - 1));
                }
            }

            // Calculate total number of triangles (each quad is 2 triangles)
            // A (rows)x(columns) grid has (rows-1)x(columns-1) quads
            int numQuads = (rows - 1) * (columns - 1);
            int[] triangles = new int[numQuads * 6]; // 6 indices per quad (2 triangles * 3 vertices/triangle)

            int triIndex = 0;
            for (int r = 0; r < rows - 1; r++)
            {
                for (int c = 0; c < columns - 1; c++)
                {
                    // Get the 4 vertex indices for the current quad
                    int topLeft = (r * columns) + c;
                    int topRight = (r * columns) + c + 1;
                    int bottomLeft = ((r + 1) * columns) + c;
                    int bottomRight = ((r + 1) * columns) + c + 1;
                    
                    triangles[triIndex++] = bottomLeft;
                    triangles[triIndex++] = topLeft;
                    triangles[triIndex++] = topRight;
                    
                    triangles[triIndex++] = bottomLeft;
                    triangles[triIndex++] = topRight;
                    triangles[triIndex++] = bottomRight;
                }
            }
            
            _mesh.vertices = vertices;
            _mesh.triangles = triangles;
            _mesh.uv = uvs;

            // Redo Normals
            _mesh.RecalculateNormals();

            Debug.Log("Generated mesh with " + _mesh.vertexCount + "vertices and " + _mesh.triangles.Length / 3 +
                      " triangles.");
        }

        private void Update()
        {
            if (_mesh == null || ParticleManager == null || ParticleManager.particleArr == null || _meshVertices == null)
            {
                return;
            }

            // Loops through particles array, converting the world position to the local mesh position and assigning to vertices array
            for (int i = 0; i < simStats.rows; i++)
            {
                for (int j = 0; j < simStats.columns; j++)
                {
                    int oneDIndex = (i * simStats.columns) + j;
                    
                    if (oneDIndex < _meshVertices.Length)
                    {
                        // Updates the mesh vertices with their new positions
                        _meshVertices[oneDIndex] = _cachedTransform.InverseTransformPoint(ParticleManager.particleArr[i, j].pos);
                    }
                }
            }

            // Updated vertex array back to the mesh
            _mesh.vertices = _meshVertices;
            _mesh.RecalculateNormals();
            
        }

        void FixedUpdate()
        {
            ParticleManager.CalculateForces(Time.fixedDeltaTime);
            SpringManager.UpdateSprings(Time.fixedDeltaTime);
            ParticleManager.UpdateParticles(Time.fixedDeltaTime);
        }
        
        private void OnDrawGizmos()
        {
            if (ParticleManager.IsUnityNull()) return;
            
            _frameCounter++;
            if (_frameCounter % _gizmoUpdateFrequency != 0) return; // Makes sure the draw call isn't higher than framerate
            
            // Draws graphical representation within classes to show springs and particles using gizmos
            SpringManager.Draw();
            ParticleManager.Draw();
            
        } 
    }
}
