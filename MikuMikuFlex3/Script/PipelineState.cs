﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using SharpDX.Direct3D11;

namespace MikuMikuFlex3.Script
{
    /// <summary>
    ///     パイプラインステート。
    ///     スクリプトに渡されるホストオブジェクト。
    /// </summary>
    public class PipelineState : IDisposable
    {

        // スクリプト（Initialize）向け


        public void CreateVertexShader( object key, string csoFilePath )
        {
            this.RemoveVetexShader( key );

            this._CreateShader( csoFilePath, ( b ) => this._VertexShaderes[ key ] = new VertexShader( this._d3dDevice, b ) );
        }

        public void CreateHullShader( object key, string csoFilePath )
        {
            this.RemoveHullShader( key );

            this._CreateShader( csoFilePath, ( b ) => this._HullShaderes[ key ] = new HullShader( this._d3dDevice, b ) );
        }

        public void CreateDomainShader( object key, string csoFilePath )
        {
            this.RemoveDomainShader( key );

            this._CreateShader( csoFilePath, ( b ) => this._DomainShaderes[ key ] = new DomainShader( this._d3dDevice, b ) );
        }

        public void CreateGeometryShader( object key, string csoFilePath )
        {
            this.RemoveGeometryShader( key );

            this._CreateShader( csoFilePath, ( b ) => this._GeometryShaderes[ key ] = new GeometryShader( this._d3dDevice, b ) );
        }

        public void CreatePixelShader( object key, string csoFilePath )
        {
            this.RemovePixelShader( key );

            this._CreateShader( csoFilePath, ( b ) => this._PixelShaderes[ key ] = new PixelShader( this._d3dDevice, b ) );
        }

        public void CreateComputeShader( object key, string csoFilePath )
        {
            this.RemoveComputeShader( key );

            this._CreateShader( csoFilePath, ( b ) => this._ComputeShaderes[ key ] = new ComputeShader( this._d3dDevice, b ) );
        }


        public void RemoveVetexShader( object key )
        {
            if( this._VertexShaderes.ContainsKey( key ) )
            {
                this._VertexShaderes[ key ]?.Dispose();
                this._VertexShaderes.Remove( key );
            }
        }

        public void RemoveHullShader( object key )
        {
            if( this._HullShaderes.ContainsKey( key ) )
            {
                this._HullShaderes[ key ]?.Dispose();
                this._HullShaderes.Remove( key );
            }
        }

        public void RemoveDomainShader( object key )
        {
            if( this._DomainShaderes.ContainsKey( key ) )
            {
                this._DomainShaderes[ key ]?.Dispose();
                this._DomainShaderes.Remove( key );
            }
        }

        public void RemoveGeometryShader( object key )
        {
            if( this._GeometryShaderes.ContainsKey( key ) )
            {
                this._GeometryShaderes[ key ]?.Dispose();
                this._GeometryShaderes.Remove( key );
            }
        }

        public void RemovePixelShader( object key )
        {
            if( this._PixelShaderes.ContainsKey( key ) )
            {
                this._PixelShaderes[ key ]?.Dispose();
                this._PixelShaderes.Remove( key );
            }
        }

        public void RemoveComputeShader( object key )
        {
            if( this._ComputeShaderes.ContainsKey( key ) )
            {
                this._ComputeShaderes[ key ]?.Dispose();
                this._ComputeShaderes.Remove( key );
            }
        }


        // スクリプト（Run）向け


        public MMDPass MMDPass;

        public void SetVertexShader( object key )
        {
            this._選択中のVertexShader = key;
        }

        public void SetHullShader( object key )
        {
            this._選択中のHullShader = key;
        }

        public void SetDomainShader( object key )
        {
            this._選択中のDomainShader = key;
        }

        public void SetGeometryShader( object key )
        {
            this._選択中のGeometryShader = key;
        }

        public void SetPixelShader( object key )
        {
            this._選択中のPixelShader = key;
        }

        public void SetComputeShader( object key )
        {
            this._選択中のComputeShader = key;
        }


        // 材質エフェクト用
        public void Draw()
        {
            // 選択されたシェーダーが既定値(null) ではなかったら設定する。

            if( null != this._選択中のVertexShader && this._VertexShaderes.ContainsKey( this._選択中のVertexShader ) )
                this._d3ddc.VertexShader.Set( this._VertexShaderes[ this._選択中のVertexShader ] );

            if( null != this._選択中のHullShader && this._HullShaderes.ContainsKey( this._選択中のHullShader ) )
                this._d3ddc.HullShader.Set( this._HullShaderes[ this._選択中のHullShader ] );

            if( null != this._選択中のDomainShader && this._DomainShaderes.ContainsKey( this._選択中のDomainShader ) )
                this._d3ddc.DomainShader.Set( this._DomainShaderes[ this._選択中のDomainShader ] );

            if( null != this._選択中のGeometryShader && this._GeometryShaderes.ContainsKey( this._選択中のGeometryShader ) )
                this._d3ddc.GeometryShader.Set( this._GeometryShaderes[ this._選択中のGeometryShader ] );

            if( null != this._選択中のPixelShader && this._PixelShaderes.ContainsKey( this._選択中のPixelShader ) )
                this._d3ddc.PixelShader.Set( this._PixelShaderes[ this._選択中のPixelShader ] );


            // 描画する。

            this._d3ddc.DrawIndexed( this._頂点数, this._頂点の開始インデックス, 0 );
        }

        // ポストエフェクト用
        public void Blit( int threadGroupCountX, int threadGroupCountY, int threadGroupCountZ )
        {
            // 選択されたシェーダーが既定値(null) ではなかったら設定する。

            if( null != this._選択中のComputeShader && this._ComputeShaderes.ContainsKey( this._選択中のComputeShader ) )
                this._d3ddc.ComputeShader.Set( this._ComputeShaderes[ this._選択中のComputeShader ] );


            // 実行する。

            this._d3ddc.Dispatch( threadGroupCountX, threadGroupCountY, threadGroupCountZ );
        }



        // 内部向け


        public Reason Reason = Reason.Initialize;


        public PipelineState( Device d3dDevice )
        {
            this._d3dDevice = d3dDevice;
            this._DefaultMaterialShader = new DefaultMaterialShader( d3dDevice );
        }

        public virtual void Dispose()
        {
            foreach( var kvp in this._VertexShaderes )
                kvp.Value.Dispose();

            foreach( var kvp in this._HullShaderes )
                kvp.Value.Dispose();

            foreach( var kvp in this._DomainShaderes )
                kvp.Value.Dispose();

            foreach( var kvp in this._GeometryShaderes )
                kvp.Value.Dispose();

            foreach( var kvp in this._PixelShaderes )
                kvp.Value.Dispose();

            this._DefaultMaterialShader?.Dispose();
            this._d3ddc = null;     // Disposeしない
            this._d3dDevice = null; // Disposeしない
        }

        public void SetDrawState( int 頂点数, int 頂点の開始インデックス, MMDPass pass種別, DeviceContext d3ddc )
        {
            this._頂点数 = 頂点数;
            this._頂点の開始インデックス = 頂点の開始インデックス;
            this.MMDPass = pass種別;
            this._d3ddc = d3ddc;

            // 既定のパイプラインステートを設定。

            switch( pass種別 )
            {
                case MMDPass.Edge:
                    this._d3ddc.VertexShader.Set( this._DefaultMaterialShader.VertexShaderForEdge );    // Edge
                    this._d3ddc.HullShader.Set( this._DefaultMaterialShader.HullShader );
                    this._d3ddc.DomainShader.Set( this._DefaultMaterialShader.DomainShader );
                    this._d3ddc.GeometryShader.Set( this._DefaultMaterialShader.GeometryShader );
                    this._d3ddc.PixelShader.Set( this._DefaultMaterialShader.PixelShaderForEdge );      // Edge
                    this._d3ddc.OutputMerger.BlendState = this._DefaultMaterialShader.BlendState通常合成;
                    break;

                default:
                    this._d3ddc.VertexShader.Set( this._DefaultMaterialShader.VertexShaderForObject );
                    this._d3ddc.HullShader.Set( this._DefaultMaterialShader.HullShader );
                    this._d3ddc.DomainShader.Set( this._DefaultMaterialShader.DomainShader );
                    this._d3ddc.GeometryShader.Set( this._DefaultMaterialShader.GeometryShader );
                    this._d3ddc.PixelShader.Set( this._DefaultMaterialShader.PixelShaderForObject );
                    this._d3ddc.OutputMerger.BlendState = this._DefaultMaterialShader.BlendState通常合成;
                    break;
            }

            this._選択中のVertexShader = null;
            this._選択中のHullShader = null;
            this._選択中のDomainShader = null;
            this._選択中のGeometryShader = null;
            this._選択中のPixelShader = null;
        }

        public void SetBlitState( DeviceContext d3ddc )
        {
            this._d3ddc = d3ddc;
            this._d3ddc.ComputeShader.Set( null );
            this._選択中のComputeShader = null;
        }


        protected int _頂点数;

        protected int _頂点の開始インデックス;

        protected DeviceContext _d3ddc;

        protected Device _d3dDevice;

        protected DefaultMaterialShader _DefaultMaterialShader;


        protected Dictionary<object, VertexShader> _VertexShaderes = new Dictionary<object, VertexShader>();

        protected Dictionary<object, HullShader> _HullShaderes = new Dictionary<object, HullShader>();

        protected Dictionary<object, DomainShader> _DomainShaderes = new Dictionary<object, DomainShader>();

        protected Dictionary<object, GeometryShader> _GeometryShaderes = new Dictionary<object, GeometryShader>();

        protected Dictionary<object, PixelShader> _PixelShaderes = new Dictionary<object, PixelShader>();

        protected Dictionary<object, ComputeShader> _ComputeShaderes = new Dictionary<object, ComputeShader>();

        protected object _選択中のVertexShader = null;

        protected object _選択中のHullShader = null;

        protected object _選択中のDomainShader = null;

        protected object _選択中のGeometryShader = null;

        protected object _選択中のPixelShader = null;

        protected object _選択中のComputeShader = null;


        protected void _CreateShader( string csoFilePath, Action<byte[]> create )
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();

                using( var fs = new FileStream( csoFilePath, FileMode.Open, FileAccess.Read, FileShare.Read ) )
                {
                    var buffer = new byte[ fs.Length ];
                    fs.Read( buffer, 0, buffer.Length );
                    create( buffer );
                }
            }
            catch( Exception e )
            {
                Trace.TraceError( $"ファイルからのシェーダーの作成に失敗しました。[{csoFilePath}][{e.Message}]" );
            }
        }
    }
}
