﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using SharpDX;

namespace MikuMikuFlex3.PMXFormat
{
    public class 基本6DOFジョイントパラメータ : ジョイントパラメータ
    {
        public int 関連剛体Aのインデックス { get; private set; }

        public int 関連剛体Bのインデックス { get; private set; }

        public Vector3 位置 { get; private set; }

        /// <summary>
        ///     (x,y,z) -> ラジアン角
        /// </summary>
        public Vector3 回転rad { get; private set; }

        public Vector3 移動制限の下限 { get; private set; }

        public Vector3 移動制限の上限 { get; private set; }

        /// <summary>
        ///     (x,y,z) -> ラジアン角
        /// </summary>
        public Vector3 回転制限の下限rad { get; private set; }

        /// <summary>
        ///     (x,y,z) -> ラジアン角
        /// </summary>
        public Vector3 回転制限の上限rad { get; private set; }


        /// <summary>
        ///     指定されたストリームから読み込む。
        /// </summary>
        internal override void 読み込む( Stream fs, ヘッダ header )
        {
            this.関連剛体Aのインデックス = ParserHelper.get_Index( fs, header.剛体インデックスサイズ );
            this.関連剛体Bのインデックス = ParserHelper.get_Index( fs, header.剛体インデックスサイズ );
            this.位置 = ParserHelper.get_Float3( fs );
            this.回転rad = ParserHelper.get_Float3( fs );
            this.移動制限の下限 = ParserHelper.get_Float3( fs );
            this.移動制限の上限 = ParserHelper.get_Float3( fs );
            this.回転制限の下限rad = ParserHelper.get_Float3( fs );
            this.回転制限の上限rad = ParserHelper.get_Float3( fs );
            ParserHelper.get_Float3( fs );   //6DOF は、 回転・平行移動バネ定数が無効なので読み取ってシーク
            ParserHelper.get_Float3( fs );
        }
    }
}
