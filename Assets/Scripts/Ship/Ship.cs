using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShipPart
{
    // Start is called before the first frame update
    void Combine(Ship ship);
}

public class Ship : MonoBehaviour
{
    public int MaxWings = 4;
    private int _numWings = 0;
    private bool _hasNose = false;
    private bool _hasNozzle = false;
    private float scalingFactor;
    public bool Complete
    {
        get {
            return MaxWings == 4 && _hasNose && _hasNozzle;
        }
    }

    void Start()
    {
        scalingFactor = transform.localScale.x / 100f;
    }

    void OnCollisionEnter(Collision other)
    {
        var p = other.gameObject.GetComponent<IShipPart>();
        if (p != null)
        {
            p.Combine(this);
        }
    }

    

    public void Attach(Wing wing)
    {
        if (_numWings > MaxWings) return;

        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Destroy(GetComponent<MeshCollider>());

        // rotate, then translate to avoid gimbal aliasing
        wing.transform.rotation = transform.rotation;
        wing.transform.position = transform.position;

        wing.transform.RotateAround(wing.transform.position, wing.transform.right, 90.0f);
        wing.transform.RotateAround(wing.transform.position, wing.transform.up, _numWings * 90.0f);
        wing.transform.position += 1.3f * scalingFactor * wing.transform.right;
        wing.transform.position += 1.5f * scalingFactor * wing.transform.up;

        MeshCombine(wing.gameObject);
        _numWings++;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void Attach(Nose nose)
    {
        if (_hasNose) return;

        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Destroy(GetComponent<MeshCollider>());

        // rotate, then translate to avoid gimbal aliasing
        nose.transform.rotation = transform.rotation;
        nose.transform.position = transform.position + (7.0f * scalingFactor * transform.forward); // blender models have z up

        MeshCombine(nose.gameObject);

        _hasNose = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void Attach(Nozzle nozzle)
    {
        if (_hasNozzle) return;

        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Destroy(GetComponent<MeshCollider>());

        transform.position += new Vector3(0f, 0f, 1.0f); // make room for nozzle
                                                         // rotate, then translate to avoid gimbal aliasing
        nozzle.transform.rotation = transform.rotation;
        nozzle.transform.position = transform.position - scalingFactor * transform.forward; // blender models have z up

        MeshCombine(nozzle.gameObject);

        _hasNozzle = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }


    // Anakin: "it's working!!"
    // https://www.youtube.com/watch?v=AXwGVXD7qEQ 
    protected void MeshCombine(GameObject collisionObj)
    {
        MeshFilter[] meshFilters = new MeshFilter[2];
        meshFilters[0] = GetComponent<MeshFilter>();
        meshFilters[1] = collisionObj.GetComponent<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            combine[i].transform = transform.worldToLocalMatrix * meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        transform.GetComponent<MeshFilter>().mesh.RecalculateBounds();
        transform.GetComponent<MeshFilter>().mesh.RecalculateNormals();
        Destroy(gameObject.GetComponent<BoxCollider>());
        transform.gameObject.AddComponent<BoxCollider>();
        transform.gameObject.SetActive(true);
        Destroy(collisionObj);
    }
}
