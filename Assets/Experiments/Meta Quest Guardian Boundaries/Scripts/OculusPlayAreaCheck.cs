using UnityEngine;
// using Unity.XR.Oculus;
using System.Collections.Generic;
using UnityEngine.XR.Management;
using UnityEngine.XR;
using System.Linq;
using System;
using System.Runtime.InteropServices;
using UnityEngine.Serialization;
using UnityEngine.Rendering;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if USING_XR_SDK
using UnityEngine.XR;
using UnityEngine.Experimental.XR;
#endif

#if USING_XR_SDK_OPENXR
using Meta.XR;
using UnityEngine.XR.OpenXR;
#endif

#if USING_XR_MANAGEMENT
using UnityEngine.XR.Management;
#endif

#if USING_URP
using UnityEngine.Rendering.Universal;
#endif

public class OculusPlayAreaCheck : MonoBehaviour
{
    void Start()
    {
        UsingOVRBoundary();
        UsingOVRManager();
    }

    private void UsingOVRBoundary()
    {
        // var display = new OVRDisplay();
        // var tracker = new OVRTracker();
        var ovrBoundary = new OVRBoundary();
        var dimensions = ovrBoundary.GetDimensions(OVRBoundary.BoundaryType.PlayArea);
        var boundaryPoints = ovrBoundary.GetGeometry(OVRBoundary.BoundaryType.PlayArea);

        if (boundaryPoints == null) //  || boundaryPoints.Count() < 3
        {
            Debug.LogWarning("⚠️⚠️⚠️ Failed to retrieve valid boundary geometry.");
            return;
        }

        Debug.Log($"⚠️⚠️⚠️ Retrieved boundary dimensions: {dimensions.x}, {dimensions.y}, {dimensions.z}");
        Debug.Log($"⚠️⚠️⚠️ Retrieved {boundaryPoints.Count()} boundary points: {string.Join(", ", boundaryPoints)}");
    }

    private void UsingOVRManager()
    {
        Debug.Log($"⚠️⚠️⚠️ OVRManager: {nameof(OVRManager)}");
        Debug.Log($"⚠️⚠️⚠️ OVRManager.boundary: {OVRManager.boundary}");
        if (!OVRManager.boundary.GetConfigured())
        {
            Debug.LogWarning("⚠️⚠️⚠️ Boundary not configured.");
            return;
        }
        var boundary = OVRManager.boundary.GetDimensions(OVRBoundary.BoundaryType.PlayArea);
        var points = OVRManager.boundary.GetGeometry(OVRBoundary.BoundaryType.PlayArea);

        Debug.Log($"⚠️⚠️⚠️ Boundary dimensions: {boundary.x}, {boundary.y}, {boundary.z}");
        Debug.Log($"⚠️⚠️⚠️ Boundary points: {string.Join(", ", points)}");
    }
}
