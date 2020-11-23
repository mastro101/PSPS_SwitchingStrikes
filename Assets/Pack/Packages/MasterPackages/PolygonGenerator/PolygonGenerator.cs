using System;
using System.Collections.Generic;
using UnityEngine;

public class PolygonGenerator : MonoBehaviour
{
    [SerializeField] MeshFilter mf = null;
    [SerializeField] SkinnedMeshRenderer smr = null;
    [Space]
    [SerializeField] [Range(1, 360)] int sides = 3;
    [SerializeField] float radius = 1;
    [SerializeField] bool changeBase = false;
    [SerializeField] bool revert = false;
    [SerializeField] axe plane = axe.y;

    public Action OnGenerate;

    [SerializeField][HideInInspector] Mesh mesh;
    [SerializeField][HideInInspector] List<Vector3> vertices;
    [SerializeField][HideInInspector] int[] indices;
    [SerializeField][HideInInspector] List<Vector2> UVs;

    [SerializeField][HideInInspector] Transform bonesParentTransform;

    [SerializeField][HideInInspector] Transform[] bones;
    [SerializeField][HideInInspector] Matrix4x4[] bindPoses;

    [SerializeField][HideInInspector] float generateRadius;

    private void OnDestroy()
    {
        Destroy();
    }

    void Destroy()
    {
        if (Application.isPlaying)
        {
            if (mesh)
                Destroy(mesh);
            if (bonesParentTransform)
                Destroy(bonesParentTransform.gameObject);
        }
        else
        {
            if (mesh)
                DestroyImmediate(mesh);
            if (bonesParentTransform)
                DestroyImmediate(bonesParentTransform.gameObject);
        }
    }

    public void GenerateMesh()
    {
        vertices = new List<Vector3>();
        indices = new int[sides * 3];
        UVs = new List<Vector2>(sides + 1);

        Destroy();

        mesh = new Mesh();
        mf.mesh = mesh;

        Vector3 upVector = Vector3.up;
        float delta = 360f / sides;
        // set vertices
        vertices.Add(Vector3.zero);
        float revertN = 1;
        if (revert)
            revertN = -1;
        Vector3 dir = Vector3.zero;
        switch (plane)
        {
            case axe.x:
                upVector = Vector3.up * radius;
                dir = Vector3.right;
                break;
            case axe.y:
                upVector = Vector3.forward * radius;
                dir = Vector3.up;
                break;
            case axe.z:
                upVector = Vector3.up * radius;
                dir = Vector3.forward;
                break;
            default:
                break;
        }

        if (changeBase)
        {
            if (sides % 2 == 0)
            {
                for (int i = 0; i < sides; i++)
                {
                    float f = delta * i * revertN;
                    vertices.Add((Quaternion.Euler(dir.x * f, dir.y * f, dir.z * f) * upVector));
                }
            }
            else
            {
                for (int i = 0; i < sides; i++)
                {
                    float f = (delta * i * revertN) + (delta / 2 * revertN);
                    vertices.Add((Quaternion.Euler(dir.x * f, dir.y * f, dir.z * f) * upVector));
                }
            }
        }
        else
        {
            if (sides % 2 == 0)
            {
                for (int i = 0; i < sides; i++)
                {
                    float f = (delta * i * revertN) + (delta / 2 * revertN);
                    vertices.Add((Quaternion.Euler(dir.x * f, dir.y * f, dir.z * f) * upVector));
                }
            }
            else
            {
                for (int i = 0; i < sides; i++)
                {
                    float f = delta * i * revertN;
                    vertices.Add((Quaternion.Euler(dir.x * f, dir.y * f, dir.z * f) * upVector));
                }
            }
        }
        mesh.SetVertices(vertices);

        // set indices
        int l = sides - 1;
        for (int i = 0; i < l; i++)
        {
            indices[i * 3] = 0;
            indices[i * 3 + 1] = i + 2;
            indices[i * 3 + 2] = i + 1;
        }
        indices[l * 3] = 0;
        indices[l * 3 + 2] = sides;
        indices[l * 3 + 1] = 1;
        mesh.SetIndices(indices, MeshTopology.Triangles, 0);

        // Set UV
        for (int i = 0; i < vertices.Count; i++)
        {
            Vector2 uvCoordinate = (VectorUtility.FromV3ToV2XY(vertices[i]) / 2) + (Vector2.one / 2);
            UVs.Add(uvCoordinate);
        }
        mesh.SetUVs(0, UVs);

        mesh.RecalculateNormals();

        // Set BoneWeight
        BoneWeight[] weights = new BoneWeight[vertices.Count];
        for (int i = 0; i < weights.Length; i++)
        {
            weights[i].boneIndex0 = i;
            weights[i].weight0 = 1;
        }

        bonesParentTransform = new GameObject("Bones").transform;
        bonesParentTransform.position = transform.position;
        bonesParentTransform.transform.SetParent(transform);

        bones = new Transform[vertices.Count];
        bindPoses = new Matrix4x4[vertices.Count];

        smr.sharedMesh = mesh;

        generateRadius = radius;

        OnGenerate?.Invoke();
    }

    public List<Vector3> GetVertexPositions()
    {
        return vertices;
    }

    public Vector3 GetVertexPositions(int i)
    {
        if (i < vertices.Count && i >= 0)
            return vertices[i];

        Debug.LogWarning("index " + i + " is not in the list");
        return Vector3.zero;
    }

    public float GetRadius()
    {
        return generateRadius;
    }

    enum axe
    {
        x, y, z,
    }
}