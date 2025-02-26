using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;

public class BoundaryVisualizer : MonoBehaviour
{
    [Header("Gizmos (Editor Only)")]
    [SerializeField] private bool drawGizmos = true;
    [SerializeField] private Color gizmoColor = Color.cyan;

    [Header("Runtime Visualization")]
    [SerializeField] private bool useLineRenderer = true;
    [SerializeField] private LineRenderer boundaryLine;
    [SerializeField] private Material lineMaterial;

    private Vector3[] boundaryPoints;

    private void Start()
    {
        var loader = XRGeneralSettings.Instance?.Manager?.activeLoader;
        if (loader == null)
        {
            Debug.LogWarning("⚠️⚠️⚠️ Could not get active Loader.");
            return;
        }
        var inputSubsystem = loader.GetLoadedSubsystem<XRInputSubsystem>();
        inputSubsystem.boundaryChanged += InputSubsystem_boundaryChanged;

        if (useLineRenderer)
        {
            InitializeLineRenderer();
        }
        FetchBoundary();
    }

    private void InputSubsystem_boundaryChanged(XRInputSubsystem subsystem)
    {
        var inputSubsystem = XRGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<XRInputSubsystem>();
        if (inputSubsystem == null)
        {
            Debug.LogWarning("⚠️⚠️⚠️ No XRInputSubsystem found.");
            return;
        }

        List<Vector3> boundaryPoints = new List<Vector3>();
        inputSubsystem.TryGetBoundaryPoints(boundaryPoints);
        Debug.Log("⚠️⚠️⚠️ OpenXR");
        Debug.Log($"⚠️⚠️⚠️ Retrieved {boundaryPoints.Count} OpenXR boundary points: {string.Join(", ", boundaryPoints)}");

    }

    private void FetchBoundary()
    {
        if (!OVRManager.boundary.GetConfigured())
        {
            Debug.LogWarning("Boundary not configured.");
            return;
        }

        boundaryPoints = OVRManager.boundary.GetGeometry(OVRBoundary.BoundaryType.PlayArea);
        /* Convert from Oculus right-handed to Unity left-handed coordinate system */
        for (int i = 0; i < boundaryPoints.Length; i++)
        {
            boundaryPoints[i] = new Vector3(boundaryPoints[i].x, boundaryPoints[i].y, boundaryPoints[i].z * -1);
        }

        /* Apply tracking origin offset (player's position/rotation) */
        Transform trackingOrigin = OVRManager.instance.transform;
        var pose = OVRManager.tracker.GetPose();
        Debug.Log($"⚠️⚠️⚠️ Tracking Pose: {pose.position}, {pose.orientation}");
        Debug.Log($"⚠️⚠️⚠️ Tracking Origin: {trackingOrigin.position}, {trackingOrigin.rotation}");
        // for (int i = 0; i < boundaryPoints.Length; i++)
        // {
        //     boundaryPoints[i] = trackingOrigin.TransformPoint(boundaryPoints[i]);
        // }

        if (useLineRenderer && boundaryLine != null)
        {
            UpdateLineRenderer();
        }
    }

    // Gizmos (Visible in Scene View)
    private void OnDrawGizmos()
    {
        if (!drawGizmos || boundaryPoints == null || boundaryPoints.Length < 2) return;

        Gizmos.color = gizmoColor;
        for (int i = 0; i < boundaryPoints.Length; i++)
        {
            Vector3 current = transform.TransformPoint(boundaryPoints[i]);
            Vector3 next = transform.TransformPoint(boundaryPoints[(i + 1) % boundaryPoints.Length]);
            Gizmos.DrawLine(current, next);
        }
    }

    // LineRenderer Setup (Runtime)
    private void InitializeLineRenderer()
    {
        if (boundaryLine == null)
        {
            boundaryLine = gameObject.AddComponent<LineRenderer>();
            boundaryLine.loop = true;
            boundaryLine.positionCount = 0;
            boundaryLine.material = lineMaterial;
            boundaryLine.startWidth = boundaryLine.endWidth = 0.01f;
        }
    }

    private void UpdateLineRenderer()
    {
        boundaryLine.positionCount = boundaryPoints.Length;
        for (int i = 0; i < boundaryPoints.Length; i++)
        {
            Debug.Log($"⚠️⚠️⚠️ Boundary Point {i}: {boundaryPoints[i]}");
            boundaryLine.SetPosition(i, transform.TransformPoint(boundaryPoints[i]));
        }
        // boundaryLine.SetPosition(boundaryPoints.Length, boundaryPoints[0]); // Close loop
    }
}
