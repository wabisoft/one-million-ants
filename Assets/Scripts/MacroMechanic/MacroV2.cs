using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CombineInstance stores the list of meshes.  
// These are combined and assigned to the attached Mesh.

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MacroV2 : MonoBehaviour
{
    private float scalingFactor;

    private bool nozzleSlot;
    private bool headSlot;
    private bool wing1Slot;
    private bool wing2Slot;
    private bool wing3Slot;
    private bool wing4Slot;

    void Start()
    {
        scalingFactor = transform.localScale.x/100f;
        nozzleSlot = true;
        headSlot = true;
        wing1Slot = true;
        wing2Slot = true;
        wing3Slot = true;
        wing4Slot = true;
    }

    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.rigidbody)
        {
            if(collision.gameObject.CompareTag("Nozzle") &&  nozzleSlot == true)
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                Destroy(GetComponent<MeshCollider>());

                transform.position += new Vector3(0f,0f,1.0f); // make room for nozzle
                // rotate, then translate to avoid gimbal aliasing
                collision.transform.rotation = transform.rotation;
                collision.transform.position = transform.position - scalingFactor * transform.forward; // blender models have z up

                meshCombine(collision.gameObject);

                // turn off ability to see nozzle object -- unecessary, but w/e
                nozzleSlot = false;
            }

            if(collision.gameObject.CompareTag("Head") &&  headSlot == true)
            {
                Debug.Log("Blah");
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                Destroy(GetComponent<MeshCollider>());

                // rotate, then translate to avoid gimbal aliasing
                collision.transform.rotation = transform.rotation;
                collision.transform.position = transform.position +  (7.0f * scalingFactor * transform.forward); // blender models have z up

                meshCombine(collision.gameObject);

                // turn off ability to see head object -- unecessary, but w/e
                headSlot = false;
            }

            if(collision.gameObject.CompareTag("Wing1") &&  wing1Slot == true)
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                Destroy(GetComponent<MeshCollider>());

                // rotate, then translate to avoid gimbal aliasing
                collision.transform.rotation = transform.rotation;
                collision.transform.position = transform.position;

                collision.transform.RotateAround(collision.transform.position, collision.transform.right, 90.0f);
                collision.transform.position += 1.3f * scalingFactor * collision.transform.right;
                collision.transform.position += 1.5f * scalingFactor * collision.transform.up;
                
                meshCombine(collision.gameObject);

                // turn off ability to see wing1 object
                wing1Slot = false;
            }

            if(collision.gameObject.CompareTag("Wing2") &&  wing2Slot == true)
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                Destroy(GetComponent<MeshCollider>());

                // rotate, then translate to avoid gimbal aliasing
                collision.transform.rotation = transform.rotation;
                collision.transform.position = transform.position;

                collision.transform.RotateAround(collision.transform.position, collision.transform.right, 90.0f);
                collision.transform.RotateAround(collision.transform.position, collision.transform.up, 180.0f);
                collision.transform.position += 1.3f * scalingFactor * collision.transform.right;
                collision.transform.position += 1.5f * scalingFactor * collision.transform.up;
                
                meshCombine(collision.gameObject);

                // turn off ability to see wing2 object
                wing2Slot = false;
            }

            if(collision.gameObject.CompareTag("Wing3") &&  wing3Slot == true)
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                Destroy(GetComponent<MeshCollider>());

                // rotate, then translate to avoid gimbal aliasing
                collision.transform.rotation = transform.rotation;
                collision.transform.position = transform.position;

                collision.transform.RotateAround(collision.transform.position, collision.transform.right, 90.0f);
                collision.transform.RotateAround(collision.transform.position, collision.transform.up, 90.0f);
                collision.transform.position += 1.3f * scalingFactor * collision.transform.right;
                collision.transform.position += 1.5f * scalingFactor * collision.transform.up;
                
                meshCombine(collision.gameObject);

                // turn off ability to see wing3 object
                wing3Slot = false;
            }

            if(collision.gameObject.CompareTag("Wing4") &&  wing4Slot == true)
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                Destroy(GetComponent<MeshCollider>());

                // rotate, then translate to avoid gimbal aliasing
                collision.transform.rotation = transform.rotation;
                collision.transform.position = transform.position;

                collision.transform.RotateAround(collision.transform.position, collision.transform.right, 90.0f);
                collision.transform.RotateAround(collision.transform.position, collision.transform.up, 270.0f);
                collision.transform.position += 1.3f * scalingFactor * collision.transform.right;
                collision.transform.position += 1.5f * scalingFactor * collision.transform.up;
                
                meshCombine(collision.gameObject);

                // turn off ability to see wing4 object
                wing4Slot = false;
            }
        }
    }

    // Anakin: "it's working!!"
    void meshCombine (GameObject collisionObj)
    {
        MeshFilter[] meshFilters =  new MeshFilter[2];
        meshFilters[0] = GetComponent<MeshFilter>();
        meshFilters[1] = collisionObj.GetComponent<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for(int i = 0; i < meshFilters.Length; i++)
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
        transform.gameObject.AddComponent<MeshCollider>();
        transform.gameObject.GetComponent<MeshCollider>().convex = true;
        transform.gameObject.SetActive(true);
    }
}
