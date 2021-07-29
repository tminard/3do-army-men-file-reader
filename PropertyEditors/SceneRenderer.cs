using SharpDX.Direct2D1;
using SharpDX.DXGI;
using System;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Factory = SharpDX.Direct2D1.Factory;

namespace AMMEdit.PropertyEditors
{
    class SceneRenderer
    {
        private IntPtr m_displayHandle;

        public SceneRenderer(IntPtr handle)
        {
            this.m_displayHandle = handle;
        }

        public Factory Factory2D { get; private set; }
        public WindowRenderTarget RenderTarget2D { get; private set; }

        public SharpDX.DirectWrite.Factory FactoryDWrite { get; private set; }

        public SolidColorBrush SceneColorBrush { get; private set; }

        private void CreateD2DResources()
        {
            Factory2D = new Factory();
            FactoryDWrite = new SharpDX.DirectWrite.Factory();

            HwndRenderTargetProperties properties = new HwndRenderTargetProperties();
            properties.Hwnd = m_displayHandle;
            properties.PixelSize = new SharpDX.Size2(1024, 768);
            properties.PresentOptions = PresentOptions.None;

            RenderTarget2D = new WindowRenderTarget(Factory2D, new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode.Premultiplied)), properties);

            RenderTarget2D.AntialiasMode = AntialiasMode.PerPrimitive;
        }

        public void BeginDraw()
        {
            RenderTarget2D.BeginDraw();
        }

        public void EndDraw()
        {
            RenderTarget2D.EndDraw();
        }
    }
}
