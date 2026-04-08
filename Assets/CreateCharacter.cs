using UnityEngine;
using UnityEditor;

public class CreateCharacter : EditorWindow
{
    [MenuItem("Tools/Create 3D Character")]
    public static void CreatePlayerCharacter()
    {
        // --- Root GameObject ---
        GameObject character = new GameObject("PlayerCharacter");
        character.tag = "Player";

        // --- Rigidbody ---
        Rigidbody rb = character.AddComponent<Rigidbody>();
        rb.mass = 75f;
        rb.linearDamping = 1f;
        rb.angularDamping = 5f;
        rb.constraints = RigidbodyConstraints.FreezeRotationX
                       | RigidbodyConstraints.FreezeRotationZ;

        // --- Capsule Collider (body) ---
        CapsuleCollider col = character.AddComponent<CapsuleCollider>();
        col.height = 1.8f;
        col.radius = 0.4f;
        col.center = new Vector3(0, 0.9f, 0);

        // --- Body mesh (Capsule) ---
        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        body.name = "Body";
        body.transform.SetParent(character.transform);
        body.transform.localPosition = new Vector3(0, 0.9f, 0);
        body.transform.localScale = new Vector3(0.8f, 0.9f, 0.8f);
        DestroyImmediate(body.GetComponent<CapsuleCollider>());
        ApplyMaterial(body, new Color(0.25f, 0.47f, 0.85f));

        // --- Head (Sphere) ---
        GameObject head = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        head.name = "Head";
        head.transform.SetParent(character.transform);
        head.transform.localPosition = new Vector3(0, 1.95f, 0);
        head.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
        DestroyImmediate(head.GetComponent<SphereCollider>());
        ApplyMaterial(head, new Color(0.95f, 0.78f, 0.62f));

        // --- Left Arm ---
        CreateLimb(character.transform, "ArmLeft",
            new Vector3(-0.65f, 1.1f, 0),
            new Vector3(0.25f, 0.6f, 0.25f),
            new Color(0.25f, 0.47f, 0.85f));

        // --- Right Arm ---
        CreateLimb(character.transform, "ArmRight",
            new Vector3(0.65f, 1.1f, 0),
            new Vector3(0.25f, 0.6f, 0.25f),
            new Color(0.25f, 0.47f, 0.85f));

        // --- Left Leg ---
        CreateLimb(character.transform, "LegLeft",
            new Vector3(-0.25f, 0.35f, 0),
            new Vector3(0.3f, 0.65f, 0.3f),
            new Color(0.18f, 0.18f, 0.22f));

        // --- Right Leg ---
        CreateLimb(character.transform, "LegRight",
            new Vector3(0.25f, 0.35f, 0),
            new Vector3(0.3f, 0.65f, 0.3f),
            new Color(0.18f, 0.18f, 0.22f));

        // --- Animator placeholder ---
        character.AddComponent<Animator>();

        // --- Audio source for footsteps etc. ---
        character.AddComponent<AudioSource>();

        // Register undo so you can Ctrl+Z it
        Undo.RegisterCreatedObjectUndo(character, "Create 3D Character");

        // Select in hierarchy
        Selection.activeGameObject = character;

        Debug.Log("PlayerCharacter created! Check your Hierarchy.");
    }

    static void CreateLimb(Transform parent, string name,
        Vector3 localPos, Vector3 scale, Color color)
    {
        GameObject limb = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        limb.name = name;
        limb.transform.SetParent(parent);
        limb.transform.localPosition = localPos;
        limb.transform.localScale = scale;
        DestroyImmediate(limb.GetComponent<CapsuleCollider>());
        ApplyMaterial(limb, color);
    }

    static void ApplyMaterial(GameObject go, Color color)
    {
        Renderer r = go.GetComponent<Renderer>();
        Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        if (mat.shader.name == "Hidden/InternalErrorShader")
            mat = new Material(Shader.Find("Standard"));
        mat.color = color;
        r.material = mat;
    }
}