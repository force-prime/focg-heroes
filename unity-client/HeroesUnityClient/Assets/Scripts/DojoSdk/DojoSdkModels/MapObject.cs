
using Dojo;
using Dojo.Torii;
using dojo_bindings;
using System;
using UnityEngine;
using UnityEngine.XR;

namespace DojoSdk
{
    public class MapObject : ModelInstance, IPlayerState
    {
        public dojo.FieldElement player;
        public UInt32 id;
        public UInt32 descriptionId;

        public string GetPlayerId() => player.ToHex();

        public override void Initialize(Model model)
        {
            player = model.members["player"].ty.ty_primitive.contract_address;
            id = model.members["id"].ty.ty_primitive.u32;
            descriptionId = model.members["description_id"].ty.ty_primitive.u32;
        }

        public override void OnUpdated(Model model)
        {
            base.OnUpdated(model);
        }
    }
}