
using Dojo;
using Dojo.Torii;
using dojo_bindings;
using System;
using UnityEngine;

namespace DojoSdk
{
    public class PlayerPosition : ModelInstance, IPlayerState
    {
        public dojo.FieldElement player;
        public UInt32 x;
        public UInt32 y;

        public Vector2Int position => new Vector2Int((int)x, (int)y);
        public string GetPlayerId() => player.ToHex();

        public override void Initialize(Model model)
        {
            player = model.members["player"].ty.ty_primitive.contract_address;
            x = model.members["position"].ty.ty_struct.children[0].ty.ty_primitive.u32;
            y = model.members["position"].ty.ty_struct.children[1].ty.ty_primitive.u32;
        }

        public override void OnUpdated(Model model)
        {
            base.OnUpdated(model);
            Debug.Log($"Position updated to {position})");
        }
    }
}