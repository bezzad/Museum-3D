using System;
using System.Windows.Forms;
using System.Text;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Museum
{
    class InitializeGraphics
    {
        private PresentParameters pp;

        public InitializeGraphics()
        {
            pp = new PresentParameters();
            pp.Windowed = true;
            pp.SwapEffect = SwapEffect.Discard;
            pp.AutoDepthStencilFormat = DepthFormat.D16;
            pp.EnableAutoDepthStencil = true;
        }

        public Device getDevice(Form frm)
        {
            Caps DevCaps = Manager.GetDeviceCaps(0, DeviceType.Hardware);
            DeviceType DevType = DeviceType.Reference;
            CreateFlags DevFlags = CreateFlags.SoftwareVertexProcessing;
            if (DevCaps.PixelShaderVersion >= new Version(2, 0))
            {
                DevType = DeviceType.Hardware;
                if (DevCaps.DeviceCaps.SupportsHardwareTransformAndLight)
                {
                    DevFlags = CreateFlags.HardwareVertexProcessing;
                    if (DevCaps.DeviceCaps.SupportsPureDevice)
                    {
                        DevFlags |= CreateFlags.PureDevice;
                    }
                }
            }
            Device device3d = new Device(0, DevType, frm, DevFlags, pp);
            return (device3d);
        }        
    }
}
