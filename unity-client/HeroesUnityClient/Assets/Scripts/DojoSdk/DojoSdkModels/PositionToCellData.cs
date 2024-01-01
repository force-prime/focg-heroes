
using Dojo;
using Dojo.Torii;
using dojo_bindings;
using System;
using UnityEngine;

namespace DojoSdk
{
    public class PositionToCellData : ModelInstance
    {
        public UInt32 x;
        public UInt32 y;

        public UInt32 tileId;
        public UInt32 objectId;

        public Vector2Int position => new Vector2Int((int)x, (int)y);

        public override void Initialize(Model model)
        {
            x = model.members["position"].ty.ty_struct.children[0].ty.ty_primitive.u32;
            y = model.members["position"].ty.ty_struct.children[1].ty.ty_primitive.u32;
            tileId = model.members["tile_id"].ty.ty_primitive.u32;
            objectId = model.members["object_id"].ty.ty_primitive.u32;
        }

        public override void OnUpdated(Model model)
        {
            base.OnUpdated(model);
        }
    }
}