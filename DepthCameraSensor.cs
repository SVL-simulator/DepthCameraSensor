/**
 * Copyright (c) 2019-2021 LG Electronics, Inc.
 *
 * This software contains code licensed as described in LICENSE.
 *
 */

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using Simulator.Bridge.Data;
using Simulator.Utilities;
using Simulator.PointCloud;

namespace Simulator.Sensors
{
    [SensorType("Depth Camera", new[] {typeof(ImageData)})]
    public class DepthCameraSensor : CameraSensorBase
    {
        private ShaderTagId passId;
        
        protected override void Initialize()
        {
            base.Initialize();
            passId = new ShaderTagId("SimulatorDepthPass");
            CameraTargetTextureReadWriteType = RenderTextureReadWrite.Linear;
            SensorCamera.GetComponent<HDAdditionalCameraData>().customRender += CustomRender;
        }

        void CustomRender(ScriptableRenderContext context, HDCamera hd)
        {
            var cmd = CommandBufferPool.Get();
            SensorPassRenderer.Render(context, cmd, hd, renderTarget, passId, Color.clear);
            if (!Fisheye)
            {
                PointCloudManager.RenderDepth(context, cmd, hd, renderTarget.ColorHandle, renderTarget.DepthHandle);
            }
            CommandBufferPool.Release(cmd);
        }

        public override bool Save(string path, int quality, int compression)
        {
            // Hide base.Save() since DepthCameraSensor does not support Save function for now.
            return false;
        }
    }
}
