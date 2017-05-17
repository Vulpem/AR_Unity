using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ConeOfVision : MonoBehaviour
{

    //For more info or questions ask to Roger Olasz or follow the tutorial which are created from this script: https://www.youtube.com/watch?v=rQG9aUWarwE

    public GameObject policeman;
    //Creating variables to create the cone
    public float view_radius;
    [Range(0, 360)]
    public float view_angle;

    //Masks to signalize what are we searching for with raycasts
    public LayerMask projectShadows_mask;
    public LayerMask obstacle_mask;

    //Mesh resolution si how many tris we will have in our cone
    public float mesh_resolution;
    //Edge resolve iterators is to creat an approximation to search the real edge with our raycasts
    public int edge_resolve_iterations;
    //Edge dist threshold is the limit we want to give to the function to know if its viable do iterations between two raycasts
    public float edge_dist_threshold;

    //This is to create and print the mesh we create with triangles
    public MeshFilter view_mesh_filter;
    Mesh view_mesh;

    void Start()
    {
        view_mesh_filter = GetComponent<MeshFilter>();

        //Setting mesh to print what we want
        view_mesh = new Mesh();
        view_mesh.name = "View Mesh";
        view_mesh_filter.mesh = view_mesh;

        //Start a coroutine, more info here: https://docs.unity3d.com/400/Documentation/ScriptReference/index.Coroutines_26_Yield.html
        StartCoroutine("FindTargetsWithDelay", 0.1f);
    }

    //Little function to tell to the "cone": hey man, wait a delay to detect if there are some target on ur area ;)
    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            //Same tutorial of coroutines to understand this line
            yield return new WaitForSeconds(delay);
            //And when we have waited for a delay, then we search for a targets in our cone
        }
    }

    //We do this on LateUpdate to make our mesh smoother, you can put this function into an update() it will work but you will see what i'm saying
    void LateUpdate()
    {
        DrawFieldOfView();
    }

    void DrawFieldOfView()
    {
        //First of all we create a step_count to knwo how many rays we want to cast, very simple
        int step_count = Mathf.RoundToInt(view_angle * mesh_resolution);
        //How many degrees are between each ray
        float step_angle_size = view_angle / step_count;

        //Raycasts we have to different points
        List<Vector3> view_points = new List<Vector3>();
        //Last view cast to compare when we must iterate between 2 rays
        ViewCastInfo old_view_cast = new ViewCastInfo();

        for (int i = 0; i <= step_count; i++)
        {
            float angle = transform.eulerAngles.y - view_angle / 2 + step_angle_size * i;
            ViewCastInfo new_view_cast = ViewCast(angle);

            //If i > 0 we start the optimitzation, having an edge dist threshold find if we are exceeding it
            if (i > 0)
            {
                bool edge_dist_threshold_exceeded = Mathf.Abs(old_view_cast.dist - new_view_cast.dist) > edge_dist_threshold;
                if (old_view_cast.hit != new_view_cast.hit || (old_view_cast.hit && new_view_cast.hit && edge_dist_threshold_exceeded))
                {
                    //If we find vertex with our defined number of iterations it returns two new points which have to be in view_point list
                    EdgeInfo edge = FindEdge(old_view_cast, new_view_cast);
                    if (edge.point_a != Vector3.zero)
                    {
                        view_points.Add(edge.point_a);
                    }
                    if (edge.point_b != Vector3.zero)
                    {
                        view_points.Add(edge.point_b);
                    }
                    //If edges returned are equal to zero is cause we didn't found vertex
                }
            }
            //Still add all regular view points
            view_points.Add(new_view_cast.point);
            old_view_cast = new_view_cast;
        }

        //Here we are creating all triangles with raycasts points
        int num_vertices = view_points.Count + 1;
        Vector3[] vertices = new Vector3[num_vertices];
        int[] triangles = new int[(num_vertices - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < num_vertices - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(view_points[i]);

            if (i < num_vertices - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        view_mesh.Clear();
        view_mesh.vertices = vertices;
        view_mesh.triangles = triangles;
        view_mesh.RecalculateNormals();
    }

    //Finder edge, having X number of iterations we cast any extra rays between two existing trying to find the obstacle vertex
    EdgeInfo FindEdge(ViewCastInfo min_view_cast, ViewCastInfo max_view_cast)
    {
        float min_angle = min_view_cast.angle;
        float max_angle = max_view_cast.angle;
        Vector3 min_point = Vector3.zero;
        Vector3 max_point = Vector3.zero;

        for (int i = 0; i < edge_resolve_iterations; i++)
        {
            float angle = (min_angle + max_angle) / 2;
            ViewCastInfo new_view_cast = ViewCast(angle);

            bool edge_dist_threshold_exceeded = Mathf.Abs(min_view_cast.dist - new_view_cast.dist) > edge_dist_threshold;
            if (new_view_cast.hit == min_view_cast.hit && !edge_dist_threshold_exceeded)
            {
                min_angle = angle;
                min_point = new_view_cast.point;
            }
            else
            {
                max_angle = angle;
                max_point = new_view_cast.point;
            }
        }

        return new EdgeInfo(min_point, max_point);
    }

    //This function is to search obstacles with rays
    ViewCastInfo ViewCast(float global_angle)
    {
        Vector3 dir = DirFromAngle(global_angle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, view_radius, projectShadows_mask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, global_angle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * view_radius, view_radius, global_angle);
        }
    }

    //Find the vec direction with an angle, if angle is not global it must add the character eulerAngles.y to reach the angle we want
    public Vector3 DirFromAngle(float angle_in_degrees, bool angle_is_global)
    {
        if (!angle_is_global)
        {
            angle_in_degrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angle_in_degrees * Mathf.Deg2Rad), 0, Mathf.Cos(angle_in_degrees * Mathf.Deg2Rad));
    }

    //Info of rays we cast
    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dist;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dist, float _angle)
        {
            hit = _hit;
            point = _point;
            dist = _dist;
            angle = _angle;

        }
    }

    //Struct to find vertex/edges of obstacles
    public struct EdgeInfo
    {
        public Vector3 point_a;
        public Vector3 point_b;

        public EdgeInfo(Vector3 _point_a, Vector3 _point_b)
        {
            point_a = _point_a;
            point_b = _point_b;
        }
    }

}