
using Dojo;
using Dojo.Torii;
using dojo_bindings;
using System;
using UnityEngine;

namespace DojoSdk
{
    public class PlayerBalance : ModelInstance, IPlayerState
    {
        public dojo.FieldElement player;
        public UInt32 gold;

        public string GetPlayerId() => player.ToHex();

        public override void Initialize(Model model)
        {
            player = model.members["player"].ty.ty_primitive.contract_address;
            gold = model.members["gold"].ty.ty_primitive.u32;
        }

        public override void OnUpdated(Model model)
        {
            base.OnUpdated(model);
            Debug.Log($"Gold updated to {gold})");
        }
    }
}