using Dojo;
using Dojo.Torii;
using dojo_bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DojoSdk
{
    public class ResourceDescription : ModelInstance
    {
        public UInt32 id;
        public UInt32 resId;
        public UInt32 count;

        public override void Initialize(Model model)
        {
            id = model.members["id"].ty.ty_primitive.u32;
            resId = model.members["resource_id"].ty.ty_primitive.u32;
            count = model.members["count"].ty.ty_primitive.u32;
        }

        public override void OnUpdated(Model model)
        {
            base.OnUpdated(model);
        }
    }
}
