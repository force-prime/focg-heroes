
using Dojo;
using Dojo.Torii;
using dojo_bindings;
using System;
using UnityEngine;

namespace DojoSdk
{
    public class PlayerUnits : ModelInstance, IPlayerState
    {
        public dojo.FieldElement player;

        public UInt32 count;
        public UInt32 unitId;

        public string GetPlayerId() => player.ToHex();

        public override void Initialize(Model model)
        {
            player = model.members["player"].ty.ty_primitive.contract_address;
            count = model.members["count"].ty.ty_primitive.u32;
            unitId = model.members["unit_id"].ty.ty_primitive.u32;
        }

        public override void OnUpdated(Model model)
        {
            base.OnUpdated(model);
            Debug.Log($"Units {unitId} updated to: {count})");
        }
    }
}